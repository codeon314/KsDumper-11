using KsDumper11.Driver;
using KsDumper11.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KsDumper11
{
    public partial class KernelModulesForm : Form
    {
        private readonly KernelDriverOperations _driverOps;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 33554432; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public KernelModulesForm()
        {
            InitializeComponent();

            try
            {
                _driverOps = new KernelDriverOperations();
            }
            catch (Exception ex)
            {
                _driverOps = null;
                MessageBox.Show(
                    "Unable to open kernel driver device: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KernelModulesForm_Load(object sender, EventArgs e)
        {
            RefreshModules();

            kdFormFix.Start();
        }

        private void kdFormFix_Tick(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width + 3, this.Size.Height);

            this.Invalidate();

            kdFormFix.Stop();
        }

        private void RefreshModules()
        {
            moduleList.Items.Clear();

            if (_driverOps == null || !_driverOps.IsOpen)
                return;

            try
            {
                KERNEL_DRIVER_INFO[] modules = _driverOps.GetKernelModules();

                foreach (KERNEL_DRIVER_INFO mod in modules)
                {
                    if (mod.BaseAddress == 0) continue;

                    string fileName = "Unknown";
                    try
                    {
                        if (!string.IsNullOrEmpty(mod.FullPathName))
                            fileName = Path.GetFileName(mod.FullPathName);
                    }
                    catch { }

                    ListViewItem item = new ListViewItem(fileName);
                    item.SubItems.Add(string.Format("0x{0:X16}", mod.BaseAddress));
                    item.SubItems.Add(string.Format("0x{0:X}", mod.SizeOfImage));
                    item.SubItems.Add(mod.FullPathName ?? "");
                    item.Tag = mod;
                    moduleList.Items.Add(item);
                }

                // Auto-size every column so header text and widest subitem are
                // always fully visible (no ellipsis truncation anywhere).
                // Width = -2 sizes each column to fit its header and longest
                // subitem; re-applied on every refresh so widths stay fitted.
                for (int i = 0; i < moduleList.Columns.Count; i++)
                {
                    moduleList.Columns[i].Width = -2;
                }

                Logger.Log(string.Format("Enumerated {0} kernel drivers.", moduleList.Items.Count));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error enumerating kernel drivers: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshModules();
        }

        private void dumpDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (moduleList.SelectedItems.Count == 0) return;
            if (_driverOps == null || !_driverOps.IsOpen) return;

            ListViewItem item = moduleList.SelectedItems[0];
            KERNEL_DRIVER_INFO info = (KERNEL_DRIVER_INFO)item.Tag;

            if (info.SizeOfImage == 0)
            {
                MessageBox.Show("Driver has zero image size; cannot dump.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Logger.Log(string.Format(
                    "Dumping kernel driver {0} (0x{1:X16}, {2} bytes)...",
                    item.Text, info.BaseAddress, info.SizeOfImage));

                // SINGLE kernel snapshot. Both the .sys (code-only PE built
                // from PE walking) and the .bin (full runtime image) are
                // derived from this one buffer so they represent the same
                // instant in time.
                byte[] snapshot = _driverOps.DumpKernelModule(info.BaseAddress, (int)info.SizeOfImage);

                if (snapshot == null || snapshot.Length == 0)
                {
                    MessageBox.Show("Driver returned no data.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Walk the PE headers of the in-memory snapshot and produce a
                // valid PE image that contains ONLY the executable code
                // sections. This produces a *compacted* file: non-executable
                // sections are removed entirely (not zero-filled), and the
                // PE section table is rewritten so the resulting file is a
                // smaller, self-contained PE whose section RVAs still match
                // the original image layout.
                byte[] codeOnlyPe = BuildCodeOnlyPe(snapshot);

                string baseName = Path.GetFileNameWithoutExtension(item.Text);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.FileName = baseName + ".sys";
                    sfd.Filter = "Driver binary (*.sys)|*.sys|All Files (*.*)|*.*";
                    sfd.Title = "Save driver code dump (writes .sys and .bin)";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    string sysPath = sfd.FileName;
                    string binPath = Path.ChangeExtension(sysPath, ".bin");

                    // 1. Runtime memory dump -> .bin (full image, unmodified)
                    File.WriteAllBytes(binPath, snapshot);
                    Logger.Log(string.Format(
                        "Saved runtime memory dump ({0} bytes) to {1}",
                        snapshot.Length, binPath));

                    // 2. Executable code from kernel memory -> .sys
                    if (codeOnlyPe != null && codeOnlyPe.Length > 0)
                    {
                        File.WriteAllBytes(sysPath, codeOnlyPe);
                        Logger.Log(string.Format(
                            "Saved executable code image ({0} bytes) to {1}",
                            codeOnlyPe.Length, sysPath));
                    }
                    else
                    {
                        // PE walk failed (stripped/invalid headers). Fall back
                        // to writing the raw snapshot so the user still gets
                        // something usable.
                        File.WriteAllBytes(sysPath, snapshot);
                        Logger.Log("PE headers invalid; wrote raw snapshot as .sys");
                    }

                    MessageBox.Show(
                        string.Format("Dump complete.\n\n.sys (code): {0} ({1} bytes)\n.bin (full): {2} ({3} bytes)",
                            sysPath,
                            codeOnlyPe != null ? codeOnlyPe.Length : snapshot.Length,
                            binPath,
                            snapshot.Length),
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Kernel driver dump failed: " + ex.Message);
                MessageBox.Show("Failed to dump driver: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -----------------------------------------------------------------
        // BuildCodeOnlyPe
        //
        // Walk the PE of the in-memory image and return a NEW, compacted PE
        // that contains ONLY sections flagged with IMAGE_SCN_MEM_EXECUTE.
        //
        // Layout of the produced file:
        //   [DOS header + DOS stub]
        //   [PE signature + COFF header (NumberOfSections rewritten)]
        //   [Optional header (SizeOfImage / SizeOfHeaders rewritten)]
        //   [executable section header 0..N-1  (PointerToRawData / SizeOfRawData rewritten)]
        //   <file-alignment padding>
        //   [executable section 0 body]
        //   [executable section 1 body]
        //   ...
        //
        // RVAs of the kept sections are NOT relocated.  This means a loader
        // that maps the .sys at the driver's original image base will still
        // resolve intra-section RIP-relative references to code that lives
        // in another kept code section.  Data references to removed sections
        // (.data, .rdata, .pdata, ...) will read zeroes, which is expected
        // for a code-only dump.
        // -----------------------------------------------------------------
        private static byte[] BuildCodeOnlyPe(byte[] image)
        {
            const uint IMAGE_SCN_MEM_EXECUTE = 0x20000000;

            if (image == null || image.Length < 0x100)
                return null;

            // --- DOS header -> e_lfanew at 0x3C ---
            int e_lfanew;
            try
            {
                e_lfanew = BitConverter.ToInt32(image, 0x3C);
            }
            catch
            {
                return null;
            }

            if (e_lfanew <= 0 || e_lfanew + 0x18 > image.Length)
                return null;

            // --- PE signature "PE\0\0" ---
            uint signature = BitConverter.ToUInt32(image, e_lfanew);
            if (signature != 0x00004550)
                return null;

            // --- COFF header ---
            int coffOffset = e_lfanew + 4;
            ushort numberOfSections = BitConverter.ToUInt16(image, coffOffset + 2);
            ushort sizeOfOptionalHeader = BitConverter.ToUInt16(image, coffOffset + 16);

            if (numberOfSections == 0 || numberOfSections > 96)
                return null;

            int optionalHeaderOffset = coffOffset + 20;
            int sectionsOffset = optionalHeaderOffset + sizeOfOptionalHeader;

            if (sectionsOffset + (numberOfSections * 40) > image.Length)
                return null;

            // --- Alignment values from the Optional Header ---
            // SectionAlignment: offset 32; FileAlignment: offset 36; these are
            // identical for PE32 and PE32+.
            uint sectionAlignment = BitConverter.ToUInt32(image, optionalHeaderOffset + 32);
            uint fileAlignment = BitConverter.ToUInt32(image, optionalHeaderOffset + 36);
            if (sectionAlignment == 0) sectionAlignment = 0x1000;
            if (fileAlignment == 0) fileAlignment = 0x200;

            // --- Identify executable sections ---
            List<int> codeSectionIndices = new List<int>();
            for (int i = 0; i < numberOfSections; i++)
            {
                int shOffset = sectionsOffset + (i * 40);
                uint characteristics = BitConverter.ToUInt32(image, shOffset + 36);

                if ((characteristics & IMAGE_SCN_MEM_EXECUTE) != 0)
                {
                    codeSectionIndices.Add(i);
                }
            }

            if (codeSectionIndices.Count == 0)
                return null;

            // --- New header size ---
            int newPeHeadersEnd = sectionsOffset + codeSectionIndices.Count * 40;

            // First section body starts at the next file-alignment boundary
            // after the new headers.
            int firstSectionFileOffset =
                (int)(((uint)newPeHeadersEnd + fileAlignment - 1) / fileAlignment * fileAlignment);

            // --- Prepare output buffer ---
            // Start with enough room for the new headers.
            byte[] output = new byte[firstSectionFileOffset];

            // Copy the DOS header, stub, PE signature, COFF, optional header,
            // and (incidentally) the first codeSectionIndices.Count original
            // section header slots.  We overwrite those slots below with the
            // headers of the sections we actually want to keep.
            Array.Copy(image, 0, output, 0, newPeHeadersEnd);

            // Rewrite COFF NumberOfSections.
            output[coffOffset + 2] = (byte)(codeSectionIndices.Count & 0xFF);
            output[coffOffset + 3] = (byte)((codeSectionIndices.Count >> 8) & 0xFF);

            // --- Emit each code section header + body ---
            int outputLen = firstSectionFileOffset;
            uint currentFileOffset = (uint)firstSectionFileOffset;
            uint newSizeOfImage = 0;

            for (int newIdx = 0; newIdx < codeSectionIndices.Count; newIdx++)
            {
                int origIdx = codeSectionIndices[newIdx];
                int origShOffset = sectionsOffset + (origIdx * 40);
                int newShOffset = sectionsOffset + (newIdx * 40);

                // Copy the 40-byte section header into its new slot.
                Array.Copy(image, origShOffset, output, newShOffset, 40);

                uint virtualSize = BitConverter.ToUInt32(image, origShOffset + 8);
                uint virtualAddress = BitConverter.ToUInt32(image, origShOffset + 12);

                // SizeOfRawData must cover the section's virtual size, aligned
                // to FileAlignment, and be at least one FileAlignment unit.
                uint alignedVS = ((virtualSize + fileAlignment - 1) / fileAlignment) * fileAlignment;
                if (alignedVS < fileAlignment) alignedVS = fileAlignment;
                uint newSizeOfRawData = alignedVS;

                // Patch SizeOfRawData (offset 16) and PointerToRawData (offset 20).
                byte[] sizeBytes = BitConverter.GetBytes(newSizeOfRawData);
                byte[] ptrBytes = BitConverter.GetBytes(currentFileOffset);
                for (int k = 0; k < 4; k++)
                {
                    output[newShOffset + 16 + k] = sizeBytes[k];
                    output[newShOffset + 20 + k] = ptrBytes[k];
                }

                // Grow the output buffer to fit the section body.  Zero-fill
                // any gap between the previous end and the new section start.
                int neededLen = (int)(currentFileOffset + newSizeOfRawData);
                if (output.Length < neededLen)
                {
                    byte[] grown = new byte[neededLen];
                    Array.Copy(output, grown, outputLen);
                    output = grown;
                }
                for (int k = outputLen; k < (int)currentFileOffset; k++)
                {
                    output[k] = 0;
                }

                // Copy section content from the in-memory image.  Bound-check
                // against the snapshot in case SizeOfImage was short.
                long srcStart = virtualAddress;
                long srcLen = virtualSize;
                if (srcStart < 0 || srcStart >= image.Length)
                {
                    srcLen = 0;
                }
                else if (srcStart + srcLen > image.Length)
                {
                    srcLen = image.Length - srcStart;
                }

                if (srcLen > 0)
                {
                    Array.Copy(image, (int)srcStart, output, (int)currentFileOffset, (int)srcLen);
                }

                // Track the effective SizeOfImage.
                uint sectionEnd = virtualAddress + alignedVS;
                if (sectionEnd > newSizeOfImage) newSizeOfImage = sectionEnd;

                currentFileOffset += newSizeOfRawData;
                outputLen = (int)currentFileOffset;
            }

            // --- Patch Optional Header: SizeOfImage (offset 56) ---
            byte[] sizeOfImageBytes = BitConverter.GetBytes(newSizeOfImage);
            for (int k = 0; k < 4; k++)
            {
                output[optionalHeaderOffset + 56 + k] = sizeOfImageBytes[k];
            }

            // --- Patch Optional Header: SizeOfHeaders (offset 60) ---
            uint newSizeOfHeaders =
                ((uint)newPeHeadersEnd + fileAlignment - 1) / fileAlignment * fileAlignment;
            byte[] sizeOfHeadersBytes = BitConverter.GetBytes(newSizeOfHeaders);
            for (int k = 0; k < 4; k++)
            {
                output[optionalHeaderOffset + 60 + k] = sizeOfHeadersBytes[k];
            }

            // Trim to the exact byte length we produced.
            if (output.Length != outputLen)
            {
                byte[] trimmed = new byte[outputLen];
                Array.Copy(output, trimmed, outputLen);
                return trimmed;
            }

            return output;
        }

        // Translate the \SystemRoot\... or \??\... paths returned by
        // ZwQuerySystemInformation(SystemModuleInformation) into a real
        // filesystem path that can be opened from user mode.
        private static string ResolveDriverDiskPath(string fullPathName)
        {
            if (string.IsNullOrEmpty(fullPathName))
                return null;

            const string sysRootPrefix = "\\SystemRoot\\";
            const string dosPrefix = "\\??\\";

            if (fullPathName.StartsWith(sysRootPrefix, StringComparison.OrdinalIgnoreCase))
            {
                string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                return Path.Combine(winDir, fullPathName.Substring(sysRootPrefix.Length));
            }

            if (fullPathName.StartsWith(dosPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return fullPathName.Substring(dosPrefix.Length);
            }

            return fullPathName;
        }

        private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (moduleList.SelectedItems.Count > 0)
            {
                Clipboard.SetText(moduleList.SelectedItems[0].SubItems[1].Text);
            }
        }
    }
}
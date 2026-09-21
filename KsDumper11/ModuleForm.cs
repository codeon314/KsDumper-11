using KsDumper11.Driver;
using KsDumper11.PE;
using KsDumper11.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KsDumper11
{
    public partial class ModuleForm : Form
    {
        private KsDumperDriverInterface _driver;
        private ProcessDumper _dumper;
        private ProcessSummary _targetProcess;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 33554432; // WS_EX_COMPOSITED for anti-flicker
                return cp;
            }
        }

        public ModuleForm(KsDumperDriverInterface driver, ProcessDumper dumper, ProcessSummary targetProcess)
        {
            InitializeComponent();
            _driver = driver;
            _dumper = dumper;
            _targetProcess = targetProcess;
        }

        private void ModuleForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Modules for {_targetProcess.ProcessName} ({_targetProcess.ProcessId})";
            RefreshModules();

            moduleFormFix.Start();
        }

        private void moduleFormFix_Tick(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width + 3, this.Size.Height);
            this.Invalidate();
            moduleFormFix.Stop();
        }

        private void RefreshModules()
        {
            moduleList.Items.Clear();
            try
            {
                var modules = _driver.GetProcessModules(_targetProcess.ProcessId);

                foreach (var mod in modules)
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
                    item.SubItems.Add($"0x{mod.BaseAddress:X8}");
                    item.SubItems.Add($"0x{mod.SizeOfImage:X}");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error enumerating modules: " + ex.Message);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshModules();
        }

        private void dumpModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (moduleList.SelectedItems.Count == 0) return;

            var item = moduleList.SelectedItems[0];
            var modInfo = (KsDumper11.Driver.Operations.KERNEL_MODULE_INFO)item.Tag;

            string fullPath = modInfo.FullPathName;
            if (string.IsNullOrEmpty(fullPath)) fullPath = item.Text;

            ProcessSummary moduleSummary = new ProcessSummary(
                _targetProcess.ProcessId,
                modInfo.BaseAddress,
                fullPath,
                modInfo.SizeOfImage,
                0,
                _targetProcess.IsWOW64
            );

            Task.Run(() =>
            {
                try
                {
                    Logger.Log($"Dumping module {item.Text}...");
                    PEFile peFile;
                    if (_dumper.DumpProcess(moduleSummary, out peFile))
                    {
                        this.Invoke(new Action(() =>
                        {
                            using (SaveFileDialog sfd = new SaveFileDialog())
                            {
                                sfd.FileName = Path.GetFileNameWithoutExtension(item.Text) + "_dump.dll";
                                sfd.Filter = "DLL File (*.dll)|*.dll|Executable File (*.exe)|*.exe|All Files (*.*)|*.*";
                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    peFile.SaveToDisk(sfd.FileName);
                                    Logger.Log($"Module saved to {sfd.FileName}");
                                    MessageBox.Show("Module Dumped Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Failed to dump module!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }

            });
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using KsDumper11.Driver;
using KsDumper11.Utility;

namespace KsDumper11.PE
{
    public class IATReconstructor
    {
        private readonly List<Operations.KERNEL_MODULE_INFO> _modules;
        private readonly bool _is64Bit;

        public IATReconstructor(List<Operations.KERNEL_MODULE_INFO> modules, bool is64Bit)
        {
            _modules = modules;
            _is64Bit = is64Bit;
        }

        public bool FixImports(PEFile dumpedPe)
        {
            Logger.Log("Starting IAT Reconstruction...");
            var importedFunctions = ScanForImports(dumpedPe);

            if (importedFunctions.Count == 0)
            {
                Logger.Log("No imports found to reconstruct.");
                return false;
            }

            Logger.Log($"Found {importedFunctions.Sum(x => x.Value.Count)} potential imports across {importedFunctions.Count} modules.");

            // Build the new Import Directory and IAT
            return RebuildImportTable(dumpedPe, importedFunctions);
        }

        private Dictionary<string, List<ImportEntry>> ScanForImports(PEFile pe)
        {
            var results = new Dictionary<string, List<ImportEntry>>();
            int ptrSize = _is64Bit ? 8 : 4;

            // Iterate over executable and readable sections
            foreach (var section in pe.Sections)
            {
                // Skip if section is empty
                if (section.Content == null || section.Content.Length == 0) continue;

                // Scan section content for pointers
                for (int i = 0; i < section.Content.Length - ptrSize; i += 4) // Alignment usually 4 even on x64
                {
                    ulong ptrVal = 0;
                    if (_is64Bit)
                    {
                        ptrVal = BitConverter.ToUInt64(section.Content, i);
                    }
                    else
                    {
                        ptrVal = BitConverter.ToUInt32(section.Content, i);
                    }

                    // Check if this pointer looks like it points into a loaded module
                    var module = FindModuleContainingAddress(ptrVal);
                    if (module.BaseAddress != 0)
                    {
                        // It points to a module. Let's see if it points to an export.
                        uint rva = (uint)(ptrVal - module.BaseAddress);
                        string exportName = ResolveExport(module.FullPathName, rva);

                        if (!string.IsNullOrEmpty(exportName))
                        {
                            // We found a valid import!
                            if (!results.ContainsKey(module.FullPathName))
                                results[module.FullPathName] = new List<ImportEntry>();

                            // Avoid duplicates for the same location (though typically IAT is contiguous)
                            // Here we are scanning for USAGES of imports or the IAT itself.
                            // ProcessDump approach: Find the IAT by locating clusters of pointers.
                            // Simplified approach: We collect all valid pointers found. 
                            // To do a true "Thunk" rebuild, we need to place these in a new IAT.

                            // However, simply finding pointers in code (CALL [0x...]) vs pointers in .rdata (IAT) is different.
                            // The "Thunk Based" reconstruction implies we found the Original IAT or we are building a new one
                            // and patching the PE to use it.

                            // For this implementation, we will assume we are building a FRESH Import Table
                            // and we will effectively make a synthetic IAT.

                            // Optimization: Check if we already have this import to avoid bloating
                            if (!results[module.FullPathName].Any(x => x.Name == exportName))
                            {
                                results[module.FullPathName].Add(new ImportEntry
                                {
                                    Name = exportName,
                                    RvaInModule = rva
                                });
                            }
                        }
                    }
                }
            }
            return results;
        }

        private Operations.KERNEL_MODULE_INFO FindModuleContainingAddress(ulong address)
        {
            foreach (var mod in _modules)
            {
                if (address >= mod.BaseAddress && address < (mod.BaseAddress + mod.SizeOfImage))
                {
                    return mod;
                }
            }
            return new Operations.KERNEL_MODULE_INFO(); // Empty
        }

        // Caches exports to avoid re-parsing DLLs from disk repeatedly
        private static Dictionary<string, Dictionary<uint, string>> _exportCache = new Dictionary<string, Dictionary<uint, string>>();

        private string ResolveExport(string modulePath, uint rva)
        {
            string fileName = Path.GetFileName(modulePath).ToLower();

            if (!_exportCache.ContainsKey(fileName))
            {
                // Attempt to find the DLL on disk
                string localPath = Environment.SystemDirectory + "\\" + fileName;
                if (!File.Exists(localPath))
                {
                    // Try original path if it exists
                    if (File.Exists(modulePath)) localPath = modulePath;
                    else return null; // Can't find module on disk
                }

                try
                {
                    var exports = ParseExports(localPath);
                    _exportCache[fileName] = exports;
                }
                catch
                {
                    _exportCache[fileName] = new Dictionary<uint, string>(); // Cache empty failure
                }
            }

            if (_exportCache[fileName].TryGetValue(rva, out string funcName))
            {
                return funcName;
            }

            return null;
        }

        private Dictionary<uint, string> ParseExports(string filePath)
        {
            var exports = new Dictionary<uint, string>();
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Simple PE parsing logic just to get Exports
            // We can reuse the PEFile class logic but we need to handle raw file vs memory alignment.
            // PEFile expects memory aligned usually? The existing ProcessDumper reads from memory.
            // On disk files are aligned differently. We'll do a quick manual parse for disk files.

            using (var reader = new BinaryReader(new MemoryStream(fileBytes)))
            {
                reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                int peOffset = reader.ReadInt32();
                reader.BaseStream.Seek(peOffset + 0x18, SeekOrigin.Begin); // Optional Header magic
                ushort magic = reader.ReadUInt16();
                bool is64 = (magic == 0x20b);

                // Offset to Data Directories
                // PE32: 0x60 bytes from OptionalHeader start. PE32+: 0x70 bytes.
                // Optional Header starts at peOffset + 0x18.
                int dataDirOffset = peOffset + 0x18 + (is64 ? 112 : 96);

                reader.BaseStream.Seek(dataDirOffset, SeekOrigin.Begin);
                uint exportRva = reader.ReadUInt32();
                uint exportSize = reader.ReadUInt32();

                if (exportRva == 0) return exports;

                uint exportFileOffset = RvaToFileOffset(exportRva, peOffset, reader);

                if (exportFileOffset == 0) return exports;

                reader.BaseStream.Seek(exportFileOffset, SeekOrigin.Begin);
                // IMAGE_EXPORT_DIRECTORY
                reader.ReadUInt32(); // Characteristics
                reader.ReadUInt32(); // TimeDateStamp
                reader.ReadUInt16(); // MajorVersion
                reader.ReadUInt16(); // MinorVersion
                reader.ReadUInt32(); // Name
                reader.ReadUInt32(); // Base
                uint numberOfFunctions = reader.ReadUInt32();
                uint numberOfNames = reader.ReadUInt32();
                uint addressOfFunctions = reader.ReadUInt32();
                uint addressOfNames = reader.ReadUInt32();
                uint addressOfNameOrdinals = reader.ReadUInt32();

                // Map addresses
                uint namesFileOffset = RvaToFileOffset(addressOfNames, peOffset, reader);
                uint ordinalsFileOffset = RvaToFileOffset(addressOfNameOrdinals, peOffset, reader);
                uint functionsFileOffset = RvaToFileOffset(addressOfFunctions, peOffset, reader);

                for (int i = 0; i < numberOfNames; i++)
                {
                    // Read Name RVA
                    reader.BaseStream.Seek(namesFileOffset + (i * 4), SeekOrigin.Begin);
                    uint nameRva = reader.ReadUInt32();
                    uint nameFileOffset = RvaToFileOffset(nameRva, peOffset, reader);

                    // Read Ordinal
                    reader.BaseStream.Seek(ordinalsFileOffset + (i * 2), SeekOrigin.Begin);
                    ushort ordinal = reader.ReadUInt16();

                    // Read Function RVA
                    reader.BaseStream.Seek(functionsFileOffset + (ordinal * 4), SeekOrigin.Begin);
                    uint funcRva = reader.ReadUInt32();

                    // Read Name String
                    reader.BaseStream.Seek(nameFileOffset, SeekOrigin.Begin);
                    string name = ReadCString(reader);

                    // Forwarded exports are ignored for now (if funcRva is inside Export Directory)
                    if (!(funcRva >= exportRva && funcRva < exportRva + exportSize))
                    {
                        if (!exports.ContainsKey(funcRva))
                            exports.Add(funcRva, name);
                    }
                }
            }
            return exports;
        }

        private string ReadCString(BinaryReader reader)
        {
            var sb = new StringBuilder();
            char c;
            while ((c = reader.ReadChar()) != 0)
                sb.Append(c);
            return sb.ToString();
        }

        private uint RvaToFileOffset(uint rva, int peOffset, BinaryReader reader)
        {
            // Parse Section Headers
            reader.BaseStream.Seek(peOffset + 0x4 + 0x10, SeekOrigin.Begin); // File Header: SizeOfOptionalHeader
            ushort sizeOfOptHeader = reader.ReadUInt16();
            reader.BaseStream.Seek(2, SeekOrigin.Current); // Characteristics
            ushort numberOfSections = (ushort)((reader.ReadUInt16() << 8) | (reader.ReadByte())); // Backtrack? No, just re-read FileHeader properly? 

            // Simpler: Go to FileHeader struct to get section count
            reader.BaseStream.Seek(peOffset + 0x6, SeekOrigin.Begin);
            numberOfSections = reader.ReadUInt16();

            int firstSectionOffset = peOffset + 0x18 + sizeOfOptHeader;

            for (int i = 0; i < numberOfSections; i++)
            {
                int sectionOffset = firstSectionOffset + (i * 40);
                reader.BaseStream.Seek(sectionOffset + 12, SeekOrigin.Begin); // VirtualAddress
                uint vAddr = reader.ReadUInt32();
                uint vSize = reader.ReadUInt32(); // SizeOfRawData (actually at +16, vSize at +8)

                reader.BaseStream.Seek(sectionOffset + 8, SeekOrigin.Begin);
                uint virtualSize = reader.ReadUInt32();

                reader.BaseStream.Seek(sectionOffset + 20, SeekOrigin.Begin);
                uint ptrRawData = reader.ReadUInt32();

                if (rva >= vAddr && rva < vAddr + virtualSize)
                {
                    return rva - vAddr + ptrRawData;
                }
            }
            return 0;
        }

        private bool RebuildImportTable(PEFile pe, Dictionary<string, List<ImportEntry>> imports)
        {
            // We need to create a new section ".idata" to hold the IAT
            // Structure:
            // [Import Directory Table] (one entry per dll + null)
            // [Lookup Tables]
            // [Strings (DLL Names + Function Names)]
            // [Address Tables] (IAT)

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                // Calculate offsets
                // We need RVA base for the new section.
                // PEFile logic needs to align up the last section.

                uint newSectionRva = pe.GetNextSectionRva();
                uint offsetBase = 0;

                // Layout:
                // 1. Import Directory Table (IDT): (NumModules + 1) * 20 bytes
                // 2. Import Lookup Tables (ILT): (NumFuncs + NumModules) * (is64 ? 8 : 4) bytes
                // 3. Hint/Name Table + Strings: Variable
                // 4. IAT: Same size as ILT (usually)

                int idtSize = (imports.Count + 1) * 20;
                uint idtOffset = 0;
                uint currentOffset = (uint)idtSize;

                // Placeholders for writing logic
                var moduleDescriptors = new List<ImportDescriptor>();

                // Data buffers
                var iltBuffer = new List<byte>();
                var stringBuffer = new List<byte>();

                // We map strings to relative offsets within stringBuffer
                var stringOffsets = new Dictionary<string, uint>();

                // Helper to add string
                Func<string, uint> addString = (s) => {
                    if (stringOffsets.ContainsKey(s)) return stringOffsets[s];
                    uint off = (uint)stringBuffer.Count;
                    stringBuffer.AddRange(Encoding.ASCII.GetBytes(s));
                    stringBuffer.Add(0);
                    // align to 2 bytes? Hint/Name needs alignment
                    if (stringBuffer.Count % 2 != 0) stringBuffer.Add(0);
                    stringOffsets[s] = off;
                    return off;
                };

                foreach (var mod in imports)
                {
                    var descriptor = new ImportDescriptor();
                    descriptor.NameOffsetInStrings = addString(Path.GetFileName(mod.Key));

                    // Build ILT for this module
                    descriptor.IltOffset = (uint)iltBuffer.Count;

                    foreach (var func in mod.Value)
                    {
                        // Hint/Name Table Entry
                        uint nameOff = addString(func.Name);

                        // Create Thunk Data
                        // If x64: 64-bit value. If by name, high bit 0.
                        // We construct RVA to the Hint/Name entry. 
                        // The Hint/Name entry is actually 2 bytes Hint + String.
                        // Our addString adds just string. We need to prepending Hint.
                        // Correction: The RVA points to IMAGE_IMPORT_BY_NAME which is { SHORT Hint; CHAR Name[1]; }
                        // So we need to adjust addString logic for functions or construct the struct manually.

                        // Let's fix string buffer logic for functions
                        uint hintNameRva = (uint)stringBuffer.Count;
                        stringBuffer.Add(0); stringBuffer.Add(0); // Hint = 0
                        stringBuffer.AddRange(Encoding.ASCII.GetBytes(func.Name));
                        stringBuffer.Add(0);
                        if (stringBuffer.Count % 2 != 0) stringBuffer.Add(0);

                        // Now write the thunk
                        if (_is64Bit)
                        {
                            ulong thunk = hintNameRva; // Relative to string buffer start for now
                            // We will adjust base later
                            iltBuffer.AddRange(BitConverter.GetBytes(thunk));
                        }
                        else
                        {
                            uint thunk = hintNameRva;
                            iltBuffer.AddRange(BitConverter.GetBytes(thunk));
                        }
                    }

                    // Null terminator for ILT
                    if (_is64Bit) iltBuffer.AddRange(new byte[8]);
                    else iltBuffer.AddRange(new byte[4]);

                    moduleDescriptors.Add(descriptor);
                }

                // Now we have sizes. 
                // IDT is at offset 0
                // ILT is at offset idtSize
                uint iltBase = (uint)idtSize;
                // Strings is at iltBase + iltBuffer.Count
                uint stringsBase = iltBase + (uint)iltBuffer.Count;

                // Wait, where is IAT? 
                // IAT is usually separate, pointed to by FirstThunk. 
                // ILT is OriginalFirstThunk.
                // They are identical on disk. So we can just copy ILT to IAT area?
                // Or make them point to same data? No, loader overwrites IAT. ILT must stay.
                // So we create IAT area after Strings.
                uint iatBase = stringsBase + (uint)stringBuffer.Count;

                // Now we construct the full binary blob

                // 1. Write IDT
                foreach (var desc in moduleDescriptors)
                {
                    uint originalFirstThunkRva = newSectionRva + iltBase + desc.IltOffset;
                    uint nameRva = newSectionRva + stringsBase + desc.NameOffsetInStrings;
                    uint firstThunkRva = newSectionRva + iatBase + desc.IltOffset;

                    writer.Write(originalFirstThunkRva); // OriginalFirstThunk
                    writer.Write((uint)0); // TimeDateStamp
                    writer.Write((uint)0); // ForwarderChain
                    writer.Write(nameRva); // Name
                    writer.Write(firstThunkRva); // FirstThunk
                }
                writer.Write(new byte[20]); // Null Descriptor

                // 2. Write ILT
                // We need to patch values in ILT buffer to be RVAs
                // The values currently are offsets into stringBuffer.
                // We need to add (newSectionRva + stringsBase) to them.
                var iltBytes = iltBuffer.ToArray();
                int thunkSize = _is64Bit ? 8 : 4;
                for (int i = 0; i < iltBytes.Length; i += thunkSize)
                {
                    if (_is64Bit)
                    {
                        ulong val = BitConverter.ToUInt64(iltBytes, i);
                        if (val != 0) // Skip null terminators
                        {
                            val += (newSectionRva + stringsBase);
                            Array.Copy(BitConverter.GetBytes(val), 0, iltBytes, i, 8);
                        }
                    }
                    else
                    {
                        uint val = BitConverter.ToUInt32(iltBytes, i);
                        if (val != 0)
                        {
                            val += (newSectionRva + stringsBase);
                            Array.Copy(BitConverter.GetBytes(val), 0, iltBytes, i, 4);
                        }
                    }
                }
                writer.Write(iltBytes);

                // 3. Write Strings
                writer.Write(stringBuffer.ToArray());

                // 4. Write IAT (Copy of ILT)
                writer.Write(iltBytes);

                // Finalize
                writer.Flush();
                byte[] newSectionData = ms.ToArray();

                // Add section to PE
                pe.AddSection(".idata", newSectionData, (uint)(NativePEStructs.DataSectionFlags.MemoryRead | NativePEStructs.DataSectionFlags.MemoryWrite | NativePEStructs.DataSectionFlags.ContentInitializedData));

                // Fix Data Directory
                // IAT Entry (Index 12) points to IAT (FirstThunk) ? No, it points to IAT table.
                // Import Entry (Index 1) points to IDT.

                pe.SetDataDirectory(NativePEStructs.IMAGE_DIRECTORY_ENTRY_IMPORT, newSectionRva, (uint)idtSize);

                // IAT Directory (12) usually points to the IAT array range.
                pe.SetDataDirectory(NativePEStructs.IMAGE_DIRECTORY_ENTRY_IAT, newSectionRva + iatBase, (uint)iltBytes.Length);

                return true;
            }
        }

        private struct ImportEntry
        {
            public string Name;
            public uint RvaInModule;
        }

        private struct ImportDescriptor
        {
            public uint NameOffsetInStrings;
            public uint IltOffset;
        }
    }
}
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KsDumper11
{
    public class KduSelfExtract
    {
        public static void DisableDriverBlockList()
        {
            RegistryKey configKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CI\Config", true);

            if (configKey == null)
            {
                configKey = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\CI\Config");
            }

            if (configKey != null)
            {
                if (configKey.GetValue("VulnerableDriverBlocklistEnable") == null)
                {
                    configKey.SetValue("VulnerableDriverBlocklistEnable", 0);
                }
            }
        }

        static string asmDir = "";
        static string driverDir = "";
        static KduSelfExtract()
        {
            DisableDriverBlockList();

            asmDir = AssemblyDirectory;
            driverDir = asmDir + @"\Driver";
        }
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string KduPath
        {
            get
            {
                return driverDir + @"\kdu.exe";
            }
        }

        private static bool Extracted()
        {
            string driverPath = driverDir + @"\KsDumperDriver.sys";
            string kduPath = driverDir + @"\kdu.exe";
            string drv64Path = driverDir + @"\drv64.dll";
            string taigei64Path = driverDir + @"\Taigei64.dll";

            if (!Directory.Exists(driverDir))
            {
                return false;
            }

            // Compare on-disk file length against the embedded resource length so that
            // updating the embedded binaries (e.g. KDU 1.4.4 -> 1.5.0) forces a re-extract
            // instead of silently reusing the stale copies already on disk.
            if (!FileMatchesResource(driverPath, DriverInterface_NET10.Properties.Resources.KsDumperDriver)) return false;
            if (!FileMatchesResource(kduPath, DriverInterface_NET10.Properties.Resources.kdu)) return false;
            if (!FileMatchesResource(drv64Path, DriverInterface_NET10.Properties.Resources.drv64)) return false;
            if (!FileMatchesResource(taigei64Path, DriverInterface_NET10.Properties.Resources.Taigei64)) return false;

            return true;
        }

        private static bool FileMatchesResource(string path, byte[] resource)
        {
            // If we have no embedded resource to compare against, fall back to
            // "does the file exist" semantics so we don't wipe a valid install.
            if (resource == null)
            {
                return File.Exists(path);
            }

            if (!File.Exists(path))
            {
                return false;
            }

            try
            {
                return new FileInfo(path).Length == resource.Length;
            }
            catch
            {
                return false;
            }
        }

        private static void WriteResourceIfNotNull(string path, byte[] resource)
        {
            if (resource != null)
            {
                File.WriteAllBytes(path, resource);
            }
        }

        public static void Extract()
        {
            if (!Extracted())
            {
                string asmDir = AssemblyDirectory;
                string driverDir = asmDir + @"\Driver";
                if (!Directory.Exists(driverDir))
                {
                    Directory.CreateDirectory(driverDir);
                }

                string driverPath = driverDir + @"\KsDumperDriver.sys";
                string kduPath = driverDir + @"\kdu.exe";
                string drv64Path = driverDir + @"\drv64.dll";
                string taigei64Path = driverDir + @"\Taigei64.dll";

                // Overwrite unconditionally now. Extracted() already gated this
                // branch on the on-disk size differing from the embedded resource,
                // so reaching here means we genuinely need to refresh the files.
                WriteResourceIfNotNull(driverPath, DriverInterface_NET10.Properties.Resources.KsDumperDriver);
                WriteResourceIfNotNull(kduPath, DriverInterface_NET10.Properties.Resources.kdu);
                WriteResourceIfNotNull(drv64Path, DriverInterface_NET10.Properties.Resources.drv64);
                WriteResourceIfNotNull(taigei64Path, DriverInterface_NET10.Properties.Resources.Taigei64);
            }
        }
    }
}

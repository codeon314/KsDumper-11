using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using KsDumper11.Driver;

namespace KsDumper11
{
    public class Program
    {
        static string exeName = "KsDumper11.exe";

        public static bool ProviderIsClosing = false;

        static JsonSettingsManager settingsManager;

        static void runSnifferBypass()
        {
            string asmPath = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(asmPath);

            string fileName = Path.GetFileName(asmPath);
            string newFile = SnifferBypass.GenerateRandomString(12) + ".exe";
            string newFileName = Path.Combine(directory, newFile);

            if (fileName == exeName)
            {
                ProcessStartInfo renameAndExecuteProcessInfo = new ProcessStartInfo();
                renameAndExecuteProcessInfo.FileName = "cmd.exe";
                renameAndExecuteProcessInfo.Arguments = $"/c timeout 3 > NUL && ren \"{asmPath}\" \"{newFile}\" && \"{newFileName}\"";
                renameAndExecuteProcessInfo.UseShellExecute = true;
                renameAndExecuteProcessInfo.CreateNoWindow = true;
                renameAndExecuteProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process renameAndExecuteProcess = new Process();
                renameAndExecuteProcess.StartInfo = renameAndExecuteProcessInfo;
                renameAndExecuteProcess.Start();

                Environment.Exit(0);
            }
            else
            {
                Application.ApplicationExit += Application_ApplicationExit;
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (!ProviderIsClosing)
            {
                string asmPath = Assembly.GetExecutingAssembly().Location;
                string directory = Path.GetDirectoryName(asmPath);

                string fileName = Path.GetFileName(asmPath);
                string newFile = SnifferBypass.GenerateRandomString(12) + ".exe";
                string newFileName = Path.Combine(directory, exeName);

                if (fileName != exeName)
                {
                    ProcessStartInfo renameAndExecuteProcessInfo = new ProcessStartInfo();
                    renameAndExecuteProcessInfo.FileName = "cmd.exe";
                    renameAndExecuteProcessInfo.Arguments = $"/c timeout 2 > NUL && ren \"{asmPath}\" \"{exeName}\"";
                    renameAndExecuteProcessInfo.UseShellExecute = false;
                    renameAndExecuteProcessInfo.CreateNoWindow = true;
                    renameAndExecuteProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    Process renameAndExecuteProcess = new Process();
                    renameAndExecuteProcess.StartInfo = renameAndExecuteProcessInfo;
                    renameAndExecuteProcess.Start();
                }
            }
            else
            {
                ProviderIsClosing = false;
            }
        }

        [STAThread]
        private static void Main()
        {
            settingsManager = new JsonSettingsManager();

            if (settingsManager.JsonSettings.enableAntiAntiDebuggerTools)
            {
                runSnifferBypass();
            }

            KduSelfExtract.DisableDriverBlockList();

            KduSelfExtract.Extract();

            ApplicationConfiguration.Initialize();

            Application.SetColorMode(SystemColorMode.Dark);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            bool driverOpen = KsDumperDriverInterface.IsDriverOpen("\\\\.\\KsDumper");

            if (!driverOpen)
            {
                if (!File.Exists(KduSelfExtract.AssemblyDirectory + @"\\Providers.json"))
                {
                    Application.Run(new ProviderSelector());
                    Application.Run(new DumperForm());
                }
                else
                {
                    KduWrapper wr = new KduWrapper(KduSelfExtract.AssemblyDirectory + @"\Driver\kdu.exe");
                    wr.LoadProviders();

                    if (wr.DefaultProvider != -1)
                    {
                        wr.Start();

                        if (KsDumperDriverInterface.IsDriverOpen("\\\\.\\KsDumper"))
                        {
                            Application.Run(new DumperForm());
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Application.Run(new ProviderSelector());

                        if (KsDumperDriverInterface.IsDriverOpen("\\\\.\\KsDumper"))
                        {
                            Application.Run(new DumperForm());
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            else
            {
                Application.Run(new DumperForm());
                Environment.Exit(0);
            }
        }
    }
}
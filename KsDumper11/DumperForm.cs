using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KsDumper11.Driver;
using KsDumper11.PE;
using KsDumper11.Utility;
using System.Collections.Generic;

namespace KsDumper11
{
    public partial class DumperForm : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 33554432; // WS_EX_COMPOSITED
                return cp;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetSecurityInfo(int HANDLE, int SE_OBJECT_TYPE, int SECURITY_INFORMATION, int psidOwner, int psidGroup, out IntPtr pDACL, IntPtr pSACL, out IntPtr pSecurityDescriptor);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetSecurityInfo(int HANDLE, int SE_OBJECT_TYPE, int SECURITY_INFORMATION, int psidOwner, int psidGroup, IntPtr pDACL, IntPtr pSACL);

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ZwSuspendProcess(IntPtr hProcess);

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ZwResumeProcess(IntPtr hProcess);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private readonly KsDumperDriverInterface driver;
        private readonly ProcessDumper dumper;
        private System.Windows.Forms.Timer t;

        bool skip_closeDriverOnExitBox_CheckedChanged_Event = false;
        bool skip_antiantiDebuggerToolsBox_CheckedChanged_Event = false;

        JsonSettingsManager settingsManager;

        public DumperForm()
        {
            this.InitializeComponent();

            settingsManager = new JsonSettingsManager();

            skip_closeDriverOnExitBox_CheckedChanged_Event = true;
            closeDriverOnExitBox.Checked = settingsManager.JsonSettings.closeDriverOnExit;

            skip_antiantiDebuggerToolsBox_CheckedChanged_Event = true;
            antiantiDebuggerToolsBox.Checked = settingsManager.JsonSettings.enableAntiAntiDebuggerTools;

            this.FormClosing += Dumper_FormClosing;
            this.Disposed += Dumper_Disposed;

            this.processList.HeaderStyle = ColumnHeaderStyle.Clickable;
            this.processList.ColumnWidthChanging += this.processList_ColumnWidthChanging;
            this.driver = new KsDumperDriverInterface("\\\\.\\KsDumper");
            this.dumper = new ProcessDumper(this.driver);

            // Show the provider that KDU is using to map KsDumperDriver.sys.
            // Read it straight from Providers.json so this status line always
            // matches what KduWrapper actually passes to `kdu.exe -prv`.
            UpdateProviderInfo();

            this.LoadProcessList();
        }

        private void Dumper_Load(object sender, EventArgs e)
        {
            if (antiantiDebuggerToolsBox.Checked)
            {
                SnifferBypass.SelfTitle(this.Handle);
                this.Text = SnifferBypass.GenerateRandomString(this.Text.Length);
            }

            Logger.OnLog += this.Logger_OnLog;
            Logger.Log("KsDumper 11 - [By EquiFox] Given Newlife", Array.Empty<object>());

            FormFixTimer.Start();
        }

        private void FormFixTimer_Tick(object sender, EventArgs e)
        {
            this.Size = new Size(this.Size.Width + 3, this.Size.Height);

            this.Invalidate();

            FormFixTimer.Stop();
        }

        private void Dumper_Disposed(object sender, EventArgs e)
        {
            if (settingsManager.JsonSettings.closeDriverOnExit)
            {
                driver.UnloadDriver();
            }
        }

        private void closeDriverOnExitBox_CheckedChanged(object sender, EventArgs e)
        {
            if (skip_closeDriverOnExitBox_CheckedChanged_Event)
            {
                skip_closeDriverOnExitBox_CheckedChanged_Event = false;
                return;
            }

            settingsManager.JsonSettings.closeDriverOnExit = closeDriverOnExitBox.Checked;
            settingsManager.Save();
        }

        private void antiantiDebuggerToolsBox_CheckedChanged(object sender, EventArgs e)
        {
            if (skip_antiantiDebuggerToolsBox_CheckedChanged_Event)
            {
                skip_antiantiDebuggerToolsBox_CheckedChanged_Event = false;
                return;
            }

            settingsManager.JsonSettings.enableAntiAntiDebuggerTools = antiantiDebuggerToolsBox.Checked;
            settingsManager.Save();
        }

        private void Dumper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeDriverOnExitBox.Checked)
            {
                driver.UnloadDriver();
            }
        }

        private void processList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = this.processList.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void LoadProcessList()
        {
            bool flag = this.driver.HasValidHandle();
            if (flag)
            {
                ProcessSummary[] result;
                bool processSummaryList = this.driver.GetProcessSummaryList(out result);
                if (processSummaryList)
                {
                    this.processList.LoadProcesses(result);
                }
                else
                {
                    MessageBox.Show("Unable to retrieve process list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        // -----------------------------------------------------------------
        // UpdateProviderInfo
        //
        // Reads Providers.json (the same file KduWrapper writes and reads)
        // and updates providerInfoLbl to show the currently selected default
        // provider. The "-prv" argument that KduWrapper passes to kdu.exe is
        // the DefaultProvider index into the Providers list, so we display
        // both the human-readable provider name and that same numeric ID.
        // -----------------------------------------------------------------
        private void UpdateProviderInfo()
        {
            try
            {
                string providersPath = Path.Combine(KduSelfExtract.AssemblyDirectory, "Providers.json");

                if (!File.Exists(providersPath))
                {
                    providerInfoLbl.Text = "Current Provider: Not configured";
                    return;
                }

                KduProviderSettings settings =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<KduProviderSettings>(
                        File.ReadAllText(providersPath));

                if (settings == null
                    || settings.Providers == null
                    || settings.DefaultProvider < 0
                    || settings.DefaultProvider >= settings.Providers.Count)
                {
                    providerInfoLbl.Text = "Current Provider: None selected";
                    return;
                }

                KduProvider provider = settings.Providers[settings.DefaultProvider];

                string name = provider.ProviderName ?? "(unknown)";
                name = name.Replace("[WORKING] ", string.Empty)
                           .Replace("[NOT WORKING] ", string.Empty);

                providerInfoLbl.Text = string.Format(
                    "Current Provider: {0} (ID #{1})",
                    name,
                    provider.ProviderIndex);
            }
            catch
            {
                providerInfoLbl.Text = "Current Provider: (unknown)";
            }
        }

        private bool DumpProcess(ProcessSummary process)
        {
            bool flag = this.driver.HasValidHandle();
            bool flag2;
            if (flag)
            {
                Logger.Log("Valid driver handle open", Array.Empty<object>());
                bool sucess = false;
                Task.Run(delegate ()
                {
                    Logger.Log("Dumping process...", Array.Empty<object>());
                    PEFile peFile;
                    sucess = this.dumper.DumpProcess(process, out peFile);
                    if (sucess)
                    {
                        Logger.Log("Sucess!", Array.Empty<object>());
                        this.Invoke(new Action(delegate ()
                        {
                            using (SaveFileDialog sfd = new SaveFileDialog())
                            {
                                sfd.FileName = process.ProcessName.Replace(".exe", "_dump.exe");
                                sfd.Filter = "Executable File (.exe)|*.exe";
                                bool flag3 = sfd.ShowDialog() == DialogResult.OK;
                                if (flag3)
                                {
                                    peFile.SaveToDisk(sfd.FileName);
                                    Logger.Log("Saved at '{0}' !", new object[] { sfd.FileName });
                                }
                            }
                        }));
                        Logger.Log(process.ProcessName + "  Killed", Array.Empty<object>());
                        this.KillProcess(process.ProcessId);
                    }
                    else
                    {
                        Logger.Log("Failure", Array.Empty<object>());
                        this.Invoke(new Action(delegate ()
                        {
                            MessageBox.Show("Unable to dump target process !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }));
                    }
                });
                flag2 = sucess;
            }
            else
            {
                MessageBox.Show("Unable to communicate with driver ! Make sure it is loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                flag2 = false;
            }
            return flag2;
        }

        private bool DumpProcess(Process process)
        {
            bool flag = this.driver.HasValidHandle();
            bool flag3;
            if (flag)
            {
                Logger.Log("Valid driver handle open", Array.Empty<object>());
                Logger.Log("Dumping process...", Array.Empty<object>());
                PEFile peFile;
                bool sucess = this.dumper.DumpProcess(process, out peFile);
                bool flag2 = sucess;
                if (flag2)
                {
                    Logger.Log("Sucess!", Array.Empty<object>());
                    base.Invoke(new Action(delegate ()
                    {
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.FileName = process.ProcessName + "_dump.exe";
                            sfd.Filter = "Executable File (.exe)|*.exe";
                            bool flag4 = sfd.ShowDialog() == DialogResult.OK;
                            if (flag4)
                            {
                                peFile.SaveToDisk(sfd.FileName);
                                Logger.Log("Saved at '{0}' !", new object[] { sfd.FileName });
                            }
                        }
                    }));
                    Logger.Log(process.ProcessName + "  Killed", Array.Empty<object>());
                    this.KillProcess(process.Id);
                }
                else
                {
                    Logger.Log("Failure", Array.Empty<object>());
                    Logger.Log(process.ProcessName + "  Killed", Array.Empty<object>());
                    this.KillProcess(process.Id);
                    base.Invoke(new Action(delegate ()
                    {
                        MessageBox.Show("Unable to dump target process !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }));
                }
                flag3 = sucess;
            }
            else
            {
                MessageBox.Show("Unable to communicate with driver ! Make sure it is loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Logger.Log(process.ProcessName + "  Killed", Array.Empty<object>());
                this.KillProcess(process.Id);
                flag3 = false;
            }
            return flag3;
        }

        private void dumpMainModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            this.DumpProcess(targetProcess);
        }

        private void viewModulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            var modForm = new ModuleForm(this.driver, this.dumper, targetProcess);
            modForm.ShowDialog();
        }

        private void Logger_OnLog(string message)
        {
            this.logsTextBox.Invoke(new Action(delegate ()
            {
                this.logsTextBox.AppendText(message);
                this.logsTextBox.Update();
            }));
        }

        private void refreshMenuBtn_Click(object sender, EventArgs e)
        {
            this.LoadProcessList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = this.processList.SelectedItems.Count == 0;
        }

        private void logsTextBox_TextChanged(object sender, EventArgs e)
        {
            this.logsTextBox.SelectionStart = this.logsTextBox.Text.Length;
            this.logsTextBox.ScrollToCaret();
        }

        private void openInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            Process.Start("explorer.exe", Path.GetDirectoryName(targetProcess.MainModuleFileName));
        }

        private void suspendProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            this.SuspendProcess(targetProcess.ProcessId);
        }

        private void KillProcess(int processId)
        {
            IntPtr hProcess = DumperForm.OpenProcess(1081U, 0, (uint)processId);
            bool flag = hProcess == IntPtr.Zero;
            if (flag)
            {
                IntPtr pDACL;
                IntPtr pSecDesc;
                DumperForm.GetSecurityInfo((int)Process.GetCurrentProcess().Handle, 6, 4, 0, 0, out pDACL, IntPtr.Zero, out pSecDesc);
                hProcess = DumperForm.OpenProcess(262144U, 0, (uint)processId);
                DumperForm.SetSecurityInfo((int)hProcess, 6, 536870916, 0, 0, pDACL, IntPtr.Zero);
                DumperForm.CloseHandle(hProcess);
                hProcess = DumperForm.OpenProcess(1080U, 0, (uint)processId);
            }
            try
            {
                DumperForm.TerminateProcess(hProcess, 0U);
            }
            catch
            {
            }
            DumperForm.CloseHandle(hProcess);
        }

        private void SuspendProcess(int processId)
        {
            IntPtr hProcess = DumperForm.OpenProcess(2048U, 0, (uint)processId);
            bool flag = hProcess == IntPtr.Zero;
            if (flag)
            {
                IntPtr pDACL;
                IntPtr pSecDesc;
                DumperForm.GetSecurityInfo((int)Process.GetCurrentProcess().Handle, 6, 4, 0, 0, out pDACL, IntPtr.Zero, out pSecDesc);
                hProcess = DumperForm.OpenProcess(262144U, 0, (uint)processId);
                DumperForm.SetSecurityInfo((int)hProcess, 6, 536870916, 0, 0, pDACL, IntPtr.Zero);
                DumperForm.CloseHandle(hProcess);
                hProcess = DumperForm.OpenProcess(1080U, 0, (uint)processId);
            }
            try
            {
                DumperForm.ZwSuspendProcess(hProcess);
            }
            catch
            {
            }
            DumperForm.CloseHandle(hProcess);
        }

        private void ResumeProcess(int processId)
        {
            IntPtr hProcess = DumperForm.OpenProcess(2048U, 0, (uint)processId);
            bool flag = hProcess == IntPtr.Zero;
            if (flag)
            {
                IntPtr pDACL;
                IntPtr pSecDesc;
                DumperForm.GetSecurityInfo((int)Process.GetCurrentProcess().Handle, 6, 4, 0, 0, out pDACL, IntPtr.Zero, out pSecDesc);
                hProcess = DumperForm.OpenProcess(262144U, 0, (uint)processId);
                DumperForm.SetSecurityInfo((int)hProcess, 6, 536870916, 0, 0, pDACL, IntPtr.Zero);
                DumperForm.CloseHandle(hProcess);
                hProcess = DumperForm.OpenProcess(1080U, 0, (uint)processId);
            }
            try
            {
                DumperForm.ZwResumeProcess(hProcess);
            }
            catch
            {
            }
            DumperForm.CloseHandle(hProcess);
        }

        private void resumeProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            this.ResumeProcess(targetProcess.ProcessId);
        }

        private void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessSummary targetProcess = this.processList.SelectedItems[0].Tag as ProcessSummary;
            this.KillProcess(targetProcess.ProcessId);
        }

        private void T_Tick(object sender, EventArgs e)
        {
            this.LoadProcessList();
        }

        private void ClearLog()
        {
            this.logsTextBox.Clear();
        }

        private void StartAndDumpFile(string dumpFile)
        {
            Logger.Log(Path.GetFileName(dumpFile) + "  Started", Array.Empty<object>());
            Process process = Process.Start(dumpFile);
            Thread.Sleep(4);
            this.SuspendProcess(process.Id);
            Logger.Log("Suspending process...", Array.Empty<object>());
            bool flag = this.DumpProcess(process);
            if (flag)
            {
                Logger.Log(Path.GetFileName(dumpFile) + "  Dumped", Array.Empty<object>());
            }
            else
            {
                Logger.Log("process dump failed", Array.Empty<object>());
            }
        }

        private void fileDumpBtn_Click(object sender, EventArgs e)
        {
            this.ClearLog();
            Logger.Log("KsDumper v1.1 - By EquiFox", Array.Empty<object>());
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable File (.exe)|*.exe";
            openFileDialog.Title = "File to dump";
            openFileDialog.RestoreDirectory = true;
            bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                string dumpFile = openFileDialog.FileName;
                this.StartAndDumpFile(dumpFile);
            }
        }

        private void hideSystemProcessBtn_Click(object sender, EventArgs e)
        {
            bool flag = !this.processList.SystemProcessesHidden;
            if (flag)
            {
                this.processList.HideSystemProcesses();
                this.hideSystemProcessBtn.Text = "Show System Processes";
            }
            else
            {
                this.processList.ShowSystemProcesses();
                this.hideSystemProcessBtn.Text = "Hide System Processes";
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            this.LoadProcessList();
        }

        private void autoRefreshCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool @checked = this.autoRefreshCheckBox.Checked;
            if (@checked)
            {
                bool flag = this.t == null;
                if (flag)
                {
                    this.t = new System.Windows.Forms.Timer();
                    this.t.Tick += this.T_Tick;
                    this.t.Interval = 100;
                    this.t.Start();
                }
                else
                {
                    this.t.Interval = 100;
                    this.t.Start();
                }
            }
            else
            {
                this.t.Stop();
            }
        }

        private void providerBtn_Click(object sender, EventArgs e)
        {
            KsDumperDriverInterface drv = KsDumperDriverInterface.OpenKsDumperDriver();

            drv.UnloadDriver();
            drv.Dispose();

            ProviderSelector prov = new ProviderSelector();

            prov.ShowDialog();

            StartDriver.Start();

            // Provider selection may have changed while the dialog was open,
            // so refresh the status line to reflect the new default provider.
            UpdateProviderInfo();
        }

        private void kernelModulesBtn_Click(object sender, EventArgs e)
        {
            if (!this.driver.HasValidHandle())
            {
                MessageBox.Show("Unable to communicate with driver ! Make sure it is loaded.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            KernelModulesForm kmf = new KernelModulesForm();
            kmf.ShowDialog();
        }
    }
}
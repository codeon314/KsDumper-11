namespace KsDumper11
{
    public partial class DumperForm : global::System.Windows.Forms.Form
    {
        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {

            components = new System.ComponentModel.Container();

            groupBox1 = new System.Windows.Forms.GroupBox();

            logsTextBox = new System.Windows.Forms.RichTextBox();

            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);

            dumpMainModuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            viewModulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();

            openInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            suspendProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            resumeProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            killProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            fileDumpBtn = new System.Windows.Forms.Button();

            refreshBtn = new System.Windows.Forms.Button();

            autoRefreshCheckBox = new System.Windows.Forms.CheckBox();

            hideSystemProcessBtn = new System.Windows.Forms.Button();

            closeDriverOnExitBox = new System.Windows.Forms.CheckBox();

            processList = new KsDumper11.Utility.ProcessListView();

            PIDHeader = new System.Windows.Forms.ColumnHeader();

            NameHeader = new System.Windows.Forms.ColumnHeader();

            PathHeader = new System.Windows.Forms.ColumnHeader();

            BaseAddressHeader = new System.Windows.Forms.ColumnHeader();

            EntryPointHeader = new System.Windows.Forms.ColumnHeader();

            ImageSizeHeader = new System.Windows.Forms.ColumnHeader();

            ImageTypeHeader = new System.Windows.Forms.ColumnHeader();

            providerBtn = new System.Windows.Forms.Button();

            kernelModulesBtn = new System.Windows.Forms.Button();

            antiantiDebuggerToolsBox = new System.Windows.Forms.CheckBox();

            FormFixTimer = new System.Windows.Forms.Timer(components);

            providerInfoLbl = new System.Windows.Forms.Label();

            groupBox1.SuspendLayout();

            contextMenuStrip1.SuspendLayout();

            SuspendLayout();

            // 

            // groupBox1

            // 

            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            groupBox1.Controls.Add(logsTextBox);

            groupBox1.Location = new System.Drawing.Point(20, 858);

            groupBox1.Margin = new System.Windows.Forms.Padding(5);

            groupBox1.Name = "groupBox1";

            groupBox1.Padding = new System.Windows.Forms.Padding(5);

            groupBox1.Size = new System.Drawing.Size(2235, 354);

            groupBox1.TabIndex = 5;

            groupBox1.TabStop = false;

            groupBox1.Text = "Logs";

            // 

            // logsTextBox

            // 

            logsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            logsTextBox.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);

            logsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;

            logsTextBox.ForeColor = System.Drawing.Color.Silver;

            logsTextBox.Location = new System.Drawing.Point(17, 34);

            logsTextBox.Margin = new System.Windows.Forms.Padding(5);

            logsTextBox.Name = "logsTextBox";

            logsTextBox.ReadOnly = true;

            logsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            logsTextBox.Size = new System.Drawing.Size(2202, 308);

            logsTextBox.TabIndex = 0;

            logsTextBox.Text = "";

            logsTextBox.TextChanged += logsTextBox_TextChanged;

            // 

            // contextMenuStrip1

            // 

            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);

            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { dumpMainModuleToolStripMenuItem, viewModulesToolStripMenuItem, toolStripSeparator1, openInExplorerToolStripMenuItem, suspendProcessToolStripMenuItem, resumeProcessToolStripMenuItem, killProcessToolStripMenuItem });

            contextMenuStrip1.Name = "contextMenuStrip1";

            contextMenuStrip1.Size = new System.Drawing.Size(220, 202);

            contextMenuStrip1.Opening += contextMenuStrip1_Opening;

            // 

            // dumpMainModuleToolStripMenuItem

            // 

            dumpMainModuleToolStripMenuItem.Name = "dumpMainModuleToolStripMenuItem";

            dumpMainModuleToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            dumpMainModuleToolStripMenuItem.Text = "Dump Process";

            dumpMainModuleToolStripMenuItem.Click += dumpMainModuleToolStripMenuItem_Click;

            // 

            // viewModulesToolStripMenuItem

            // 

            viewModulesToolStripMenuItem.Name = "viewModulesToolStripMenuItem";

            viewModulesToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            viewModulesToolStripMenuItem.Text = "View Modules";

            viewModulesToolStripMenuItem.Click += viewModulesToolStripMenuItem_Click;

            // 

            // toolStripSeparator1

            // 

            toolStripSeparator1.Name = "toolStripSeparator1";

            toolStripSeparator1.Size = new System.Drawing.Size(216, 6);

            // 

            // openInExplorerToolStripMenuItem

            // 

            openInExplorerToolStripMenuItem.Name = "openInExplorerToolStripMenuItem";

            openInExplorerToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            openInExplorerToolStripMenuItem.Text = "Open In Explorer";

            openInExplorerToolStripMenuItem.Click += openInExplorerToolStripMenuItem_Click;

            // 

            // suspendProcessToolStripMenuItem

            // 

            suspendProcessToolStripMenuItem.Name = "suspendProcessToolStripMenuItem";

            suspendProcessToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            suspendProcessToolStripMenuItem.Text = "Suspend process";

            suspendProcessToolStripMenuItem.Click += suspendProcessToolStripMenuItem_Click;

            // 

            // resumeProcessToolStripMenuItem

            // 

            resumeProcessToolStripMenuItem.Name = "resumeProcessToolStripMenuItem";

            resumeProcessToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            resumeProcessToolStripMenuItem.Text = "Resume process";

            resumeProcessToolStripMenuItem.Click += resumeProcessToolStripMenuItem_Click;

            // 

            // killProcessToolStripMenuItem

            // 

            killProcessToolStripMenuItem.Name = "killProcessToolStripMenuItem";

            killProcessToolStripMenuItem.Size = new System.Drawing.Size(219, 32);

            killProcessToolStripMenuItem.Text = "Kill process";

            killProcessToolStripMenuItem.Click += killProcessToolStripMenuItem_Click;

            // 

            // fileDumpBtn

            // 

            fileDumpBtn.Location = new System.Drawing.Point(163, 8);

            fileDumpBtn.Margin = new System.Windows.Forms.Padding(5);

            fileDumpBtn.Name = "fileDumpBtn";

            fileDumpBtn.Size = new System.Drawing.Size(150, 38);

            fileDumpBtn.TabIndex = 1;

            fileDumpBtn.Text = "Dump File";

            fileDumpBtn.UseVisualStyleBackColor = true;

            fileDumpBtn.Click += fileDumpBtn_Click;

            // 

            // refreshBtn

            // 

            refreshBtn.Location = new System.Drawing.Point(20, 8);

            refreshBtn.Margin = new System.Windows.Forms.Padding(5);

            refreshBtn.Name = "refreshBtn";

            refreshBtn.Size = new System.Drawing.Size(133, 38);

            refreshBtn.TabIndex = 10;

            refreshBtn.Text = "Refresh";

            refreshBtn.UseVisualStyleBackColor = true;

            refreshBtn.Click += refreshBtn_Click;

            // 

            // autoRefreshCheckBox

            // 

            autoRefreshCheckBox.AutoSize = true;

            autoRefreshCheckBox.Location = new System.Drawing.Point(1212, 16);

            autoRefreshCheckBox.Margin = new System.Windows.Forms.Padding(5);

            autoRefreshCheckBox.Name = "autoRefreshCheckBox";

            autoRefreshCheckBox.Size = new System.Drawing.Size(133, 24);

            autoRefreshCheckBox.TabIndex = 11;

            autoRefreshCheckBox.Text = "Auto Refresh";

            autoRefreshCheckBox.UseVisualStyleBackColor = true;

            autoRefreshCheckBox.CheckedChanged += autoRefreshCheckBox_CheckedChanged;

            // 

            // hideSystemProcessBtn

            // 

            hideSystemProcessBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            hideSystemProcessBtn.Location = new System.Drawing.Point(1759, 8);

            hideSystemProcessBtn.Margin = new System.Windows.Forms.Padding(5);

            hideSystemProcessBtn.Name = "hideSystemProcessBtn";

            hideSystemProcessBtn.Size = new System.Drawing.Size(267, 38);

            hideSystemProcessBtn.TabIndex = 12;

            hideSystemProcessBtn.Text = "Show System Processes";

            hideSystemProcessBtn.UseVisualStyleBackColor = true;

            hideSystemProcessBtn.Click += hideSystemProcessBtn_Click;

            // 

            // closeDriverOnExitBox

            // 

            closeDriverOnExitBox.AutoSize = true;

            closeDriverOnExitBox.Location = new System.Drawing.Point(640, 16);

            closeDriverOnExitBox.Margin = new System.Windows.Forms.Padding(5);

            closeDriverOnExitBox.Name = "closeDriverOnExitBox";

            closeDriverOnExitBox.Size = new System.Drawing.Size(185, 24);

            closeDriverOnExitBox.TabIndex = 13;

            closeDriverOnExitBox.Text = "Close Driver on Exit";

            closeDriverOnExitBox.UseVisualStyleBackColor = true;

            closeDriverOnExitBox.CheckedChanged += closeDriverOnExitBox_CheckedChanged;

            // 

            // processList

            // 

            processList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            processList.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);

            processList.BorderStyle = System.Windows.Forms.BorderStyle.None;

            processList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { PIDHeader, NameHeader, PathHeader, BaseAddressHeader, EntryPointHeader, ImageSizeHeader, ImageTypeHeader });

            processList.ContextMenuStrip = contextMenuStrip1;

            processList.ForeColor = System.Drawing.Color.Silver;

            processList.FullRowSelect = true;

            processList.Location = new System.Drawing.Point(20, 88);

            processList.Margin = new System.Windows.Forms.Padding(5);

            processList.MultiSelect = false;

            processList.Name = "processList";

            processList.OwnerDraw = true;

            processList.Size = new System.Drawing.Size(2235, 755);

            processList.Sorting = System.Windows.Forms.SortOrder.Ascending;

            processList.TabIndex = 2;

            processList.UseCompatibleStateImageBehavior = false;

            processList.View = System.Windows.Forms.View.Details;

            // 

            // PIDHeader

            // 

            PIDHeader.Text = "PID";

            PIDHeader.Width = 40;

            // 

            // NameHeader

            // 

            NameHeader.Text = "Name";

            NameHeader.Width = 59;

            // 

            // PathHeader

            // 

            PathHeader.Text = "Path";

            PathHeader.Width = 47;

            // 

            // BaseAddressHeader

            // 

            BaseAddressHeader.Text = "Base Address";

            BaseAddressHeader.Width = 118;

            // 

            // EntryPointHeader

            // 

            EntryPointHeader.Text = "Entry Point";

            EntryPointHeader.Width = 98;

            // 

            // ImageSizeHeader

            // 

            ImageSizeHeader.Text = "Image Size";

            ImageSizeHeader.Width = 250;

            // 

            // ImageTypeHeader

            // 

            ImageTypeHeader.Text = "Image Type";

            ImageTypeHeader.Width = 150;

            // 

            // providerBtn

            // 

            providerBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            providerBtn.Location = new System.Drawing.Point(2039, 8);

            providerBtn.Margin = new System.Windows.Forms.Padding(5);

            providerBtn.Name = "providerBtn";

            providerBtn.Size = new System.Drawing.Size(217, 38);

            providerBtn.TabIndex = 17;

            providerBtn.Text = "Provider Selector";

            providerBtn.UseVisualStyleBackColor = true;

            providerBtn.Click += providerBtn_Click;

            // 

            // kernelModulesBtn

            // 

            kernelModulesBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            kernelModulesBtn.Location = new System.Drawing.Point(1592, 8);

            kernelModulesBtn.Margin = new System.Windows.Forms.Padding(5);

            kernelModulesBtn.Name = "kernelModulesBtn";

            kernelModulesBtn.Size = new System.Drawing.Size(160, 38);

            kernelModulesBtn.TabIndex = 19;

            kernelModulesBtn.Text = "Kernel Drivers";

            kernelModulesBtn.UseVisualStyleBackColor = true;

            kernelModulesBtn.Click += kernelModulesBtn_Click;

            // 

            // antiantiDebuggerToolsBox

            // 

            antiantiDebuggerToolsBox.AutoSize = true;

            antiantiDebuggerToolsBox.Location = new System.Drawing.Point(835, 16);

            antiantiDebuggerToolsBox.Margin = new System.Windows.Forms.Padding(5);

            antiantiDebuggerToolsBox.Name = "antiantiDebuggerToolsBox";

            antiantiDebuggerToolsBox.Size = new System.Drawing.Size(355, 24);

            antiantiDebuggerToolsBox.TabIndex = 18;

            antiantiDebuggerToolsBox.Text = "Enable Anti Anti Debugger Tools Detection";

            antiantiDebuggerToolsBox.UseVisualStyleBackColor = true;

            antiantiDebuggerToolsBox.CheckedChanged += antiantiDebuggerToolsBox_CheckedChanged;

            // 

            // FormFixTimer

            // 

            FormFixTimer.Interval = 125;

            FormFixTimer.Tick += FormFixTimer_Tick;

            // 

            // providerInfoLbl

            // 

            providerInfoLbl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            providerInfoLbl.AutoEllipsis = true;

            providerInfoLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);

            providerInfoLbl.ForeColor = System.Drawing.Color.FromArgb(170, 200, 235);

            providerInfoLbl.Location = new System.Drawing.Point(20, 50);

            providerInfoLbl.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);

            providerInfoLbl.Name = "providerInfoLbl";

            providerInfoLbl.Size = new System.Drawing.Size(2235, 30);

            providerInfoLbl.TabIndex = 20;

            providerInfoLbl.Text = "Current Provider: (loading...)";

            providerInfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 

            // DumperForm

            // 

            AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);

            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            ClientSize = new System.Drawing.Size(2275, 1231);

            Controls.Add(providerInfoLbl);

            Controls.Add(antiantiDebuggerToolsBox);

            Controls.Add(kernelModulesBtn);

            Controls.Add(providerBtn);

            Controls.Add(closeDriverOnExitBox);

            Controls.Add(hideSystemProcessBtn);

            Controls.Add(autoRefreshCheckBox);

            Controls.Add(refreshBtn);

            Controls.Add(fileDumpBtn);

            Controls.Add(groupBox1);

            Controls.Add(processList);

            DoubleBuffered = true;

            Margin = new System.Windows.Forms.Padding(5);

            MinimumSize = new System.Drawing.Size(1485, 893);

            Name = "DumperForm";

            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Text = "KsDumper 11";

            Load += Dumper_Load;

            groupBox1.ResumeLayout(false);

            contextMenuStrip1.ResumeLayout(false);

            ResumeLayout(false);

            PerformLayout();

        }

        private System.ComponentModel.IContainer components = null;
        private KsDumper11.Utility.ProcessListView processList;
        private System.Windows.Forms.ColumnHeader PIDHeader;
        private System.Windows.Forms.ColumnHeader NameHeader;
        private System.Windows.Forms.ColumnHeader PathHeader;
        private System.Windows.Forms.ColumnHeader BaseAddressHeader;
        private System.Windows.Forms.ColumnHeader EntryPointHeader;
        private System.Windows.Forms.ColumnHeader ImageSizeHeader;
        private System.Windows.Forms.ColumnHeader ImageTypeHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox logsTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dumpMainModuleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewModulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suspendProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumeProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killProcessToolStripMenuItem;
        private System.Windows.Forms.Button fileDumpBtn;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.CheckBox autoRefreshCheckBox;
        private System.Windows.Forms.Button hideSystemProcessBtn;
        private System.Windows.Forms.CheckBox closeDriverOnExitBox;
        private System.Windows.Forms.Button providerBtn;
        private System.Windows.Forms.Button kernelModulesBtn;
        private System.Windows.Forms.CheckBox antiantiDebuggerToolsBox;

        private System.Windows.Forms.Timer FormFixTimer;

        private System.Windows.Forms.Label providerInfoLbl;

    }
}
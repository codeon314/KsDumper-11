namespace KsDumper11
{
    public partial class KernelModulesForm : global::System.Windows.Forms.Form
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
            moduleList = new System.Windows.Forms.ListView();
            NameHeader = new System.Windows.Forms.ColumnHeader();
            BaseHeader = new System.Windows.Forms.ColumnHeader();
            SizeHeader = new System.Windows.Forms.ColumnHeader();
            PathHeader = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            dumpDriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshBtn = new System.Windows.Forms.Button();
            kdFormFix = new System.Windows.Forms.Timer(components);
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // moduleList
            // 
            moduleList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            moduleList.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
            moduleList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { NameHeader, BaseHeader, SizeHeader, PathHeader });
            moduleList.ContextMenuStrip = contextMenuStrip1;
            moduleList.ForeColor = System.Drawing.Color.Silver;
            moduleList.FullRowSelect = true;
            moduleList.Location = new System.Drawing.Point(20, 74);
            moduleList.Margin = new System.Windows.Forms.Padding(5);
            moduleList.Name = "moduleList";
            moduleList.Size = new System.Drawing.Size(1254, 1053);
            moduleList.TabIndex = 0;
            moduleList.UseCompatibleStateImageBehavior = false;
            moduleList.View = System.Windows.Forms.View.Details;
            // 
            // NameHeader
            // 
            NameHeader.Text = "Driver";
            NameHeader.Width = 59;
            // 
            // BaseHeader
            // 
            BaseHeader.Text = "Base Address";
            BaseHeader.Width = 118;
            // 
            // SizeHeader
            // 
            SizeHeader.Text = "Size";
            SizeHeader.Width = 43;
            // 
            // PathHeader
            // 
            PathHeader.Text = "Path";
            PathHeader.Width = 1030;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { dumpDriverToolStripMenuItem, copyAddressToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(238, 68);
            // 
            // dumpDriverToolStripMenuItem
            // 
            dumpDriverToolStripMenuItem.Name = "dumpDriverToolStripMenuItem";
            dumpDriverToolStripMenuItem.Size = new System.Drawing.Size(237, 32);
            dumpDriverToolStripMenuItem.Text = "Dump Driver Code";
            dumpDriverToolStripMenuItem.Click += dumpDriverToolStripMenuItem_Click;
            // 
            // copyAddressToolStripMenuItem
            // 
            copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
            copyAddressToolStripMenuItem.Size = new System.Drawing.Size(237, 32);
            copyAddressToolStripMenuItem.Text = "Copy Base Address";
            copyAddressToolStripMenuItem.Click += copyAddressToolStripMenuItem_Click;
            // 
            // refreshBtn
            // 
            refreshBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            refreshBtn.Location = new System.Drawing.Point(1152, 18);
            refreshBtn.Margin = new System.Windows.Forms.Padding(5);
            refreshBtn.Name = "refreshBtn";
            refreshBtn.Size = new System.Drawing.Size(125, 38);
            refreshBtn.TabIndex = 11;
            refreshBtn.Text = "Refresh";
            refreshBtn.UseVisualStyleBackColor = true;
            refreshBtn.Click += refreshBtn_Click;
            // 
            // kdFormFix
            // 
            kdFormFix.Interval = 125;
            kdFormFix.Tick += kdFormFix_Tick;
            // 
            // KernelModulesForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1297, 1148);
            Controls.Add(refreshBtn);
            Controls.Add(moduleList);
            Margin = new System.Windows.Forms.Padding(5);
            MinimumSize = new System.Drawing.Size(985, 585);
            Name = "KernelModulesForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "KsDumper 11 - Kernel Drivers";
            Load += KernelModulesForm_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView moduleList;
        private System.Windows.Forms.ColumnHeader NameHeader;
        private System.Windows.Forms.ColumnHeader BaseHeader;
        private System.Windows.Forms.ColumnHeader SizeHeader;
        private System.Windows.Forms.ColumnHeader PathHeader;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dumpDriverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Timer kdFormFix;
    }
}
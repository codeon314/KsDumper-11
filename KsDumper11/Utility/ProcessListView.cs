using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KsDumper11.Utility
{
	// Token: 0x0200000B RID: 11
	public class ProcessListView : ListView
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000046B8 File Offset: 0x000028B8
  // (set) Token: 0x06000063 RID: 99 RVA: 0x000046C0 File Offset: 0x000028C0
  [DefaultValue(true)]
  public bool SystemProcessesHidden { get; set; } = true;

		// Token: 0x06000064 RID: 100 RVA: 0x000046C9
		public ProcessListView()
		{
			base.OwnerDraw = true;
			this.DoubleBuffered = true;
			base.Sorting = SortOrder.Ascending;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000046F9
		public void LoadProcesses(ProcessSummary[] processSummaries)
		{
			this.processCache = processSummaries;
			this.ReloadItems();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000470A
		public void ShowSystemProcesses()
		{
			this.SystemProcessesHidden = false;
			this.ReloadItems();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000471C
		public void HideSystemProcesses()
		{
			this.SystemProcessesHidden = true;
			this.ReloadItems();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000472E
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			e.DrawDefault = true;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000473C
		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawBackground();
			using (StringFormat sf = new StringFormat())
			{
				sf.Alignment = StringAlignment.Center;
				using (Font headerFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold))
				{
					e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
					e.Graphics.DrawString(e.Header.Text, headerFont, new SolidBrush(this.ForeColor), e.Bounds, sf);
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000047F8
		private void ReloadItems()
		{
			base.BeginUpdate();
			int idx = 0;
			bool flag = base.SelectedIndices.Count > 0;
			if (flag)
			{
				idx = base.SelectedIndices[0];
				bool flag2 = idx == -1;
				if (flag2)
				{
					idx = 0;
				}
			}
			base.Items.Clear();
			string systemRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows).ToLower();
			foreach (ProcessSummary processSummary in this.processCache)
			{
				bool flag3 = this.SystemProcessesHidden && (processSummary.MainModuleFileName.ToLower().StartsWith(systemRootFolder) || processSummary.MainModuleFileName.StartsWith("\\"));
				if (!flag3)
				{
					ListViewItem lvi = new ListViewItem(processSummary.ProcessId.ToString());
					lvi.BackColor = this.BackColor;
					lvi.ForeColor = this.ForeColor;
					lvi.SubItems.Add(Path.GetFileName(processSummary.MainModuleFileName));
					lvi.SubItems.Add(processSummary.MainModuleFileName);
					lvi.SubItems.Add(string.Format("0x{0:x8}", processSummary.MainModuleBase));
					lvi.SubItems.Add(string.Format("0x{0:x8}", processSummary.MainModuleEntryPoint));
					lvi.SubItems.Add(string.Format("0x{0:x4}", processSummary.MainModuleImageSize));
					lvi.SubItems.Add(processSummary.IsWOW64 ? "x86" : "x64");
					lvi.Tag = processSummary;
					base.Items.Add(lvi);
				}
			}
			base.ListViewItemSorter = new ProcessListView.ProcessListViewItemComparer(this.sortColumnIndex, base.Sorting);
			base.Sort();
			base.Items[idx].Selected = true;

			// Auto-size every column so the header text and the widest subitem
			// are always fully visible (nothing gets truncated with an ellipsis).
			// Setting Width to -2 instructs the ListView to size the column to
			// fit the header and the longest subitem. This is re-applied on
			// every reload so widths stay fitted while Auto Refresh runs.
			for (int i = 0; i < this.Columns.Count; i++)
			{
				this.Columns[i].Width = -2;
			}

			// The custom header painter (OnDrawColumnHeader) renders the header
			// text using a bold 9pt "Microsoft Sans Serif" font, which is wider
			// than the font WinForms uses when it computes the -2 auto-size.
			// As a result the "Image Size" and "Image Type" headers (columns 5
			// and 6) can be clipped by their own bounds. Enforce a minimum width
			// for exactly those two columns based on the actual rendered bold
			// header text; every other column is left untouched.
			EnsureHeaderFitsForColumns(5, 6);

			base.EndUpdate();
		}

		// Measures the header text of the given column indices using the exact
		// font used by OnDrawColumnHeader and, if -2 auto-size produced a width
		// too narrow for the bold header, expands the column to fit. Only the
		// specified columns are ever modified.
		private void EnsureHeaderFitsForColumns(params int[] columnIndices)
		{
			if (this.Columns.Count == 0 || columnIndices == null || columnIndices.Length == 0)
			{
				return;
			}

			using (Font boldHeaderFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold))
			using (Graphics g = this.CreateGraphics())
			{
				// Small horizontal padding so the text is not flush against the
				// column border after centering.
				const int headerPadding = 12;

				foreach (int columnIndex in columnIndices)
				{
					if (columnIndex < 0 || columnIndex >= this.Columns.Count)
					{
						continue;
					}

					ColumnHeader column = this.Columns[columnIndex];
					if (column == null)
					{
						continue;
					}

					int requiredWidth = (int)Math.Ceiling(
						g.MeasureString(column.Text, boldHeaderFont).Width) + headerPadding;

					if (column.Width < requiredWidth)
					{
						column.Width = requiredWidth;
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000049FC
		protected override void OnColumnClick(ColumnClickEventArgs e)
		{
			bool flag = e.Column != this.sortColumnIndex;
			if (flag)
			{
				this.sortColumnIndex = e.Column;
				base.Sorting = SortOrder.Ascending;
			}
			else
			{
				bool flag2 = base.Sorting == SortOrder.Ascending;
				if (flag2)
				{
					base.Sorting = SortOrder.Descending;
				}
				else
				{
					base.Sorting = SortOrder.Ascending;
				}
			}
			base.ListViewItemSorter = new ProcessListView.ProcessListViewItemComparer(e.Column, base.Sorting);
			base.Sort();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004A7C
		protected override void WndProc(ref Message m)
		{
			bool flag = m.Msg == 1;
			if (flag)
			{
			}
			base.WndProc(ref m);
		}

		// Token: 0x0600006D RID: 109
		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

		// Token: 0x0400003C RID: 60
		private int sortColumnIndex = 1;

		// Token: 0x0400003D RID: 61
		private ProcessSummary[] processCache;

		// Token: 0x02000025 RID: 37
		private class ProcessListViewItemComparer : IComparer
		{
			// Token: 0x060000F3 RID: 243 RVA: 0x000063F0 File Offset: 0x000045F0
			public ProcessListViewItemComparer(int columnIndex, SortOrder sortOrder)
			{
				this.columnIndex = columnIndex;
				this.sortOrder = sortOrder;
			}

			// Token: 0x060000F4 RID: 244 RVA: 0x00006408 File Offset: 0x00004608
			public int Compare(object x, object y)
			{
				bool flag = x is ListViewItem && y is ListViewItem;
				if (flag)
				{
					ProcessSummary p = ((ListViewItem)x).Tag as ProcessSummary;
					ProcessSummary p2 = ((ListViewItem)y).Tag as ProcessSummary;
					bool flag2 = p != null && p2 != null;
					if (flag2)
					{
						int result = 0;
						switch (this.columnIndex)
						{
						case 0:
							result = p.ProcessId.CompareTo(p2.ProcessId);
							break;
						case 1:
							result = p.ProcessName.CompareTo(p2.ProcessName);
							break;
						case 2:
							result = p.MainModuleFileName.CompareTo(p2.MainModuleFileName);
							break;
						case 3:
							result = p.MainModuleBase.CompareTo(p2.MainModuleBase);
							break;
						case 4:
							result = p.MainModuleEntryPoint.CompareTo(p2.MainModuleEntryPoint);
							break;
						case 5:
							result = p.MainModuleImageSize.CompareTo(p2.MainModuleImageSize);
							break;
						case 6:
							result = p.IsWOW64.CompareTo(p2.IsWOW64);
							break;
						}
						bool flag3 = this.sortOrder == SortOrder.Descending;
						if (flag3)
						{
							result = -result;
						}
						return result;
					}
				}
				return 0;
			}

			// Token: 0x040000B8 RID: 184
			private readonly int columnIndex;

			// Token: 0x040000B9 RID: 185
			private readonly SortOrder sortOrder;
		}
	}
}
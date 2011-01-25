namespace Kohina {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.xmlToCBButton = new System.Windows.Forms.ToolStripButton();
			this.readXMLBtn = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.leftTabs = new System.Windows.Forms.TabControl();
			this.propsTab = new System.Windows.Forms.TabPage();
			this.propTablePanel = new System.Windows.Forms.TableLayoutPanel();
			this.constPropGrid = new System.Windows.Forms.PropertyGrid();
			this.pinPropGrid = new System.Windows.Forms.PropertyGrid();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.nodeCatalogList = new System.Windows.Forms.ListView();
			this.nodeTabCtrl = new System.Windows.Forms.TabControl();
			this.connTabPage = new System.Windows.Forms.TabPage();
			this.connView = new Kohina.ConnectionViewPanel();
			this.nodeListPage = new System.Windows.Forms.TabPage();
			this.nodeListView = new System.Windows.Forms.ListView();
			this.nodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.leftTabs.SuspendLayout();
			this.propsTab.SuspendLayout();
			this.propTablePanel.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.nodeTabCtrl.SuspendLayout();
			this.connTabPage.SuspendLayout();
			this.nodeListPage.SuspendLayout();
			this.nodeContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 540);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(984, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.xmlToCBButton,
									this.readXMLBtn});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(984, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// xmlToCBButton
			// 
			this.xmlToCBButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.xmlToCBButton.Image = ((System.Drawing.Image)(resources.GetObject("xmlToCBButton.Image")));
			this.xmlToCBButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.xmlToCBButton.Name = "xmlToCBButton";
			this.xmlToCBButton.Size = new System.Drawing.Size(70, 22);
			this.xmlToCBButton.Text = "XML To CB";
			this.xmlToCBButton.Click += new System.EventHandler(this.XmlToCBButtonClick);
			// 
			// readXMLBtn
			// 
			this.readXMLBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.readXMLBtn.Image = ((System.Drawing.Image)(resources.GetObject("readXMLBtn.Image")));
			this.readXMLBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.readXMLBtn.Name = "readXMLBtn";
			this.readXMLBtn.Size = new System.Drawing.Size(122, 22);
			this.readXMLBtn.Text = "XML on CB To World";
			this.readXMLBtn.Click += new System.EventHandler(this.ReadXMLBtnClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.leftTabs);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.nodeTabCtrl);
			this.splitContainer1.Size = new System.Drawing.Size(984, 515);
			this.splitContainer1.SplitterDistance = 180;
			this.splitContainer1.TabIndex = 2;
			// 
			// leftTabs
			// 
			this.leftTabs.Controls.Add(this.propsTab);
			this.leftTabs.Controls.Add(this.tabPage2);
			this.leftTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.leftTabs.Location = new System.Drawing.Point(0, 0);
			this.leftTabs.Name = "leftTabs";
			this.leftTabs.SelectedIndex = 0;
			this.leftTabs.Size = new System.Drawing.Size(180, 515);
			this.leftTabs.TabIndex = 0;
			// 
			// propsTab
			// 
			this.propsTab.Controls.Add(this.propTablePanel);
			this.propsTab.Location = new System.Drawing.Point(4, 22);
			this.propsTab.Margin = new System.Windows.Forms.Padding(0);
			this.propsTab.Name = "propsTab";
			this.propsTab.Size = new System.Drawing.Size(172, 489);
			this.propsTab.TabIndex = 0;
			this.propsTab.Text = "Properties";
			this.propsTab.UseVisualStyleBackColor = true;
			// 
			// propTablePanel
			// 
			this.propTablePanel.ColumnCount = 1;
			this.propTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.propTablePanel.Controls.Add(this.constPropGrid, 0, 1);
			this.propTablePanel.Controls.Add(this.pinPropGrid, 0, 0);
			this.propTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propTablePanel.Location = new System.Drawing.Point(0, 0);
			this.propTablePanel.Name = "propTablePanel";
			this.propTablePanel.RowCount = 2;
			this.propTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.propTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.propTablePanel.Size = new System.Drawing.Size(172, 489);
			this.propTablePanel.TabIndex = 0;
			// 
			// constPropGrid
			// 
			this.constPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.constPropGrid.HelpVisible = false;
			this.constPropGrid.Location = new System.Drawing.Point(0, 244);
			this.constPropGrid.Margin = new System.Windows.Forms.Padding(0);
			this.constPropGrid.Name = "constPropGrid";
			this.constPropGrid.Size = new System.Drawing.Size(172, 245);
			this.constPropGrid.TabIndex = 2;
			this.constPropGrid.ToolbarVisible = false;
			this.constPropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PinPropGridPropertyValueChanged);
			// 
			// pinPropGrid
			// 
			this.pinPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pinPropGrid.HelpVisible = false;
			this.pinPropGrid.Location = new System.Drawing.Point(0, 0);
			this.pinPropGrid.Margin = new System.Windows.Forms.Padding(0);
			this.pinPropGrid.Name = "pinPropGrid";
			this.pinPropGrid.Size = new System.Drawing.Size(172, 244);
			this.pinPropGrid.TabIndex = 1;
			this.pinPropGrid.ToolbarVisible = false;
			this.pinPropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PinPropGridPropertyValueChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.nodeCatalogList);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(172, 489);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Node Catalog";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// nodeCatalogList
			// 
			this.nodeCatalogList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeCatalogList.FullRowSelect = true;
			this.nodeCatalogList.LabelWrap = false;
			this.nodeCatalogList.Location = new System.Drawing.Point(3, 3);
			this.nodeCatalogList.Name = "nodeCatalogList";
			this.nodeCatalogList.Size = new System.Drawing.Size(166, 483);
			this.nodeCatalogList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.nodeCatalogList.TabIndex = 0;
			this.nodeCatalogList.TileSize = new System.Drawing.Size(150, 15);
			this.nodeCatalogList.UseCompatibleStateImageBehavior = false;
			this.nodeCatalogList.View = System.Windows.Forms.View.Tile;
			this.nodeCatalogList.ItemActivate += new System.EventHandler(this.NodeCatalogListItemActivate);
			// 
			// nodeTabCtrl
			// 
			this.nodeTabCtrl.Controls.Add(this.connTabPage);
			this.nodeTabCtrl.Controls.Add(this.nodeListPage);
			this.nodeTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeTabCtrl.Location = new System.Drawing.Point(0, 0);
			this.nodeTabCtrl.Name = "nodeTabCtrl";
			this.nodeTabCtrl.SelectedIndex = 0;
			this.nodeTabCtrl.Size = new System.Drawing.Size(800, 515);
			this.nodeTabCtrl.TabIndex = 0;
			// 
			// connTabPage
			// 
			this.connTabPage.Controls.Add(this.connView);
			this.connTabPage.Location = new System.Drawing.Point(4, 22);
			this.connTabPage.Name = "connTabPage";
			this.connTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.connTabPage.Size = new System.Drawing.Size(792, 489);
			this.connTabPage.TabIndex = 1;
			this.connTabPage.Text = "Connection View";
			this.connTabPage.UseVisualStyleBackColor = true;
			// 
			// connView
			// 
			this.connView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.connView.Location = new System.Drawing.Point(3, 3);
			this.connView.Name = "connView";
			this.connView.SelectedNode = null;
			this.connView.Size = new System.Drawing.Size(786, 483);
			this.connView.TabIndex = 0;
			this.connView.World = null;
			this.connView.NodeSelected += new Kohina.NodeEventHandler(this.ConnectionViewPanel1NodeSelected);
			// 
			// nodeListPage
			// 
			this.nodeListPage.Controls.Add(this.nodeListView);
			this.nodeListPage.Location = new System.Drawing.Point(4, 22);
			this.nodeListPage.Name = "nodeListPage";
			this.nodeListPage.Padding = new System.Windows.Forms.Padding(3);
			this.nodeListPage.Size = new System.Drawing.Size(792, 489);
			this.nodeListPage.TabIndex = 0;
			this.nodeListPage.Text = "Node List";
			this.nodeListPage.UseVisualStyleBackColor = true;
			// 
			// nodeListView
			// 
			this.nodeListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeListView.Location = new System.Drawing.Point(3, 3);
			this.nodeListView.Name = "nodeListView";
			this.nodeListView.Size = new System.Drawing.Size(786, 483);
			this.nodeListView.TabIndex = 0;
			this.nodeListView.UseCompatibleStateImageBehavior = false;
			this.nodeListView.View = System.Windows.Forms.View.Tile;
			this.nodeListView.ItemActivate += new System.EventHandler(this.NodeListViewItemActivate);
			// 
			// nodeContextMenu
			// 
			this.nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.deleteToolStripMenuItem});
			this.nodeContextMenu.Name = "nodeContextMenu";
			this.nodeContextMenu.ShowImageMargin = false;
			this.nodeContextMenu.Size = new System.Drawing.Size(128, 48);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 562);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Kohina<3";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.leftTabs.ResumeLayout(false);
			this.propsTab.ResumeLayout(false);
			this.propTablePanel.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.nodeTabCtrl.ResumeLayout(false);
			this.connTabPage.ResumeLayout(false);
			this.nodeListPage.ResumeLayout(false);
			this.nodeContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip nodeContextMenu;
		private System.Windows.Forms.TabPage nodeListPage;
		private System.Windows.Forms.ToolStripButton readXMLBtn;
		private System.Windows.Forms.ToolStripButton xmlToCBButton;
		private Kohina.ConnectionViewPanel connView;
		private System.Windows.Forms.TabControl nodeTabCtrl;
		private System.Windows.Forms.TabPage connTabPage;
		private System.Windows.Forms.TabControl leftTabs;
		private System.Windows.Forms.TabPage propsTab;
		private System.Windows.Forms.ListView nodeCatalogList;
		private System.Windows.Forms.TableLayoutPanel propTablePanel;
		private System.Windows.Forms.PropertyGrid constPropGrid;
		private System.Windows.Forms.ListView nodeListView;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PropertyGrid pinPropGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;

		#endregion
	}
}


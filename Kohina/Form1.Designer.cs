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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.xmlToCBButton = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.leftTabs = new System.Windows.Forms.TabControl();
			this.propsTab = new System.Windows.Forms.TabPage();
			this.propTablePanel = new System.Windows.Forms.TableLayoutPanel();
			this.constPropGrid = new System.Windows.Forms.PropertyGrid();
			this.pinPropGrid = new System.Windows.Forms.PropertyGrid();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.nodeCatalogList = new System.Windows.Forms.ListView();
			this.nodeTabCtrl = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.nodeListView = new System.Windows.Forms.ListView();
			this.connTabPage = new System.Windows.Forms.TabPage();
			this.connectionViewPanel1 = new Kohina.ConnectionViewPanel();
			this.readXMLBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.leftTabs.SuspendLayout();
			this.propsTab.SuspendLayout();
			this.propTablePanel.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.nodeTabCtrl.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.connTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 539);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(844, 22);
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
			this.toolStrip1.Size = new System.Drawing.Size(844, 25);
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
			this.splitContainer1.Size = new System.Drawing.Size(844, 514);
			this.splitContainer1.SplitterDistance = 281;
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
			this.leftTabs.Size = new System.Drawing.Size(281, 514);
			this.leftTabs.TabIndex = 0;
			// 
			// propsTab
			// 
			this.propsTab.Controls.Add(this.propTablePanel);
			this.propsTab.Location = new System.Drawing.Point(4, 22);
			this.propsTab.Margin = new System.Windows.Forms.Padding(0);
			this.propsTab.Name = "propsTab";
			this.propsTab.Size = new System.Drawing.Size(273, 488);
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
			this.propTablePanel.Size = new System.Drawing.Size(273, 488);
			this.propTablePanel.TabIndex = 0;
			// 
			// constPropGrid
			// 
			this.constPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.constPropGrid.HelpVisible = false;
			this.constPropGrid.Location = new System.Drawing.Point(0, 244);
			this.constPropGrid.Margin = new System.Windows.Forms.Padding(0);
			this.constPropGrid.Name = "constPropGrid";
			this.constPropGrid.Size = new System.Drawing.Size(273, 244);
			this.constPropGrid.TabIndex = 2;
			this.constPropGrid.ToolbarVisible = false;
			// 
			// pinPropGrid
			// 
			this.pinPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pinPropGrid.HelpVisible = false;
			this.pinPropGrid.Location = new System.Drawing.Point(0, 0);
			this.pinPropGrid.Margin = new System.Windows.Forms.Padding(0);
			this.pinPropGrid.Name = "pinPropGrid";
			this.pinPropGrid.Size = new System.Drawing.Size(273, 244);
			this.pinPropGrid.TabIndex = 1;
			this.pinPropGrid.ToolbarVisible = false;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.nodeCatalogList);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(273, 488);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Node Catalog";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// nodeCatalogList
			// 
			this.nodeCatalogList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeCatalogList.Location = new System.Drawing.Point(3, 3);
			this.nodeCatalogList.Name = "nodeCatalogList";
			this.nodeCatalogList.Size = new System.Drawing.Size(267, 482);
			this.nodeCatalogList.TabIndex = 0;
			this.nodeCatalogList.UseCompatibleStateImageBehavior = false;
			this.nodeCatalogList.View = System.Windows.Forms.View.List;
			this.nodeCatalogList.ItemActivate += new System.EventHandler(this.NodeCatalogListItemActivate);
			// 
			// nodeTabCtrl
			// 
			this.nodeTabCtrl.Controls.Add(this.tabPage3);
			this.nodeTabCtrl.Controls.Add(this.connTabPage);
			this.nodeTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeTabCtrl.Location = new System.Drawing.Point(0, 0);
			this.nodeTabCtrl.Name = "nodeTabCtrl";
			this.nodeTabCtrl.SelectedIndex = 0;
			this.nodeTabCtrl.Size = new System.Drawing.Size(559, 514);
			this.nodeTabCtrl.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.nodeListView);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(551, 488);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "Node List";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// nodeListView
			// 
			this.nodeListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nodeListView.Location = new System.Drawing.Point(3, 3);
			this.nodeListView.Name = "nodeListView";
			this.nodeListView.Size = new System.Drawing.Size(545, 482);
			this.nodeListView.TabIndex = 0;
			this.nodeListView.UseCompatibleStateImageBehavior = false;
			this.nodeListView.View = System.Windows.Forms.View.Tile;
			this.nodeListView.ItemActivate += new System.EventHandler(this.NodeListViewItemActivate);
			// 
			// connTabPage
			// 
			this.connTabPage.Controls.Add(this.connectionViewPanel1);
			this.connTabPage.Location = new System.Drawing.Point(4, 22);
			this.connTabPage.Name = "connTabPage";
			this.connTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.connTabPage.Size = new System.Drawing.Size(551, 488);
			this.connTabPage.TabIndex = 1;
			this.connTabPage.Text = "Connection View";
			this.connTabPage.UseVisualStyleBackColor = true;
			// 
			// connectionViewPanel1
			// 
			this.connectionViewPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.connectionViewPanel1.Location = new System.Drawing.Point(3, 3);
			this.connectionViewPanel1.Name = "connectionViewPanel1";
			this.connectionViewPanel1.Size = new System.Drawing.Size(545, 482);
			this.connectionViewPanel1.TabIndex = 0;
			this.connectionViewPanel1.World = null;
			this.connectionViewPanel1.NodeSelected += new Kohina.NodeEventHandler(this.ConnectionViewPanel1NodeSelected);
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(844, 561);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
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
			this.tabPage3.ResumeLayout(false);
			this.connTabPage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripButton readXMLBtn;
		private System.Windows.Forms.ToolStripButton xmlToCBButton;
		private Kohina.ConnectionViewPanel connectionViewPanel1;
		private System.Windows.Forms.TabControl nodeTabCtrl;
		private System.Windows.Forms.TabPage connTabPage;
		private System.Windows.Forms.TabControl leftTabs;
		private System.Windows.Forms.TabPage propsTab;
		private System.Windows.Forms.ListView nodeCatalogList;
		private System.Windows.Forms.TableLayoutPanel propTablePanel;
		private System.Windows.Forms.PropertyGrid constPropGrid;
		private System.Windows.Forms.ListView nodeListView;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.PropertyGrid pinPropGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;

		#endregion
	}
}


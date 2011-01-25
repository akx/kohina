using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

using Kohina.Nodes;

namespace Kohina {
	public partial class MainForm : Form {
		World w;
		public MainForm() {
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			InitializeComponent();
			
			
			w = new World();
			
			/*SolidColorBitmap scb = new SolidColorBitmap();			
			ColorBlender cb = new ColorBlender();
			
			w.AddNode(scb);
			w.AddNode(cb);
			scb.GetPinByName("Color").Connect(cb.GetPinByName("Output"));*/
			
			BitmapFileReader bf1 = new BitmapFileReader();
			bf1.FileName = "Base.jpg";
			BitmapFileReader bf2 = new BitmapFileReader();
			bf2.FileName = "Blend.jpg";
			
			BitmapXBlender bb = new BitmapXBlender();
			bb.BlendMode = BlendMode.ColorBurn;
			bb.SetPinConstantValue("Mix", 1.0);
			w.AddNode(bf1);
			w.AddNode(bf2);
			w.AddNode(bb);
			bb.GetPinByName("Input 1").Connect(bf1.GetPinByName("Output"));
			bb.GetPinByName("Input 2").Connect(bf2.GetPinByName("Output"));
			
			
			/*
			TimeToLFO t2l = new TimeToLFO();
			t2l.SetPinConstantValue("Amplitude", 0.5);
			t2l.SetPinConstantValue("Bias", 0.5);
			w.AddNode(t2l);
			bb.GetPinByName("Mix").Connect(t2l.GetPinByName("Output"));
			*/
			
			BitmapChannelRemap cr = new BitmapChannelRemap();
			w.AddNode(cr);
			cr.GetPinByName("Input").Connect(bb.GetPinByName("Output"));
			
			BitmapViewer bv = new BitmapViewer();
			w.AddNode(bv);
			bv.UpdateInterval = 1000 / 20;
			bv.GetPinByName("Input").Connect(cr.GetPinByName("Output"));
			
			
			connView.World = w;
			NodeRegistry.Instance.Populate();
			UpdateNodeCatalogView();
			UpdateNodeListView();
			nodeTabCtrl.SelectedTab = connTabPage;
			bv.Interact();
			
		}
		
		void UpdateNodeCatalogView() {
			
			nodeCatalogList.BeginUpdate();
			nodeCatalogList.Items.Clear();
			nodeCatalogList.Groups.Clear();
			Dictionary<string, List<ListViewItem>> byCategory = new Dictionary<string, List<ListViewItem>>();
			foreach(Type t in NodeRegistry.Instance.Enumerate()) {
				object[] attrs = t.GetCustomAttributes(typeof(NodeCategoryAttribute), true);
				string category = "Other";
				if(attrs.Length > 0) category = (attrs[0] as NodeCategoryAttribute).ToString();
				if(!byCategory.ContainsKey(category)) byCategory[category] = new List<ListViewItem>();
				ListViewItem lvi = new ListViewItem() {
					Text = t.Name,
					Tag = t
				};
				byCategory[category].Add(lvi);
				
			}
			foreach(var kv in byCategory) {
				ListViewGroup lvg = new ListViewGroup(kv.Key);
				nodeCatalogList.Groups.Add(lvg);
				foreach(var item in kv.Value) {
					item.Group = lvg;
					nodeCatalogList.Items.Add(item);
				}
			}
			nodeCatalogList.EndUpdate();
		}
		
		void UpdateNodeListView() {
			nodeListView.BeginUpdate();
			nodeListView.Items.Clear();
			foreach(Node n in w.GetNodes()) {
				ListViewItem lvi = new ListViewItem() {
					Tag = n,
					Text = n.ToString()
				};
				nodeListView.Items.Add(lvi);
			}
			nodeListView.EndUpdate();
			connView.Refresh();
		}
		
		void NodeListViewItemActivate(object sender, EventArgs e)
		{
			try {
				SelectNode((Node)(nodeListView.SelectedItems[0].Tag));
			} catch {
				
			}				
		}
		
		void SelectNode(Node node) {
			NodePropertyProxy pproxy = null;
			if(node != null) {
				pproxy = node.GetPropertyProxy();
			}
			pinPropGrid.SelectedObject = null;
			constPropGrid.SelectedObject = null;
			pinPropGrid.SelectedObject = pproxy;
			constPropGrid.SelectedObject = node;
			connView.SelectedNode = node;
			
		}
		
		void NodeCatalogListItemActivate(object sender, EventArgs e)
		{
			Type t = ((sender as ListView).SelectedItems[0].Tag) as Type;
			Node n = (Activator.CreateInstance(t)) as Node;
			w.AddNode(n);
			UpdateNodeListView();
			SelectNode(n);
			connView.Refresh();
			leftTabs.SelectedTab = propsTab;
		}
		
		
		
		void ConnectionViewPanel1NodeSelected(object sender, NodeEventArgs e)
		{
			SelectNode(e.Node);
			if(e.WasRightClick) {
				nodeContextMenu.Show(connView, e.MouseArgs.Location);
			}
		}
		
		void XmlToCBButtonClick(object sender, EventArgs e)
		{
			XElement el = w.SerializeToXML();
			Clipboard.SetText(el.ToString());
		}
		
		void ReadXMLBtnClick(object sender, EventArgs e)
		{
			XElement el = XElement.Parse(Clipboard.GetText());
			w.ParseXML(el);
			UpdateNodeListView();
		}
		
		void PinPropGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			connView.Refresh();
		}
		
		void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			Node n = connView.SelectedNode;
			SelectNode(null);
			w.RemoveNode(n);
			n.Dispose();
			connView.Refresh();
			
		}
	}
}

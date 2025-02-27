﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Kohina
{
		
	class CVNodeInfo {
		public Node Node { get; set; }
		public int Cardinality { get; set; }
		public Point Location { get; set; }
		public int Height { get; set; }
	}
	class CVPinInfo {
		public Pin Pin { get; set; }
		public Point LocalLocation { get; set; }
		public Point Location { get; set; }
	}
	
	public class NodeEventArgs: EventArgs {
		public Node Node { get; set; }
		public bool WasRightClick;
		public MouseEventArgs MouseArgs;
		
		public NodeEventArgs(Node n, bool wasRightClick, MouseEventArgs mouseEventArgs) {
			this.Node = n;
			this.WasRightClick = wasRightClick;
			this.MouseArgs = mouseEventArgs;
		}
	}
	
	public delegate void NodeEventHandler(object sender, NodeEventArgs e);
	
	public class ConnectionViewPanel: Panel
	{
		World world;
		Dictionary<Node, CVNodeInfo> nodeInfos = new Dictionary<Node, CVNodeInfo>();
		Dictionary<Pin, CVPinInfo> pinInfos = new Dictionary<Pin, CVPinInfo>();
		Timer updateTimer = new Timer() { Interval = 30 };
		Pin holdingPin = null;
		Pin highlightPin = null;
		Node highlightNode = null;
		Point mousePos;
		bool mbDown;
		public event NodeEventHandler NodeSelected;
		
		static Pen connPen = new Pen(Color.DarkGray) {
			Width = 2,
	    };
		static Pen aConnPen = new Pen(Color.OrangeRed) {
			Width = 4,
	    };
		static Pen nodePen = Pens.Black;
		static Pen aNodePen = new Pen(nodePen.Color) {
			Width = 2,
			Alignment = PenAlignment.Outset
		};
		
		static Font titleFont = new Font("Segoe UI", 8, FontStyle.Bold);
		static Font noteFont = new Font("Segoe UI", 6);
		static Font pinFont = new Font("Segoe UI", 8);
		
		const int NODE_WIDTH = 150;
				
		
		Node selectedNode = null;
		
		public Node SelectedNode {
			get { return selectedNode; }
			set { selectedNode = value; }
		}
		
		
		protected virtual void OnNodeSelected(NodeEventArgs e)
		{
			if (NodeSelected != null) {
				NodeSelected(this, e);
			}
		}
		
		public World World {
			get { return world; }
			set { world = value; }
		}
		
		public ConnectionViewPanel()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			updateTimer.Tick += new EventHandler(updateTimer_Tick);
		}

		void updateTimer_Tick(object sender, EventArgs e)
		{
			Refresh();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			Render(e.Graphics);
		}
		
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			
		}
		
		private CVPinInfo FindPinAt(Point loc) {
			foreach(CVPinInfo cpi in pinInfos.Values) {
				if(loc.ManhattanDistance(cpi.Location) < 12) return cpi;
			}
			return null;
		}
		
		private CVNodeInfo FindNodeTitleAt(Point loc) {
			foreach(CVNodeInfo cni in nodeInfos.Values) {
				if(loc.X >= cni.Location.X && loc.X <= cni.Location.X + NODE_WIDTH && loc.Y >= cni.Location.Y && loc.Y <= cni.Location.Y + 20) {
					return cni;
				}
			}
			return null;
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			mousePos = e.Location;
			CVPinInfo pinAtCur = FindPinAt(e.Location);
			if(pinAtCur != null) {
				holdingPin = pinAtCur.Pin;
				updateTimer.Enabled = true;
				Capture = true;
				return;
			}
			CVNodeInfo nodeAtCur = FindNodeTitleAt(e.Location);
			if(nodeAtCur != null) {
				if((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
					nodeAtCur.Node.Interact();
					Refresh();
					return;
				} else {
					OnNodeSelected(new NodeEventArgs(nodeAtCur.Node, (e.Button == MouseButtons.Right), e));
				}
			}
			mbDown = true;
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			mousePos = e.Location;
			if(!mbDown) {
				CVPinInfo pinAtCur = FindPinAt(e.Location);
				Pin hlPin = (pinAtCur != null) ? pinAtCur.Pin : null;
				if(hlPin != highlightPin) {
					highlightPin = hlPin;
					Refresh();
				}
				CVNodeInfo nodeAtCur = FindNodeTitleAt(e.Location);
				Node hlNode = (nodeAtCur != null) ? nodeAtCur.Node : null;
				if(hlNode != highlightNode) {
					highlightNode = hlNode;
					Refresh();
				}
				
			}
			Cursor = Cursors.Arrow;
			if(holdingPin != null) {
				CVPinInfo pinAtCur = FindPinAt(e.Location);
				if(pinAtCur != null) {
					if(pinAtCur.Pin.Direction != holdingPin.Direction && holdingPin.DataType == pinAtCur.Pin.DataType) {
						Cursor = Cursors.UpArrow;
					} else {
						Cursor = Cursors.No;
					}
				} else {
					Cursor = Cursors.SizeAll;
				}
			} else if(highlightPin != null || highlightNode != null) {
				Cursor = Cursors.Hand;
			}
		}
		
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(holdingPin != null) {
				Pin connectThis = holdingPin;
				Pin connectTo = null;
				CVPinInfo pinAtCur = FindPinAt(e.Location);
				if(pinAtCur != null && pinAtCur.Pin.Direction != holdingPin.Direction) {
					if(holdingPin.Direction == PinDirection.Input) {
						connectThis = holdingPin;
						connectTo = pinAtCur.Pin;
					} else {
						connectThis = pinAtCur.Pin;
						connectTo = holdingPin;
					}
				}
				if(connectThis != null) {
					try {
						connectThis.ConnectedOutputPin = connectTo;
					} catch(Exception exc) {
						MessageBox.Show(exc.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				holdingPin = null;
			}
			updateTimer.Enabled = false;
			mousePos = e.Location;
			Capture = false;
			mbDown = false;
			Refresh();
		}
		
		private Pin CheckAutoConnectValidity() {
			if(holdingPin == null || highlightNode == null || highlightNode == holdingPin.Owner) return null;
			foreach(Pin p in highlightNode.AllPins) {
				if(!p.Connected && p.Direction != holdingPin.Direction && p.DataType == holdingPin.DataType) return p;
			}
			return null;
		}
		
		private void Render(Graphics g) {
			if(world == null) {
				g.Clear(Color.Red);
				return;
			}
			nodeInfos.Clear();
			pinInfos.Clear();
			
			Dictionary<int, List<CVNodeInfo>> cardinalityBins = new Dictionary<int, List<CVNodeInfo>>();
			
			foreach(Node n in world.GetNodes()) {
				int c = n.RecurGetConnectedInputs();
				CVNodeInfo ni = new CVNodeInfo() {
					Node = n,
					Cardinality = c//InputPins.Count()//(p)=>p.Connected)
				};
				nodeInfos[n] = ni;
				if(!cardinalityBins.ContainsKey(c)) {
					cardinalityBins[c] = new List<CVNodeInfo>();
				}
				cardinalityBins[c].Add(ni);
			}
			int nC = 0;
			foreach(var c in cardinalityBins.Keys.OrderBy((k)=>k)) {
				foreach(CVNodeInfo ni in cardinalityBins[c]) {
					ni.Cardinality = nC;
				}
				nC ++;
			}
			int maxCard = nC;//nodeInfos.Values.Select((ni)=>ni.Cardinality).Max();
			int[] yByCard = new int[maxCard + 1];
			
			foreach(CVNodeInfo cni in nodeInfos.Values) {
				Node n = cni.Node;
				cni.Height = n.AllPins.Count() * 15 + 25;
				cni.Location = new Point(5 + (cni.Cardinality) * 170, 5 + yByCard[cni.Cardinality]);
				yByCard[cni.Cardinality] += cni.Height + 10;
				int i = 0;
				foreach(Pin p in cni.Node.AllPins) {
					CVPinInfo cpi = new CVPinInfo();
					cpi.Pin = p;
					cpi.LocalLocation = new Point((p.Direction == PinDirection.Input ? -2 : 147), 24 + i * 15);
					cpi.Location = new Point(cni.Location.X + cpi.LocalLocation.X, cni.Location.Y + cpi.LocalLocation.Y);
					pinInfos[p] = cpi;
					i ++;
				}
			}
			
			
			
			GraphicsState baseState = g.Save();
			g.Clear(SystemColors.Window);
			g.SmoothingMode = SmoothingMode.HighQuality;
			foreach(CVPinInfo cpi in pinInfos.Values) {
				
				if(cpi.Pin.Direction == PinDirection.Input && cpi.Pin.Connected) {
					Pin op = cpi.Pin.ConnectedOutputPin;
					CVPinInfo opi = pinInfos[op];
					g.DrawLine((cpi.Pin == highlightPin || opi.Pin == highlightPin ? aConnPen : connPen), cpi.Location.X + 2, cpi.Location.Y + 2, opi.Location.X + 2, opi.Location.Y + 2);
				}
			}
			if(holdingPin != null) {
				CVPinInfo pi = pinInfos[holdingPin];
				g.DrawLine(Pens.SlateBlue, pi.Location.X + 2, pi.Location.Y + 2, mousePos.X, mousePos.Y);
			}
			g.SmoothingMode = SmoothingMode.None;
			foreach(CVNodeInfo cni in nodeInfos.Values) {
				GraphicsState gs = g.Save();
				g.TranslateTransform(cni.Location.X, cni.Location.Y);
				g.FillRectangle(Brushes.WhiteSmoke, 0, 0, NODE_WIDTH, cni.Height);
				g.DrawRectangle((cni.Node == selectedNode ? aNodePen : nodePen), 0, 0, NODE_WIDTH, cni.Height);
				g.DrawString(cni.Node.Name, titleFont, (cni.Node == highlightNode ? Brushes.Blue : Brushes.Black), 3, 3);
				if(!string.IsNullOrEmpty(cni.Node.Note)) {
					g.DrawString(cni.Node.Note, noteFont, Brushes.Black, 3, cni.Height + 3);
				}

				foreach(Pin p in cni.Node.AllPins) {
					CVPinInfo pi = pinInfos[p];
					Brush br = p.Direction == PinDirection.Output ? Brushes.Cyan : (p.Connected ? Brushes.Lime : Brushes.Orange);
					
					g.FillRectangle(br, pi.LocalLocation.X, pi.LocalLocation.Y, 6, 6);
					g.DrawRectangle(Pens.Black, pi.LocalLocation.X, pi.LocalLocation.Y, 6, 6);
					StringFormat sf = StringFormat.GenericDefault;
					sf.Alignment = (p.Direction == PinDirection.Input) ? StringAlignment.Near : StringAlignment.Far;
					g.DrawString(p.Name, pinFont, Brushes.Black, new RectangleF(6, pi.LocalLocation.Y - 3, NODE_WIDTH - 10, 15), sf);

				}
				
				g.Restore(gs);
			}
			g.Restore(baseState);
		}
		
		
	}
}

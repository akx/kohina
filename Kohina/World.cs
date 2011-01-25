using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
namespace Kohina
{
	/// <summary>
	/// Description of World.
	/// </summary>
	public class World
	{
		List<Node> nodes;
		public World()
		{
			nodes = new List<Node>();
		}
		
		public void AddNode(Node n) {
			if(!nodes.Contains(n)) {
				nodes.Add(n);
				n.World = this;
			}
		}
		
		public void RemoveNode(Node toRemove) {
			nodes.Remove(toRemove);
			foreach(Node n in nodes) {
				foreach(Pin p in n.InputPins) {
					if(p.Connected && p.ConnectedOutputPin.Owner == toRemove) p.ConnectedOutputPin = null;
				}
			}
		}
		
		public IEnumerable<Node> GetNodes() {
			foreach(Node n in nodes) yield return n;
		}
		
		public XElement SerializeToXML() {
			XElement el = new XElement("World");
			foreach(Node n in GetNodes()) {
				el.Add(n.SerializeToXML());
			}
			return el;
		}
		
		public void Dismantle() {
			foreach(Node n in nodes) {
				foreach(Pin p in n.InputPins) {
					p.ConnectedOutputPin = null;
				}
				n.Dispose();
			}
			nodes.Clear();
		}
		
		private class NodeAndEl {
			public Node Node { get; set; }
			public XElement Element { get; set; }
		}
		
		public void ParseXML(XElement el) {
			Dismantle();
			Dictionary<Guid, NodeAndEl> guidMap = new Dictionary<Guid, NodeAndEl>();
			foreach(XElement nodeEl in el.Elements("Node")) {
				Node n = Node.ParseXML(nodeEl);
				guidMap[n.Guid] = new NodeAndEl{ Node = n, Element = nodeEl };
				AddNode(n);
			}
			foreach(var ge in guidMap) {
				Node n = ge.Value.Node;
				XElement nodeEl = ge.Value.Element;
				foreach(XElement pinEl in nodeEl.Elements("Pin")) {
					Pin thisPin = n.GetPinByName(pinEl.Attribute("name").Value);
					if(thisPin != null) {
						XAttribute constAttr = pinEl.Attribute("const");
						if(constAttr != null) thisPin.SetConstantValue(constAttr.Value);
						XAttribute connAttr = pinEl.Attribute("conn");
						if(connAttr != null) {
							string[] bits = connAttr.Value.Split(':');
							Guid otherGuid = new Guid(bits[0]);
							if(guidMap.ContainsKey(otherGuid)) {
								Node otherNode = guidMap[otherGuid].Node;
								Pin otherPin = otherNode.GetPinByName(bits[1]);
								if(otherPin != null) {
									thisPin.ConnectedOutputPin = otherPin;
								}
							}
						}
					}
				}
			}
		}
	}
}

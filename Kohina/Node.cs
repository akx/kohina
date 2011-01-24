using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

using Kohina.Events;

namespace Kohina {
	public class Node: IDisposable {
		World world;
		Guid guid;
		
		string name = "";
		string note = "";
		
		[Browsable(false)]
		public Guid Guid {
			get { return guid; }
		}
		
		public string Name {
			get { return name; }
			set { name = value; }
		}
		
		public string Note {
			get { return note; }
			set { note = value; }
		}
		
		
		
		
		[Browsable(false)]
		public World World {
			get { return world; }
			set { world = value; }
		}
		
		
		[Browsable(false)]
		public virtual IEnumerable<Pin> InputPins {
			get { return GetAttrPinsByDirection(PinDirection.Input); }
		}
		[Browsable(false)]
		public virtual IEnumerable<Pin> OutputPins {
			get { return GetAttrPinsByDirection(PinDirection.Output); }
		}
		
		[Browsable(false)]
		public virtual IEnumerable<Pin> AllPins {
			get {
				foreach(Pin p in InputPins) yield return p;
				foreach(Pin p in OutputPins) yield return p;
			}
		}
		
		
		
		private const BindingFlags pinBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		
		private PinAttribs GetPinAttribs(FieldInfo fi) {
			Attribute[] attrs = (Attribute[])fi.GetCustomAttributes(typeof(PinAttribs), true);
			return (attrs.Length > 0 ? attrs[0] as PinAttribs : null);
		}
		
		private IEnumerable<Pin> GetAttrPinsByDirection(PinDirection direction) {
			foreach(FieldInfo fi in GetType().GetFields(pinBinding)) {
				if(fi.FieldType == (typeof(Pin))) {
					PinAttribs pa = GetPinAttribs(fi);
					if(pa != null && pa.direction == direction) yield return (Pin)fi.GetValue(this);
				}
			}
		}
		
		public Pin GetPinByName(string name) {
			Type t = GetType();
			foreach(FieldInfo fi in GetType().GetFields(pinBinding)) {
				//Debug.WriteLine(t.Name + ": " + fi.Name + "; " + fi.FieldType.Name);
				if(fi.FieldType == typeof(Pin)) {
					Pin pin = (Pin)fi.GetValue(this);
					if(pin.Name == name) return pin;
				}
			}
			return null;
		}
		
		public void SetPinConstantValue(string name, object obj) {
			Pin p = GetPinByName(name);
			if(p != null) p.ConstantValue = obj;
		}

		public virtual object GetPinValue(Pin pin, PinRequest request) {
			return null;
			//return default(T);//null;
		}
		
		public virtual void Interact() {
			
		}
		
		protected void InitPins() {
			Debug.WriteLine("Initing pins for " + this.ToString());
			foreach(FieldInfo fi in GetType().GetFields(pinBinding)) {
				if(fi.FieldType == typeof(Pin)) {
					PinAttribs attr = GetPinAttribs(fi);
					//Debug.WriteLine("  " + fi.Name + ": " + attr.ToString());
					if(attr != null) {
						string name = attr.name;
						string desc = "???";
						Pin p = Activator.CreateInstance(fi.FieldType, new object[] { name, desc, attr.dataType, attr.direction, this }) as Pin;
						fi.SetValue(this, p);
						p.SetConstantValue(GetPinDefaultConstantValue(p));
					}
				}
			}
		}
		
		protected virtual object GetPinDefaultConstantValue(Pin p) {
			return null;
		}
		
		protected Node() {
			guid = Guid.NewGuid();
			name = GetType().Name;
			InitPins();
		}
		
		public NodePropertyProxy GetPropertyProxy() {
			return new NodePropertyProxy(this);
		}
		
		public int RecurGetConnectedInputs() {
			var bs = InputPins.Where((p)=>p.Connected);
			int nB = bs.Count();
			int nR = bs.Select((p)=>p.ConnectedOutputPin.Owner.RecurGetConnectedInputs()).Sum();
			//Debug.Print("{0} rgci = {1}", this, n);
			return nB + nR;
		}
		
		public virtual XElement SerializeToXML() {
			XElement el = new XElement("Node");
			el.SetAttributeValue("type", GetType().Name);
			el.SetAttributeValue("guid", guid);
			if(!string.IsNullOrEmpty(name)) el.SetAttributeValue("name", name);
			if(!string.IsNullOrEmpty(note)) el.SetAttributeValue("note", note);
			foreach(Pin p in AllPins) {
				XElement pcEl = p.SerializeToXML();
				if(pcEl != null) el.Add(pcEl);
			}
			foreach(PropertyInfo pi in GetType().GetProperties()) {
				bool canAdd = false;
				XElement pEl = new XElement("Property");
				pEl.SetAttributeValue("name", pi.Name);
				if(pi.PropertyType == typeof(string)) {
					pEl.SetAttributeValue("type", "string");
					pEl.SetAttributeValue("value", pi.GetValue(this, null));
					canAdd = true;
				}
				if(pi.PropertyType == typeof(Color)) {
					pEl.SetAttributeValue("type", "color");
					pEl.SetAttributeValue("value", Colors.Converters.SixHexPlusAlpha((Color)pi.GetValue(this, null)));
				}
				if(canAdd) {
					el.Add(pEl);
				}
			}
			return el;
		}
		
		public static Node ParseXML(XElement el) {
			if(el.Name != "Node") throw new Exception("HERP");
			string typeName = el.Attribute("type").Value;
			Type t = NodeRegistry.Instance.Get(typeName);
			Debug.Print("{0} => {1}", typeName, t);
			Node n = (Node)Activator.CreateInstance(t);
			n.guid = new Guid(el.Attribute("guid").Value);
			n.name = el.GetAttributeOrDefault("name", n.name);
			n.note = el.GetAttributeOrDefault("note", "");
			foreach(XElement pEl in el.Elements("Property")) {
				PropertyInfo pi = t.GetProperty(pEl.Attribute("name").Value);
				if(pi != null) {
					string type = pEl.Attribute("type").Value;
					switch(type) {
						case "string":
							pi.SetValue(n, pEl.Attribute("value").Value, null);
							break;
						default:
							throw new Exception("DERP!");
					}
				}
			}
			return n;
		}
		
		public virtual void Dispose()
		{
		
		}
		
		public override string ToString()
		{
			return string.Format("{0} (GUID={1})", GetType().Name, guid);
		}
		
	}
}

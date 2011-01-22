using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Kohina.Events;

namespace Kohina {
	public enum PinDirection {
		Input,
		Output
	}
	
	public delegate object ConstantValueCtor();
	
	public class PinAttribs: Attribute {
		public string name;
		public string description;
		public DataType dataType;
		public PinDirection direction;
		

		public PinAttribs(string name, PinDirection direction, DataType dataType) {
			this.name = name;
			this.description = "";
			this.dataType = dataType;
			this.direction = direction;
		}
		
	}
	
	
	
	public class Pin {
		Node owner;
		DataType dataType;
		string name;
		string description;
		PinDirection direction;
		
		object constantValue;
		Pin connectedOutputPin;

		public Node Owner { get { return owner; } }
		public DataType DataType { get { return dataType; } }
		public PinDirection Direction { get { return direction; } }
		public string Name { get { return name; } }
		public string Description { get { return description; } }
		public object ConstantValue {
			get { return constantValue; }
			set { constantValue = value; }
		}
		
		
		public Pin ConnectedOutputPin {
			get { return connectedOutputPin; }
			set {
				if(value != null) {
					if(value.owner.World != owner.World) throw new Exception("Wrong world!");
					if(value.dataType != dataType) throw new Exception("Mismatching data types:\nConnecting " + value.ToString() + " to " + this.ToString());
					if(value.direction == PinDirection.Input) throw new Exception("Connect output pins to inputs, not this way around");
				}
				connectedOutputPin = value;
			}
		}
		
		public bool Connected { get { return (connectedOutputPin != null); } }
		
		
		public Pin(string name, string description, DataType dataType, PinDirection direction, Node owner)
		{
			this.name = name;
			this.description = description;
			this.direction = direction;
			this.dataType = dataType;
			this.owner = owner;
		}		
		
		public void Connect(Pin p) {
			ConnectedOutputPin = p;
		}
		
		public T Read<T>(PinRequest request) {
			if(connectedOutputPin == null) return (T)(constantValue ?? default(T));
			T val = (T)connectedOutputPin.GetOwnerValue(request);
			return val;//if(val == default(T)) return default(T);
		}
		
		public T Read<T>() {
			return Read<T>(null);
		}
		
		public object GetOwnerValue(PinRequest request) {
			return owner.GetPinValue(this, request);
		}
		
		public override string ToString()
		{
			return string.Format("{0}.{1} ({2})", this.owner.GetType().Name, this.name, this.dataType);
		}
		
		public void SetConstantValue(object cv) {
			switch(dataType) {
				case DataType.Number:
					if(cv == null) {
						constantValue = 0;
					} else {
						try {
							constantValue = (double)cv;
						} catch(InvalidCastException) {
							double d = 0;
							Double.TryParse(cv.ToString(), out d);
							constantValue = d;
						}
					}
					break;
				case DataType.String:
					constantValue = (cv == null ? "" : cv.ToString());
					break;
				case DataType.Color:
					if(cv is Color) {
						constantValue = (Color)cv;
					} else {
						constantValue = (cv == null ? Color.Black : Kohina.Colors.Utils.Recognize(cv.ToString()));
					}
					break;
				case DataType.Bitmap:
					constantValue = null; // TODO
					break;
				default:
					Debug.Print("SetConstantValue dt={0} cv={1}", dataType, cv);
					break;
			}
		}
		
		public virtual XElement SerializeToXML() {
			bool doAdd = false;
			XElement el = new XElement("Pin");
			el.SetAttributeValue("name", name);
			if(connectedOutputPin != null) {
				el.SetAttributeValue("conn", connectedOutputPin.Owner.Guid + ":" + connectedOutputPin.Name);
				doAdd = true;
			}
			if(direction == PinDirection.Input && constantValue != null) {
				el.SetAttributeValue("const", constantValue.ToString());
				doAdd = true;
			}
			return (doAdd ? el : null);
		}
	}
}

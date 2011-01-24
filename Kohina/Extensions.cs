
using System;
using System.Drawing;
using System.Xml.Linq;

namespace Kohina
{
	/// <summary>
	/// Description of Extensions.
	/// </summary>
	public static class Extensions
	{
		public static int ManhattanDistance(this Point p, Point q) {
			return Math.Abs(p.X - q.X) + Math.Abs(p.Y - q.Y);
		}
		
		public static Rectangle GetEntireRect(this Bitmap b) {
			return new Rectangle(0, 0, b.Width, b.Height);
		}
		
		public static Bitmap ResizeIfNeeded(this Bitmap b, Size desiredSize) {
			if(b.Size != desiredSize) {
				Bitmap nb = new Bitmap(b, desiredSize);
				b.Dispose();
				return nb;
			}
			return b;
		}
		
		public static string GetAttributeOrDefault(this XElement el, string attrName, string defaultValue) {
			XAttribute attr = el.Attribute(attrName);
			if(attr == null) return defaultValue;
			return attr.Value;
		}
		
		public delegate T StringConverter<T>(string s);
		public static T GetAttributeOrDefault<T>(this XElement el, string attrName, T defaultValue, StringConverter<T> converter) {
			XAttribute attr = el.Attribute(attrName);
			if(attr == null) return defaultValue;
			if((typeof(T)).IsSubclassOf(typeof(Enum))) return (T)Enum.Parse(typeof(T), attr.Value, true);
			return converter(attr.Value);
		}
	}
}

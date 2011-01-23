
using System;
using System.Drawing;

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
	}
}


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
	}
}


using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Kohina
{
	/// <summary>
	/// Description of Utils.
	/// </summary>
	public static class Utils
	{
		[ThreadStatic]
		private static StringBuilder makeKeySB = new StringBuilder();
		public static string MakeKey(params object[] objs) {
			makeKeySB.Length = 0;
			foreach(object obj in objs) {
				makeKeySB.Append('@');
				makeKeySB.Append(obj.ToString());
			}
			return makeKeySB.ToString();
		}
	}
}

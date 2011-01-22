using System;
using System.Runtime.InteropServices;

namespace Kohina {
	public enum BlendMode {
		Normal = 1,
		Lighten = 2,
		Darken = 3,
		Multiply = 4,
		Average = 5,
		Add = 6,
		Subtract = 7,
		Difference = 8,
		Negation = 9,
		Screen = 10,
		Exclusion = 11,
		Overlay = 12,
		SoftLight = 13,
		HardLight = 14,
		ColorDodge = 15,
		ColorBurn = 16,
		LinearDodge = 17,
		LinearBurn = 18,
		LinearLight = 19,
		VividLight = 20,
		PinLight = 21,
		HardMix = 22,
		Reflect = 23,
		Glow = 24,
		Phoenix = 25,
		_Dummy
	};
	public class CBlend {
		private static class Impl {
			[DllImport("cblend.dll")]
			public static extern void Blend(int mode, IntPtr buf1, IntPtr buf2, UInt32 lenBytes, float O);
			[DllImport("cblend.dll")]
			public static extern void BlendV(int mode, IntPtr buf1, IntPtr buf2, UInt32 lenBytes, float O);
			[DllImport("cblend.dll")]
			public static extern void Seed(UInt32 seed);
			[DllImport("cblend.dll")]
			public static extern void Noise(IntPtr buf, UInt32 lenBytes, UInt32 aMask, UInt32 oMask);
			[DllImport("cblend.dll")]
			public static extern void Clear(IntPtr buf, UInt32 lenBytes, UInt32 color, UInt32 oMask);

		}
		
		public static void Seed(int seed) {
			unchecked { Impl.Seed((UInt32)seed); }
		}
		
		public static void Noise(IntPtr buf, UInt32 lenBytes, UInt32 cMask, UInt32 oMask) {
			Impl.Noise(buf, lenBytes, cMask, oMask);
		}
		public static void Noise(IntPtr buf, UInt32 lenBytes, UInt32 cMask, UInt32 oMask) {
			Impl.Noise(buf, lenBytes, cMask, oMask);
		}
		
		
		public static void Blend(BlendMode mode, IntPtr buf1, IntPtr buf2, UInt32 lenBytes, float O, bool useSrcAlpha) {
			if(useSrcAlpha) {
				Impl.BlendV((int)mode, buf1, buf2, lenBytes, O);
			} else {
				Impl.Blend((int)mode, buf1, buf2, lenBytes, O);
			}
			
		}
	}
}

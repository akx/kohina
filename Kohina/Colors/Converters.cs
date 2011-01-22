/*
 * Date: 7.7.2010
 * Time: 20:11
 */
using System;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;

namespace Kohina.Colors
{
	delegate string ColorToStringConverterDelegate(Color c);
	
	
	public class Converters
	{
		public static bool PercentageDecimal = false;
		private static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
		{
		    double M = Math.Max(color.R, Math.Max(color.G, color.B)) / 255.0d;
		    double m = Math.Min(color.R, Math.Min(color.G, color.B)) / 255.0d;
		    double C = M - m;
		    hue = color.GetHue();
		    value = M;
		    saturation = (C == 0) ? 0 : C / value;
		}

		private static void ColorToHSL(Color color, out double hue, out double saturation, out double lightness)
		{
		    double M = Math.Max(color.R, Math.Max(color.G, color.B)) / 255.0d;
		    double m = Math.Min(color.R, Math.Min(color.G, color.B)) / 255.0d;
		    double C = M - m;
		    hue = color.GetHue();
		    lightness = 0.5d * (M + m);
		    saturation = (C == 0) ? 0 : (lightness <= 0.5 ? C / 2 * lightness : C / (2 - 2 * lightness));
		}

		private static string DecimalFormat(double d) {
			if(PercentageDecimal) return String.Format(NumberFormatInfo.InvariantInfo, "{0:0}%", d * 100);
			return String.Format(NumberFormatInfo.InvariantInfo, "{0:0.###}", d);
		}
		
		public static string SixHex(Color c) {
			string s = Convert.ToString(c.R, 16).PadLeft(2, '0') + Convert.ToString(c.G, 16).PadLeft(2, '0') + Convert.ToString(c.B, 16).PadLeft(2, '0');
			if(s[0] == s[1] && s[2] == s[3] && s[4] == s[5]) return "#" + s[0] + s[2] + s[4];
			return "#" + s;
		}
		
		public static string SixHexPS(Color c) {
			return ColorTranslator.ToHtml(c).Trim('#');
		}
		
		public static string SixHexPlusAlpha(Color c) {
			string s = ColorTranslator.ToHtml(c);
			if(c.A != 255) s += "@" + Convert.ToString((int)c.A, 16);
			return s;
		}
		
		public static string CSSRGB(Color c) {
			return String.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
		}
		
		public static string CSSHSV(Color c) {
			double hue, sat, val;
			ColorToHSV(c, out hue, out sat, out val); 
			return String.Format(NumberFormatInfo.InvariantInfo, "hsv({0:0.##},{1},{2})", hue, DecimalFormat(sat), DecimalFormat(val));
		}
		
		public static string DecimalRGB(Color c) {
			return String.Format(NumberFormatInfo.InvariantInfo, "RGB {0},{1},{2}", DecimalFormat(c.R / 255.0), DecimalFormat(c.G / 255.0), DecimalFormat(c.B / 255.0));
		}
		
		public static string HSL(Color c) {
			double hue, sat, lgt;
			ColorToHSL(c, out hue, out sat, out lgt);
			return String.Format(NumberFormatInfo.InvariantInfo, "HSL {0:0.##},{1},{2}", hue, DecimalFormat(sat), DecimalFormat(lgt));
		}

		public static string CMYK(Color c) {
			double C, M, Y, K;
			if(c.GetBrightness() == 0) {
				C = M = Y = 0;
				K = 1;
			} else {
				C = 1 - c.R / 255.0;
				M = 1 - c.G / 255.0;
				Y = 1 - c.B / 255.0;
				K = Math.Min(C, Math.Min(M, Y));
				C = (C - K) / (1.0 - K);
				M = (M - K) / (1.0 - K);
				Y = (Y - K) / (1.0 - K);
			}
			return String.Format(NumberFormatInfo.InvariantInfo, "CMYK {0} {1} {2} {3}", DecimalFormat(C), DecimalFormat(M), DecimalFormat(Y), DecimalFormat(K));
		}
		
	}
}

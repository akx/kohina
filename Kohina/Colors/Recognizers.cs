/*
 * Date: 7.7.2010
 * Time: 22:36
 */
using System;
using System.Drawing;
using System.Globalization;

namespace Kohina.Colors
{
	delegate Color? StringToColorConverterDelegate(string s);

	public class Recognizers
	{
		private static bool IsXDigit(char c)
		{
		  if ('0' <= c && c <= '9') return true;
		  if ('a' <= c && c <= 'f') return true;
		  if ('A' <= c && c <= 'F') return true;
		  return false;
		}
		private static bool IsXDigits(string s) { foreach(char c in s) if(!IsXDigit(c)) return false; return true; }
		
		public static Color? FromHex(string s) {
			if(s.Length == 4 && s.StartsWith("#")) { // likely a short CSS-style hex
				s = "#" + s[0] + s[0] + s[1] + s[1] + s[2] + s[2];
			}
			if(s.Length == 6 && !s.StartsWith("#") && IsXDigits(s)) { // maybe a PS-style hex
				s = "#" + s;
			}
			try {
				return ColorTranslator.FromHtml(s);
			} catch {
				return null;
			}
		}
		
		private static Color ColorFromHSV(double hue, double saturation, double value)
		{
		    int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
		    double f = hue / 60 - Math.Floor(hue / 60);
		
		    value = value * 255;
		    int v = Convert.ToInt32(value);
		    int p = Convert.ToInt32(value * (1 - saturation));
		    int q = Convert.ToInt32(value * (1 - f * saturation));
		    int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
		
		    if (hi == 0)
		        return Color.FromArgb(255, v, t, p);
		    else if (hi == 1)
		        return Color.FromArgb(255, q, v, p);
		    else if (hi == 2)
		        return Color.FromArgb(255, p, v, t);
		    else if (hi == 3)
		        return Color.FromArgb(255, p, q, v);
		    else if (hi == 4)
		        return Color.FromArgb(255, t, p, v);
		    else
		        return Color.FromArgb(255, v, p, q);
		}
		
      // Given H,S,L in range of 0-1
      // Returns a Color (RGB struct) in range of 0-255
      private static Color ColorFromHSL(double h, double sl, double l)
      {
            double v;
            double r,g,b;
 
            r = g = b = l;   // default to gray
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                  double m;
                  double sv;
                  int sextant;
                  double fract, vsf, mid1, mid2;
 
                  m = l + l - v;
                  sv = (v - m ) / v;
                  h *= 6.0;
                  sextant = (int)h;
                  fract = h - sextant;
                  vsf = v * sv * fract;
                  mid1 = m + vsf;
                  mid2 = v - vsf;
                  switch (sextant)
                  {
                        case 0:
                              r = v;
                              g = mid1;
                              b = m;
                              break;
                        case 1:
                              r = mid2;
                              g = v;
                              b = m;
                              break;
                        case 2:
                              r = m;
                              g = v;
                              b = mid1;
                              break;
                        case 3:
                              r = m;
                              g = mid2;
                              b = v;
                              break;
                        case 4:
                              r = mid1;
                              g = m;
                              b = v;
                              break;
                        case 5:
                              r = v;
                              g = m;
                              b = mid2;
                              break;
                  }
            }
            return Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
      }		

		
		private static double[] ParseTripletString(string s, bool dontScaleFirst) {
			string[] rgbS = s.Split(new char[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);
			double[] rgb = new double[3];
			for(int i=0;i<3;i++) {
				double v = -1;
				try {
					v = Convert.ToDouble(rgbS[i]);
				} catch {
					try {
						v = Convert.ToDouble(rgbS[i], NumberFormatInfo.InvariantInfo);
					} catch {
					
					}
				}
				if(v < 0) return null;
				rgb[i] = v;
			}
			if((rgb[0] > 1 || rgb[1] > 1 || rgb[2] > 1)) {
				if(!dontScaleFirst) rgb[0] /= 255.0;
				rgb[1] /= 255.0;
				rgb[2] /= 255.0;
			}
			return rgb;
		}
		
		public static Color? FromRGB(string s) {
			s = s.ToLower().Trim('r','g','b','(',')','{','}');
			double[] triplet = ParseTripletString(s, false);
			if(triplet != null) {
				return Color.FromArgb((int)(triplet[0] * 255.0), (int)(triplet[1] * 255.0), (int)(triplet[2] * 255.0));
			}
			return null; //
		}
		
		public static Color? FromHSV(string s) {
			if(!s.ToLower().StartsWith("hsv")) return null;
			s = s.ToLower().Trim('h','s','v','(',')','{','}');
			double[] triplet = ParseTripletString(s, true);
			if(triplet != null) {
				return ColorFromHSV(triplet[0], triplet[1], triplet[2]);
			}
			return null; //
		}
      
      	public static Color? FromHSL(string s) {
			if(!s.ToLower().StartsWith("hsl")) return null;
			s = s.ToLower().Trim('h','s','l','(',')','{','}');
			double[] triplet = ParseTripletString(s, true);
			if(triplet != null) {
				return ColorFromHSL(triplet[0], triplet[1], triplet[2]);
			}
			return null; //
		}
	}
}

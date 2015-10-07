using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{
    public class Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Color() 
        {
            
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

         public Color(int a, int r, int g, int b)
        {
            A = (byte)a;
            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
        }

         public byte GetHue()
         {
             return 0;
         }

        public static Color FromArgb(int a, int r, int g, int b)
        {
            return new Color(a,r,g,b);
        }

        public static Color White
        {
            get{return new Color(255, 255, 255, 255);}
        }

        public static Color Black
        {
            get { return new Color(255, 0, 0, 0); }
        }

        public static Color Transparent
        {
            get { return new Color(0, 0, 0, 0); }
        }

        public static Color Red
        {
            get { return new Color(255, 255, 0, 0); }
        }

        public static Color Green
        {
            get { return new Color(255, 255, 0, 0); }
        }

        public static Color Blue
        {
            get { return new Color(255, 0, 255, 0); }
        }

        public static Color Yellow
        {
            get { return new Color(255, 255, 255, 0); }
        }
        public static Color LightCoral
        {
            get { return new Color(255, 240,128,128); }
        }
        public static Color ForestGreen
        {
            get { return new Color(255, 34, 139, 34); } 
        }
        public static Color Magenta
        {
            get { return new Color(255, 255, 0, 255); }
        }
    }
}

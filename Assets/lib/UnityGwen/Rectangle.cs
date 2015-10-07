using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{
    public struct Rectangle
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
        public int Width { get { return _width; } set { _width = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        public int Left { get { return X; } }
        public int Top { get { return Y; } }
        public int Bottom { get { return Y + Height; } }
        public int Right { get { return X + Width; } }
        public static Rectangle Empty { get { return new Rectangle(); } }

        public Rectangle(int x, int y, int w, int h)
        {
            _x = x;
            _y = y;
            _width = w;
            _height = h;
        }
    }
}

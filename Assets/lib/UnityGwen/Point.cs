using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{
    public struct Point
    {
        private int _x;
        private int _y;
        public int X { get {return _x;} set{ _x = value;} }
        public int Y { get { return _y; } set { _y = value; } }

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static Point Empty
        {
            get { return new Point(); }
        }

        public static bool operator !=(Point left, Point right)
        {
            return ((left.X != right.X) || (left.Y != right.Y));
        }

        public static bool operator ==(Point left, Point right)
        {
            return ((left.X == right.X) && (left.Y == right.Y));
        }
    }
}

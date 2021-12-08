using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticCurve
{
    class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   x == point.x &&
                   y == point.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return $"x = {this.x} y = {this.y}";
        }


    }
}

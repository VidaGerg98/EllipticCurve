using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticCurve
{
    class ECurve
    {
        public int a;
        public int b;
        public int q;
        public Point nullPoint;
        public Point G;

        public ECurve(int a, int b, int q, Point G)
        {
            this.a = a;
            this.b = b;
            this.q = q;
            this.nullPoint = new Point(0, 0);
            this.G = G;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EllipticCurve
{
    class Signature
    {
        public int r;
        public int s;

        public Signature(int r, int s)
        {
            this.r = r;
            this.s = s;
        }
    }
}

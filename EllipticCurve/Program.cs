using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace EllipticCurve
{
    class Program
    {
        static void Main(string[] args)
        {
            Point P = new Point(3, 1);
            ECurve EC = new ECurve(1, 6, 7, P);
            Random rnd = new Random();
            string m = "Hello!";


            int d = rnd.Next(1, EC.q); //2
            Point B = Multiplication(P, d, EC);
            Console.WriteLine($"d = {d}, B -> {B}");           
            Signature S = GenerateSignature(EC, d, m);
            Console.WriteLine($"Signature -> s = {S.s} r = {S.r}");
            Console.WriteLine($"verified = {VerifySignature(EC, m, B, S)}");
            //Console.WriteLine(Inv(5, EC.q)); 
            //Console.WriteLine(Inverse(new Point(1, 1), EC));
            //Console.WriteLine(Addition(new Point(4, 5), new Point(3, 1), EC));
            //Console.WriteLine(Multiplication(new Point(3, 1), 5, EC));
        }

        static Signature GenerateSignature(ECurve EC, int d, string m)
        {
            //Console.WriteLine("----- Generate Signature -----");
            Random rnd = new Random();
            int k = rnd.Next(1, EC.q); //5
            //Console.WriteLine($"k = {k}");
            Point R = Multiplication(EC.G, k, EC);
            int r = R.x;
            SHA256 sha256Hash = SHA256.Create();
            int h = BitConverter.ToInt32(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(m)));
            //Console.WriteLine($"h = {h}");
            int s = mod((h + (d * r)) * Inv(k, EC.q), EC.q);
            //Console.WriteLine($"inv_k = {Inv(k, EC.q)}");
            //Console.WriteLine($"d*r = {((d * r))}");
            //Console.WriteLine($"h+(d*r) = {(h + (d * r))}");
            //Console.WriteLine($"h+(d*r) * inv_k = {(h + (d * r)) * Inv(k, EC.q)}");
            //Console.WriteLine($"s = {s}, r = {r}");
            Signature S = new Signature(r, s);
            //Console.WriteLine("----- ----- -----");
            return S;
        }

        static bool VerifySignature(ECurve EC, string m, Point B, Signature S)
        {
            //Console.WriteLine("----- Verify Signature -----");
            int w = mod(Inv(S.s, EC.q), EC.q);
            //Console.WriteLine($"s_inv = {Inv(S.s, EC.q)}");
            //Console.WriteLine($"w = {w}");
            SHA1 sha1Hash = SHA1.Create();
            int h = BitConverter.ToInt32(sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(m)));
            //Console.WriteLine($"h = {h}");
            int u1 = mod(w * h, EC.q);
            int u2 = mod(w * S.r, EC.q);
            //Console.WriteLine($"w * h = {w * h}");
            //Console.WriteLine($"u1 = {u1}");
            //Console.WriteLine($"w * S.r = {w * S.r}");
            //Console.WriteLine($"u2 = {u2}");
            Point P = Addition(Multiplication(EC.G, u1, EC), Multiplication(B, u2, EC), EC);
            //Console.WriteLine($"u1 * G = {Multiplication(EC.G, u1, EC)}");
            //Console.WriteLine($"u2 * B = {Multiplication(B, u2, EC)}");
            //Console.WriteLine($"P = {Addition(Multiplication(EC.G, u1, EC), Multiplication(B, u2, EC), EC)}");
            //Console.WriteLine($"S.r % q = {mod(S.r, EC.q)}");
            Console.WriteLine($"P.x = {P.x}");
            //Console.WriteLine("----- ----- -----");
            return P.x == mod(S.r, EC.q);
        }

        static Point Inverse(Point P, ECurve EC)
        {
            int y = -P.y;
            while (y <= 0)
            {
                y = y + EC.q;
            }
            return new Point(P.x, mod(y, EC.q));
        }

        static int Inv(int n, int q)
        {
            int inv = 0;
            for (int i = 0; i < q; i++)
            {
                if (mod(n * i, q) == 1)
                {
                    inv = i;
                    break;
                }
            }

            return inv;
        }

        static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        static Point Addition(Point P, Point Q, ECurve EC)
        {
            if (P.Equals(EC.nullPoint))
            {
                return Q;
            }
            if (Q.Equals(EC.nullPoint))
            {
                return P;
            }
            if (Q.Equals(Inverse(P, EC)))
            {
                return EC.nullPoint;
            }
            if (P.Equals(Inverse(Q, EC)))
            {
                return EC.nullPoint;
            }
            int Rx = 0, Ry = 0;
            int lambda = 0;
            if (!P.Equals(Inverse(Q, EC)))
            {
                if (P.Equals(Q))
                {
                    lambda = (3 * (P.x * P.x) + EC.a) * Inv(2 * P.y, EC.q);
                }
                else
                {
                    lambda = (Q.y - P.y) * Inv((Q.x - P.x), EC.q);
                }
            }
            Rx = (lambda * lambda) - P.x - Q.x;
            Ry = lambda * (P.x - Rx) - P.y;

            Point R = new Point(mod(Rx, EC.q), mod(Ry, EC.q));
            return R;
        }

        static Point Multiplication(Point P, int  n, ECurve EC)
        {
            Point R = EC.nullPoint;
            for (int i = 1; i <= n; i++)
            {
                R = Addition(R, P, EC);
            }

            return R;
        }
    }
}

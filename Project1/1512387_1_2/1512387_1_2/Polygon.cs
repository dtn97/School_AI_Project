using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace _1512387_1_2
{
    class Line
    {
        Int64 a;
        Int64 b;
        Int64 c;

        private Int64 getUCLN(Int64 p1, Int64 p2)
        {
            p1 = Math.Abs(p1);
            p2 = Math.Abs(p2);

            while (p2 != 0)
            {
                Int64 r = p1 % p2;
                p1 = p2;
                p2 = r;
            }
            return p1;
        }

        public Line(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            a = p2.Value - p1.Value;
            b = p1.Key - p2.Key;
            c = p1.Value * p2.Key - p1.Key * p2.Value;

            Int64 ucln = this.getUCLN(a, this.getUCLN(b, c));
            if (ucln != 0)
            {
                a /= ucln;
                b /= ucln;
                c /= ucln;
            }
        }

        public Int64 CalculatePoint(KeyValuePair<int, int> p)
        {
            return a * p.Key + b * p.Value + c;
        }

        public bool LineIntersect(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            if (a == 0 && b != 0)
            {
                double tmp = -1.0 * c / b;
                return ((p1.Value - tmp) * (p2.Value - tmp)) <= 0;
            }
            if (b == 0 && a != 0)
            {
                double tmp = -1.0 * c / a;
                return ((p1.Key - tmp) * (p2.Key - tmp)) <= 0;
            }
            
            return (this.CalculatePoint(p1) * this.CalculatePoint(p2) <= 0);
        }

        public bool LineIntersect1(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            if (a == 0 && b != 0)
            {
                double tmp = -1.0 * c / b;
                return ((p1.Value - tmp) * (p2.Value - tmp)) < 0;
            }
            if (b == 0 && a != 0)
            {
                double tmp = -1.0 * c / a;
                return ((p1.Key - tmp) * (p2.Key - tmp)) < 0;
            }

            return (this.CalculatePoint(p1) * this.CalculatePoint(p2) < 0);
        }

        public double DistancePointToLine(KeyValuePair<int, int> p)
        {
            double x = (double)this.CalculatePoint(p);
            double y = (double)Math.Sqrt(a * a + b * b);
            return x / y;
        }
        ~Line()
        {

        }


    }
    class Polygon
    {
        private List<KeyValuePair<int, int>> arr;
        public Polygon()
        {
            arr = new List<KeyValuePair<int, int>>();
        }

        public Polygon(List<KeyValuePair<int, int>> p)
        {
            arr = new List<KeyValuePair<int, int>>(p);
        }

        public Polygon(Polygon p)
        {
            arr = new List<KeyValuePair<int, int>>(p.arr);
        }

        ~Polygon()
        {

        }

        public void AddVertex(int x, int y)
        {
            arr.Add(new KeyValuePair<int, int>(x, y));
        }

        public bool LineIntersectPolygon(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            int n = arr.Count;
            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = i + 1; j < n; ++j)
                {
                    Line l1 = new Line(p1, p2);
                    Line l2 = new Line(arr[i], arr[j]);
                    if (l1.LineIntersect(arr[i], arr[j]) && l2.LineIntersect(p1, p2))
                        return true;
                }
            }
            return false;
        }
        
        private Int64 TriangleSquare(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2, KeyValuePair<int, int> p3)
        {
            return Math.Abs((Int64)p1.Key * p2.Value - (Int64)p1.Value * p2.Key + (Int64)p2.Key * p3.Value - (Int64)p3.Key * p2.Value + (Int64)p3.Key * p1.Value - (Int64)p1.Key * p3.Value);
        }

        private Int64 PolygonSquare()
        {
            Int64 ret = 0;
            int n = arr.Count;
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;
                ret += ((Int64)arr[i].Key * arr[j].Value - (Int64)arr[j].Key * arr[i].Value);
            }
            return Math.Abs(ret);
        }

        public bool PointInsidePolygon(KeyValuePair<int, int> p)
        {
            Int64 S1 = 0;
            int n = arr.Count;
            for (int i = 0; i < n; ++i)
            {
                int j = (i + 1) % n;
                S1 += this.TriangleSquare(p, arr[i], arr[j]);
            }
            return S1 == this.PolygonSquare();
        }
        public bool PolygonIntersectPolygon(Polygon p)
        {
            int n = arr.Count;
            int m = p.arr.Count;

            for (int i = 0; i < n; ++i)
            {
                int ii = (i + 1) % n;
                Line l1 = new Line(arr[i], arr[ii]);
                for (int j = 0; j < m; ++j)
                {
                    int jj = (j + 1) % m;
                    Line l2 = new Line(p.arr[j], p.arr[jj]);
                    if (l1.LineIntersect(p.arr[j], p.arr[jj]) && l2.LineIntersect(arr[i], arr[ii]))
                        return true;
                }
            }
            return false;
        }

        public double getMinDistanceLineToPolygon(Line l)
        {
            double res = double.MaxValue;
            int n = arr.Count;
            for (int i = 0; i < n; ++i)
            {
                double tmp = l.DistancePointToLine(arr[i]);
                if (Math.Abs(res) > Math.Abs(tmp))
                    res = tmp;
            }

            return res;
        }

        public void MovePolygon(int dx, int dy)
        {
            int n = arr.Count;
            List<KeyValuePair<int, int>> tmp = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < n; ++i)
            {
                int x = arr[i].Key + dx;
                int y = arr[i].Value + dy;
                if (x < 0 || x > 100000 || y < 0 || y > 100000)
                    return;
                tmp.Add(new KeyValuePair<int, int>(x, y));
            }
            arr.Clear();
            foreach (KeyValuePair<int, int> p in tmp)
            {
                arr.Add(new KeyValuePair<int, int>(p.Key, p.Value));
            }
        }

        public List<KeyValuePair<int, int>> getPoints()
        {
            return new List<KeyValuePair<int, int>>(arr);
        }

        public void DrawPolygon(Graphics gp)
        {
            int n = arr.Count;
            Point[] p = new Point[n];
            for (int i = 0; i < n; ++i)
            {
                p[i] = new Point(arr[i].Key, arr[i].Value);
            }
            gp.FillPolygon(new SolidBrush(Color.Green), p);
        }
    }
}

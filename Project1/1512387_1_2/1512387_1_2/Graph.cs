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
    public class Vertex : IComparable<Vertex>
    {
        public int v;
        public double f;
        public Vertex(int _v, double _f)
        {
            v = _v;
            f = _f;
        }
        public Vertex(Vertex p)
        {
            v = p.v;
            f = p.f;
        }
        public int CompareTo(Vertex other)
        {
            return (this.f).CompareTo(other.f);
        }
    }
    class Graph
    {
        private KeyValuePair<int, int> source;
        private KeyValuePair<int, int> goal;
        private List<Polygon> arrPolygons;
        private HashSet<KeyValuePair<int, int>> arrVertexs;
        int[] trace;
        private List<KeyValuePair<int, int>> path;
        public Size size;
        double cost;

        public Graph()
        {
            source = new KeyValuePair<int, int>();
            goal = new KeyValuePair<int, int>();
            arrPolygons = new List<Polygon>();
            path = new List<KeyValuePair<int, int>>();
            size = new Size(0, 0);
            cost = 0;
        }

        public Graph(KeyValuePair<int, int> s, KeyValuePair<int, int> g, List<Polygon> arr)
        {
            source = new KeyValuePair<int, int>(s.Key, s.Value);
            goal = new KeyValuePair<int, int>(g.Key, g.Value);
            arrPolygons = new List<Polygon>(arr);
        }

        ~Graph()
        {

        }

        private double getPointDistance(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            int dx = p1.Key - p2.Key;
            int dy = p1.Value - p2.Value;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        bool PointInsidePolygons(KeyValuePair<int, int> p)
        {
            foreach (Polygon pol in arrPolygons)
            {
                if (pol.PointInsidePolygon(p) == true)
                    return true;
            }
            return false;
        }

        void getCandidateVertex()
        {
            arrVertexs = new HashSet<KeyValuePair<int, int>>();
            foreach(Polygon pol in arrPolygons)
            {
                List<KeyValuePair<int, int>> arrPoints = pol.getPoints();
                foreach (KeyValuePair<int, int> p in arrPoints)
                {
                    for (int i = -1; i <= 1; ++i)
                    {
                        for (int j = -1; j <= 1; ++j)
                        {
                            if (i == 0 && j == 0)
                                continue;
                            int x = p.Key + i;
                            int y = p.Value + j;
                            if (x < 0 || x > 100000 || y < 0 || y > 100000)
                                continue;
                            KeyValuePair<int, int> tmp = new KeyValuePair<int, int>(x, y);
                            if (arrVertexs.Contains(tmp))
                                continue;
                            if (this.PointInsidePolygons(tmp) == false)
                            {
                                arrVertexs.Add(tmp);
                            }
                        }
                    }
                }
            }
            arrVertexs.Add(source);
            arrVertexs.Add(goal);
        }

        private bool LinePointInt(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            bool check1 = false;
            bool check2 = false;
            Line l = new Line(p1, p2);
            double d = Math.Sqrt((double)2) / 2.0;
            foreach(Polygon pol in arrPolygons)
            {
                if (pol.LineIntersectPolygon(p1, p2))
                    return false;
                double tmp = pol.getMinDistanceLineToPolygon(l);
                if (Math.Abs(tmp) < d)
                {
                    if (tmp > 0)
                        check1 = true;
                    else
                        check2 = true;
                }
                if (check1 && check2)
                    return false;
            }
            return true;
        }

        private double getWeight(KeyValuePair<int, int> p1, KeyValuePair<int, int> p2)
        {
            if (this.LinePointInt(p1, p2))
                return this.getPointDistance(p1, p2);
            return 0;
        }

        private double[,] BuildGraph()
        {
            this.getCandidateVertex();
            int n = arrVertexs.Count;
            double[,] res = new double[n, n];

            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = i + 1; j < n; ++j)
                {
                    res[i, j] = res[j, i] = this.getWeight(arrVertexs.ElementAt(i), arrVertexs.ElementAt(j));
                }
            }
            return res;
        }

        private double[] getHeuristic()
        {
            int n = arrVertexs.Count;
            double[] res = new double[n];

            for (int i = 0; i < n; ++i)
            {
                res[i] = this.getPointDistance(goal, arrVertexs.ElementAt(i));
            }

            return res;
        }

        public void MovePolygon()
        {
            Random getRandom = new Random();
            int n = arrPolygons.Count;
            for (int i = 0; i < n; ++i)
            {
                int dx = getRandom.Next(-30, 30);
                int dy = getRandom.Next(-30, 30);
                arrPolygons[i].MovePolygon(dx, dy);
                if (arrPolygons[i].PointInsidePolygon(source) || arrPolygons[i].PointInsidePolygon(goal))
                    arrPolygons[i].MovePolygon(-dx, -dy);
                for (int j = 0; j < n; ++j)
                {
                    if (i == j)
                        continue;
                    if (arrPolygons[i].PolygonIntersectPolygon(arrPolygons[j]))
                        arrPolygons[i].MovePolygon(-dx, -dy);
                }
                
            }
        }
        public void Astar()
        {
            
            double[,] a = this.BuildGraph();
            int n = arrVertexs.Count;
            trace = new int[n];
            for (int i = 0; i < n; ++i)
                trace[i] = -1;
            bool oke = false;
            for (int i = 0; i < n; ++i)
            {
                if (a[n - 1, i] > 0)
                {
                    oke = true;
                    break;
                }
            }
            if (!oke)
                return;
            oke = false;
            for (int i = 0; i < n; ++i)
            {
                if (a[n - 2, i] > 0)
                {
                    oke = true;
                    break;
                }
            }
            if (!oke)
                return;
            double[] heuristic = this.getHeuristic();
            double[] d = new double[n];
            for (int i = 0; i < n; ++i)
                d[i] = double.MaxValue;
            trace[n - 2] = n - 2;
            d[n - 2] = 0;

            SortedSet<Vertex> arr = new SortedSet<Vertex>();
            arr.Add(new Vertex(n - 2, heuristic[n - 2]));

            while (arr.Count > 0)
            {
                Vertex tmp = arr.First();
                arr.Remove(tmp);
                int u = tmp.v;
                double w = tmp.f - heuristic[u];
                if (Math.Abs(w - d[u]) > 0.0001)
                    continue;
                if (u == n - 1)
                    return;
                for (int v = 0; v < n; ++v)
                {
                    if (a[u, v] > 0 && d[v] > d[u] + a[u, v])
                    {
                        d[v] = d[u] + a[u, v];
                        arr.Add(new Vertex(v, d[v] + heuristic[v]));
                        trace[v] = u;
                    }
                }
            }
        }

        public bool finish()
        {
            return (source.Key == goal.Key && source.Value == goal.Value);
        }

        private Point[] getPath()
        {
            int n = arrVertexs.Count;
            if (trace[n - 1] == -1)
            {
                return null;
            }

            int s = n - 2;
            int g = s + 1;

            Stack<int> tmp = new Stack<int>();
            while (s != g)
            {
                tmp.Push(g);
                g = trace[g];
            }
            tmp.Push(g);

            n = tmp.Count;
            Point[] res = new Point[n];
            int i = 0;
            int j = 0;
            while (tmp.Count > 0)
            {
                j = tmp.Peek();
                tmp.Pop();
                res[i++] = new Point(arrVertexs.ElementAt(j).Key, arrVertexs.ElementAt(j).Value);
            }
            return res;
        }

        public void DrawMap(Graphics gp)
        {
            gp.FillEllipse(new SolidBrush(Color.Blue), goal.Key - 3, goal.Value - 3, 6, 6);
            gp.FillEllipse(new SolidBrush(Color.Blue), source.Key - 3, source.Value - 3, 6, 6);
            foreach (Polygon pol in arrPolygons)
            {
                pol.DrawPolygon(gp);
            }
        }

        public void DrawPath(Graphics gp)
        {
            Point[] points = this.getPath();
            if (points != null)
            {
                int n = points.Length;
                for (int i = 0; i < n - 1; ++i)
                {
                    midpointline(points[i], points[i + 1], gp);
                }
                if (n >= 1)
                {
                    int dx = Math.Abs(source.Key - points[1].X);
                    int dy = Math.Abs(source.Value - points[1].Y);
                    int mx = Math.Max(dx, dy);
                    int mn = Math.Min(dx, dy);
                    cost += mn * Math.Sqrt(2.0) + mx - mn;
                    source = new KeyValuePair<int, int>(points[1].X, points[1].Y);
                    
                }
            }
        }

        public void swap(ref KeyValuePair<int, int> p1, ref KeyValuePair<int, int> p2)
        {
            KeyValuePair<int, int> tmp = p1;
            p1 = p2;
            p2 = tmp;
        }

        public void SetInput(string fileName)
        {
            string text;
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            string[] buffer = text.Split(new string[] { " ", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries);

            int cnt = 1;
            int n = int.Parse(buffer[0]);
            int x = 0;
            int y = 0;
            int m = 0;
            for (int i = 0; i < n; ++i)
            {
                m = int.Parse(buffer[cnt++]);
                Polygon p = new Polygon();
                for (int j = 0; j < m; ++j)
                {
                    x = int.Parse(buffer[cnt++]);
                    y = int.Parse(buffer[cnt++]);
                    p.AddVertex(x, y);
                    if (x > size.Width)
                        size.Width = x;
                    if (y > size.Height)
                        size.Height = y;
                }
                arrPolygons.Add(p);
            }
            x = int.Parse(buffer[cnt++]);
            y = int.Parse(buffer[cnt++]);
            if (x > size.Width)
                size.Width = x;
            if (y > size.Height)
                size.Height = y;
            source = new KeyValuePair<int, int>(x, y);
            x = int.Parse(buffer[cnt++]);
            y = int.Parse(buffer[cnt++]);
            goal = new KeyValuePair<int, int>(x, y);
            if (x > size.Width)
                size.Width = x;
            if (y > size.Height)
                size.Height = y;
            size.Height += 50;
            size.Width += 50;
        }

        public void SaveOutput()
        {
            using (StreamWriter writer = new StreamWriter("output.txt"))
            {
                Point[] points = this.getPath();
                if (points == null)
                {
                    writer.WriteLine("NO\n{0}", 0.0);
                    return;
                }
                double res = 0;
                int n = points.Length;
                for (int i = 0; i < n - 1; ++i)
                {
                    int dx = Math.Abs(points[i].X - points[i + 1].X);
                    int dy = Math.Abs(points[i].Y - points[i + 1].Y);
                    if (dx < dy)
                    {
                        swap(ref dx, ref dy);
                    }
                    res += dy * Math.Sqrt(2.0) + dx - dy;
                }
                writer.WriteLine("YES\n{0}", res);
            }
        }

        public void SaveOutputMoving()
        {
            using (StreamWriter writer = new StreamWriter("outputMoving.txt"))
            {
                writer.WriteLine("YES\n{0}", cost);
            }
        }

        private void swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
        public void midpointline(Point p1, Point p2, Graphics gp)
        {
            int x1 = p1.X, y1 = p1.Y;
            int x2 = p2.X, y2 = p2.Y;
            int dx, dy, d, incry, incre, incrne, slopegt1 = 0;
            dx = Math.Abs(x1 - x2); dy = Math.Abs(y1 - y2);
            if (dy > dx)
            {
                swap(ref x1, ref y1);
                swap(ref x2, ref y2);
                swap(ref dx, ref dy);
                slopegt1 = 1;
            }
            if (x1 > x2)
            {
                swap(ref x1, ref x2);
                swap(ref y1, ref y2);
            }
            if (y1 > y2)
                incry = -1;
            else
                incry = 1;
            d = 2 * dy - dx;
            incre = 2 * dy;
            incrne = 2 * (dy - dx);
            while (x1 < x2)
            {
                int xnew = x1, ynew = y1;
                if (d <= 0)
                    d += incre;
                else
                {
                    d += incrne;
                    ynew += incry;
                }
                xnew++;
                if (slopegt1 > 0)
                {
                    Pen p = new Pen(Color.Red, 1);
                    gp.DrawLine(p, new Point(y1, x1), new Point(ynew, xnew));
                    x1 = xnew; y1 = ynew;
                }
                else
                {
                    Pen p = new Pen(Color.Red, 1);
                    gp.DrawLine(p, new Point(x1, y1), new Point(xnew, ynew));
                    x1 = xnew; y1 = ynew;
                }
            }
        }
    }
}

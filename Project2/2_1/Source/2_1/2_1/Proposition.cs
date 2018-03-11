using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1
{
    public class MyCompare_1 : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return a.CompareTo(b);
        }
    }
    public class Proposition
    {
        private SortedSet<string> arrLiteral1;
        private SortedSet<string> arrLiteral2;
        public Proposition()
        {
            arrLiteral1 = new SortedSet<string>(new MyCompare_1());
            arrLiteral2 = new SortedSet<string>(new MyCompare_1());
        }
        public Proposition(string str)
        {
            arrLiteral1 = new SortedSet<string>(new MyCompare_1());
            arrLiteral2 = new SortedSet<string>(new MyCompare_1());
            string[] arr = str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in arr)
            {
                if (i[0].Equals('~') == true)
                {
                    string tmp = i;
                    tmp = tmp.Remove(0, 1);
                    arrLiteral2.Add(tmp);
                }
                else
                {
                    arrLiteral1.Add(i);
                }
            }
        }

        public Proposition(SortedSet<string> arr1, SortedSet<string> arr2)
        {
            arrLiteral1 = new SortedSet<string>(arr1, new MyCompare_1());
            arrLiteral2 = new SortedSet<string>(arr2, new MyCompare_1());
        }

        public Proposition(Proposition p)
        {
            arrLiteral1 = new SortedSet<string>(p.arrLiteral1, new MyCompare_1());
            arrLiteral2 = new SortedSet<string>(p.arrLiteral2, new MyCompare_1());
        }
        ~Proposition() { }

        public void Set(string str)
        {
            string[] arr = str.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string i in arr)
            {
                if (i[0].Equals('~') == true)
                {
                    string tmp = i;
                    tmp = tmp.Remove(0, 1);
                    arrLiteral1.Add(tmp);
                }
                else
                {
                    arrLiteral2.Add(i);
                }
            }
        }

        public void Add(string s)
        {
            if (s[0].Equals('~') == true)
            {
                s.Remove(0, 1);
                arrLiteral2.Add(s);
            }
            else
            {
                arrLiteral1.Add(s);
            }
        }

        public void Remove(string s, int type)
        {
            if (type == 1)
            {
                arrLiteral1.Remove(s);
            }
            else
            {
                arrLiteral2.Remove(s);
            }
        }

        public string ToString()
        {
            string res = "";
            foreach(string i in arrLiteral1)
            {
                res += i;
                res += "|";
            }
            foreach(string i in arrLiteral2)
            {
                res += "~";
                res += i;
                res += "|";
            }
            if (res.Length >= 1)
                res = res.Remove(res.Length - 1, 1);
            return res;
        }
        
        public bool IsEmpty()
        {
            return (arrLiteral1.Count == 0) && (arrLiteral2.Count == 0);
        }

        public bool IsTrue()
        {
            foreach(string i in arrLiteral1)
            {
                foreach(string j in arrLiteral2)
                {
                    if (i.Equals(j) == true)
                        return true;
                }
            }
            return false;
        }

        public int Compare(Proposition p)
        {
            int n1 = arrLiteral1.Count + arrLiteral2.Count;
            int n2 = p.arrLiteral1.Count + p.arrLiteral2.Count;
            if (n1 > n2)
                return 1;
            if (n1 < n2)
                return -1;
            string tmp1 = this.ToString();
            string tmp2 = p.ToString();
            return tmp1.CompareTo(tmp2);
        }

        public bool Equals(Proposition p)
        {
            return (arrLiteral1.Equals(p.arrLiteral1) && arrLiteral2.Equals(p.arrLiteral2));
        }

        public HashSet<Proposition> Process(Proposition p)
        {
            HashSet<Proposition> res = new HashSet<Proposition>();
            SortedSet<string> tmp1 = new SortedSet<string>(arrLiteral1, new MyCompare_1());
            foreach(string i in p.arrLiteral1)
                    tmp1.Add(i);
            SortedSet<string> tmp2 = new SortedSet<string>(arrLiteral2, new MyCompare_1());
            foreach (string i in p.arrLiteral2)
                tmp2.Add(i);

            foreach (string i in arrLiteral1)
            {
                foreach(string j in p.arrLiteral2)
                {
                    if (i.Equals(j) == true)
                    {
                        Proposition tmp = new Proposition(tmp1, tmp2);
                        tmp.Remove(i, 1);
                        tmp.Remove(j, 2);
                        res.Add(tmp);
                    }
                }
            }

            foreach (string i in p.arrLiteral1)
            {
                foreach (string j in arrLiteral2)
                {
                    if (i.Equals(j) == true)
                    {
                        Proposition tmp = new Proposition(tmp1, tmp2);
                        tmp.Remove(i, 1);
                        tmp.Remove(j, 2);
                        res.Add(tmp);
                    }
                }
            }

            return res;
        }
    }
}

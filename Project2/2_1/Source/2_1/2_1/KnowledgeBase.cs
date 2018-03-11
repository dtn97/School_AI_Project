using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1
{
    public class MyCompare_2 : IComparer<Proposition>
    {
        public int Compare(Proposition a, Proposition b)
        {
            return a.Compare(b);
        }
    }

    public class KnowledgeBase
    {
        private SortedSet<Proposition> arrProposition;
        List<string> arrString;
        public KnowledgeBase()
        {
            arrProposition = new SortedSet<Proposition>(new MyCompare_2());
            arrString = new List<string>();
        }
        public KnowledgeBase(string fileName)
        {
            arrProposition = new SortedSet<Proposition>(new MyCompare_2());
            arrString = new List<string>();
            string[] text = System.IO.File.ReadAllLines(fileName);
            int n = text.Length;
            string s = "";
            for (int i = 1; i < n - 2; ++i)
            {
                arrProposition.Add(new Proposition(text[i]));
                s += (text[i] + ",");
            }
            arrString.Add(text[n - 1]);
            Proposition tmp = new Proposition();
            tmp.Set(text[n - 1]);
            s += tmp.ToString();
            arrProposition.Add(tmp);
            arrString.Add(s);
        }
        public KnowledgeBase(KnowledgeBase p)
        {
            arrProposition = new SortedSet<Proposition>(p.arrProposition, new MyCompare_2());
        }
        ~KnowledgeBase() { }

        public string ToString()
        {
            string res = "";
            foreach(Proposition i in arrProposition)
            {
                res += (i.ToString() + ",");
            }
            res = res.Remove(res.Length - 1, 1);
            return res;
        }

        public void write(string fileName)
        {
            System.IO.File.WriteAllLines(fileName, arrString.ToArray());
        }

        public bool Union(HashSet<Proposition> p)
        {
            int n = arrProposition.Count;
            foreach(Proposition i in p)
            {
                arrProposition.Add(i);
            }
            return (arrProposition.Count == n && p.Count != 0);
        }

        public void Nomalize(ref HashSet<Proposition> p)
        {
            for (int i = 0; i < p.Count; ++i)
            {
                if (p.ElementAt(i).IsTrue() == true)
                {
                    p.Remove(p.ElementAt(i));
                    --i;
                }
            }
        }

        public void Process()
        {
            while (true)
            {
                int n = arrProposition.Count;
                HashSet<Proposition> tmp = new HashSet<Proposition>();
                for (int i = 0; i < n; ++i)
                {
                    for (int j = i + 1; j < n; ++j)
                    {
                        HashSet<Proposition> arr = (arrProposition.ElementAt(i).Process(arrProposition.ElementAt(j)));
                        foreach(Proposition pro in arr)
                        {
                            tmp.Add(pro);
                        }
                    }
                }
                this.Nomalize(ref tmp);
                if (this.Union(tmp) == true)
                {
                    arrString.Add(this.ToString());
                    arrString.Add("False");
                    return;
                }
                foreach (Proposition p in arrProposition)
                {
                    if (p.IsEmpty() == true)
                    {
                        arrString.Add("True");
                        return;
                    }
                }
                arrString.Add(this.ToString());
            }
        }
    }
}

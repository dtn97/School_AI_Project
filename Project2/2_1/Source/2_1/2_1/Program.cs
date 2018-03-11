using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_1
{
    class Program
    {
        static void Main(string[] args)
        {
            KnowledgeBase kb = new KnowledgeBase("input.txt");
            kb.Process();
            kb.write("1512387_1512491.txt");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kotitehtavat
{
    class kt5
    {
        public Stack<int> numerot = new Stack<int>();
        public void A()
        {
            numerot.Push(0);
            numerot.Push(1);
            for(int i = 0; i<7; i++)
            {
                int newValue = numerot.ElementAt(0) + numerot.ElementAt(1);
                numerot.Push(newValue);
            }
            Console.Write(numerot.Peek());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS1_l4_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(HelloWorld));
            t.Start();
            Console.ReadLine();
        }
        static void HelloWorld()
        {
            Console.WriteLine("Hello world");
        }
    }
}

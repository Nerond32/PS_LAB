using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

namespace PS1_l4_2
{
    class Program
    {
        private static readonly int threadAmount = 10;
        private static Thread[] t = new Thread[threadAmount];
        private static Semaphore s = new Semaphore(3, 3);
        static void Main(string[] args)
        {
            String x = null;
            for (int i = 0; i < threadAmount; i++)
            {
                int z = i;
                t[i] = new Thread(() => AlphabetWriter(z));
                t[i].Start();
            }
            while (x != "end")
            {
                x = Console.ReadLine();
            }
        }
        static void AlphabetWriter(int number)
        {
            s.WaitOne();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if (number + 1 > 9)
                {
                    number = -1;
                }
                Console.WriteLine("{0}{1}", letter, number + 1);
                System.Threading.Thread.Sleep(100);
            }
            s.Release();
        }
    }
}

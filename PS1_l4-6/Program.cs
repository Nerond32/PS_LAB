using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS1_l4_6
{
    class Program
    {
        private static readonly int philosopherAmount = 5;
        private static bool[] forkIsUsed = new bool[philosopherAmount];
        private static Thread[] philosopherThread = new Thread[philosopherAmount];
        private static object o = new object();
        //This delegate can be used to point to methods
        //which return void and take a string.
        public delegate void MyEventHandler();

        //This event can cause any method which conforms
        //to MyEventHandler to be called.
        public event MyEventHandler SomethingHappened;
        void HandleSomethingHappened()
        {
            //Do some stuff
        }
        static void Main(string[] args)
        {
            for (int i = 0; i < philosopherAmount; i++)
            {
                int z = i;
                forkIsUsed[z] = false;
                philosopherThread[i] = new Thread(() => Philosophing(z));
                philosopherThread[i].Start();
            }
        }
        static void Philosophing(int fork1Number)
        {
            int fork2Number = fork1Number + 1;
            if (fork2Number == philosopherAmount)
            {
                fork2Number = 0;
            }
            while (true)
            {
                bool nSHouldWait;
                lock (o)
                {
                    if(forkIsUsed[fork1Number] == false && forkIsUsed[fork2Number])
                    {
                        forkIsUsed[fork1Number] = true;
                        forkIsUsed[fork2Number] = true;
                        nSHouldWait = false;
                    }
                    else
                    {
                        nSHouldWait = true;
                    }
                }
                if (nSHouldWait)
                {
                    //WaitForSingleObject(hndEvents[1], INFINITE);
                }
            }
        }
    }
}

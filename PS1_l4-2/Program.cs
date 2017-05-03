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
        static void Main(string[] args)
        {
            String x = null;
            for(int i = 0; i < threadAmount; i++)
            {
                int z = i;
                t[i] = new Thread(() => AlphabetWriter(z));
            }
            while (x != "end")
            {
                x = Console.ReadLine();
                try
                {
                    InterpretCommand(x);
                }
                catch(ThreadStateException e)
                {
                    Console.WriteLine("Thread is already running!");
                }
                catch(Exception e)
                {
                    if (x != "end")
                    {
                        Console.WriteLine("Invalid command");
                    }
                }
            }
        }
        static void AlphabetWriter(int number)
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if(number+1 > 9)
                {
                    number = -1;
                }
                Console.WriteLine("{0}{1}", letter, number+1);
                System.Threading.Thread.Sleep(1000);
            }
        }
        static void InterpretCommand(String command)
        {
            String[] splittedCommand = command.Split(null);
            List<int> thrNumber = RecognizeNumbers(splittedCommand[1]);
            if (splittedCommand[0] == "stop")
            {
                if (thrNumber.Count == 1)
                {
                    SuspendThread(thrNumber[0]);
                }
                else if (thrNumber.Count == 2)
                {
                    for (int i = thrNumber[0]; i <= thrNumber[1]; i++)
                    {
                        SuspendThread(i);
                    }
                }
            }
            else if (splittedCommand[0] == "start")
            {
                if (thrNumber.Count == 1)
                {
                    StartOrResumeThread(thrNumber[0]);
                }
                else if (thrNumber.Count == 2)
                {
                    
                    for (int i = thrNumber[0]; i <= thrNumber[1]; i++)
                    {
                        StartOrResumeThread(i);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
        static void SuspendThread(int nr)
        {
            nr--;
            try
            {
                if (t[nr].ThreadState != ThreadState.Suspended)
                {
                    t[nr].Suspend();
                }
                else
                {
                    throw new ThreadStateException();
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Thread with number {0} does not exist", nr+1);
            }
            catch (ThreadStateException e)
            {
                Console.WriteLine("Thread {0} is not running, can't suspend", nr+1);
            }
        }
        static void StartOrResumeThread(int nr)
        {
            nr--;
            try
            {
                if (t[nr].ThreadState == ThreadState.Suspended)
                {
                    t[nr].Resume();
                }
                else if(t[nr].ThreadState == ThreadState.Unstarted)
                {
                    t[nr] = new Thread(() => AlphabetWriter(nr));
                    t[nr].Start();
                }
                else
                {
                    Console.WriteLine("Thread {0} is already running", nr+1);
                }
            }
            catch(IndexOutOfRangeException e)
            {
                Console.WriteLine("{0} is invalid number of a thread", nr+1);
            }
        }
        static List<int> RecognizeNumbers(String numbers)
        {
            List<int> intList = new List<int>();
            int x;
            if (int.TryParse(numbers, out x))
            {
                intList.Add(x);
            }
            else
            {
                String[] s = numbers.Split('-');
                intList.Add(Int32.Parse(s[0]));
                intList.Add(Int32.Parse(s[1]));
            }
            return intList;
        }
    }
}

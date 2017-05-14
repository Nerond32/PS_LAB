using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS1_l5
{
    class Program
    {
        private static readonly int connectionLimit = 3;
        private static int current = 0;
        private static Semaphore s = new Semaphore(connectionLimit, connectionLimit);
        private static Thread t;
        public static Socket InitServer()
        {
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Enter server port(default 7): ");
            int sPort = 7;
            try
            {
                sPort = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Using default port value");
            }
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAd, sPort);
            s.Bind(localEndPoint);
            return s;
        }
        public static void Main()
        {
            Socket serverSocket = InitServer();
            Console.WriteLine("Starting server at: " + serverSocket.LocalEndPoint);
            serverSocket.Listen(10);

            bool stopServer = false;
            while (!stopServer)
            {
                Console.WriteLine("Waiting for new connection.");
                Socket clientSocket = serverSocket.Accept();
                t = new Thread(() => ListenThread(clientSocket));
                t.Start();
            }
            serverSocket.Close();
            Console.ReadLine();
        }
        public static void ListenThread(Socket clientSocket)
        {
            if(current >= connectionLimit)
            {
                String msg = "Refused by server";
                Console.WriteLine("Refusing new connection");
                clientSocket.Send(Encoding.UTF8.GetBytes(msg));
                clientSocket.Close();
                return;
            }
            s.WaitOne();
            current++;
            Console.WriteLine("Connection accepted from " + clientSocket.RemoteEndPoint.ToString());
            try
            {
                while (!(clientSocket.Poll(1, SelectMode.SelectRead) && clientSocket.Available == 0))
                {
                    byte[] fromServer = new byte[clientSocket.ReceiveBufferSize];
                    int length = clientSocket.Receive(fromServer);
                    String ts = Encoding.UTF8.GetString(fromServer).Substring(0, length);
                    Console.WriteLine("Message received from {0}: {1}", clientSocket.RemoteEndPoint.ToString(), ts);
                    clientSocket.Send(Encoding.UTF8.GetBytes(ts));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Lost connection");
            }
            Console.WriteLine("Disconnected");
            clientSocket.Close();
            current--;
            s.Release();
        }
    }
}

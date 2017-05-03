using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PS1_l5
{
    class Program
    {
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
            while (true)
            {
                Console.WriteLine("Waiting for new connection.");
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("Connection accepted from " + clientSocket.RemoteEndPoint.ToString());
                byte[] fromServer = new byte[clientSocket.ReceiveBufferSize];
                int length = clientSocket.Receive(fromServer);
                String ts = Encoding.UTF8.GetString(fromServer).Substring(0, length);
                Console.WriteLine("Message received from client: " + ts);
                clientSocket.Send(Encoding.UTF8.GetBytes(ts));
                clientSocket.Close();
            }
            serverSocket.Close();
            Console.ReadLine();
        }
    }
}

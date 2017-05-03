using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace PS1_l2
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = null;
            try
            {
                s = InitConnectionSocket();
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to connect to a server");
            }
            if (s != null)
            {
                WorkWithServer(s);
                s.Close();
            }
            Console.ReadLine();
        }
        
        public static Socket InitConnectionSocket()
        {
            Console.WriteLine("Enter server address: ");
            String sAddress = Console.ReadLine();
            Console.WriteLine("Enter port: ");
            int sPort = Convert.ToInt32(Console.ReadLine());
            IPAddress ipAddress = null;
            foreach (IPAddress tmpIPAddress in Dns.GetHostEntry(sAddress).AddressList)
            {
                if (tmpIPAddress.AddressFamily == AddressFamily.InterNetwork) // filter out ipv4
                {
                    ipAddress = tmpIPAddress;
                }
            }
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipAddress, sPort);
            Console.WriteLine("Connection succesful");
            return s;
        }

        public static void WorkWithServer(Socket s)
        {
            bool condition = false;
            Console.WriteLine("Enter message to server: ");
            String ts = Console.ReadLine();
            do
            {
                byte[] toServer = Encoding.UTF8.GetBytes(ts);
                s.Send(toServer);
                Console.WriteLine("Message sent to server: " + ts);
                byte[] fromServer = new byte[s.ReceiveBufferSize];
                int length = s.Receive(fromServer);
                String fs = Encoding.UTF8.GetString(fromServer);
                if (!String.IsNullOrEmpty(fs))
                {
                    condition = !condition;
                    Console.WriteLine("Message received from server: " + fs.Substring(0, length));
                }
            } while (!condition);
        }
        /*public static TcpClient InitConnection()
        {
            Console.WriteLine("Enter server address: ");
            String sAddress = Console.ReadLine();
            Console.WriteLine("Enter port: ");
            int sPort = Convert.ToInt32(Console.ReadLine());
            IPAddress ipAddress = null;
            foreach (IPAddress tmpIPAddress in Dns.GetHostEntry(sAddress).AddressList)
            {
                if (tmpIPAddress.AddressFamily == AddressFamily.InterNetwork) // filter out ipv4
                {
                    ipAddress = tmpIPAddress;
                }
            }
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, sPort);
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ipEndPoint);
            Console.WriteLine("Connection succesful");
            return tcpClient;
        }*/
        /*public static void WorkWithServer(TcpClient tcpClient)
        {
            bool condition = false;
            String ts = Console.ReadLine();
            NetworkStream netStream = tcpClient.GetStream();
            do
            {
                if (netStream.CanWrite)
                {
                    byte[] toServer = Encoding.UTF8.GetBytes(ts);
                    netStream.Write(toServer, 0, toServer.Length);
                    Console.WriteLine("Message sent to server: " + ts);
                }
                if (netStream.CanRead)
                {
                    byte[] fromServer = new byte[tcpClient.ReceiveBufferSize];
                    netStream.Read(fromServer, 0, (int)tcpClient.ReceiveBufferSize);
                    String fs = Encoding.UTF8.GetString(fromServer);
                    if (!String.IsNullOrEmpty(fs))
                    {
                        condition = !condition;
                        Console.WriteLine("Message received from server: " + fs);
                    }
                }
            } while (!condition);
        }*/
    }
}
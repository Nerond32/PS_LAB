using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PS1_l6S
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "";
            UdpClient server = new UdpClient(3301);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server");
            while (true)
            {
                byte[] receivedBytes = server.Receive(ref remoteIPEndPoint);
                data = Encoding.ASCII.GetString(receivedBytes);
                Console.WriteLine("Message Received " + data.TrimEnd());
                server.Send(receivedBytes, receivedBytes.Length, remoteIPEndPoint);
                Console.WriteLine("Message Sent: " + data);
            }
            Console.ReadLine();
            server.Close(); 
        }
    }
}

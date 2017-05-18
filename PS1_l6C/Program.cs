using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PS1_l6C
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "";
            byte[] sendBytes = new Byte[1024];
            byte[] rcvPacket = new Byte[1024];
            UdpClient client = new UdpClient();
            IPAddress address = IPAddress.Parse("127.0.0.1");
            client.Connect(address, 3301);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Client");
            while (true)
            {
                data = Console.ReadLine();
                Console.WriteLine("Message Sent: " + data);
                sendBytes = Encoding.ASCII.GetBytes(data);
                client.Send(sendBytes, sendBytes.GetLength(0));
                rcvPacket = client.Receive(ref remoteIPEndPoint);
                string rcvData = Encoding.ASCII.GetString(rcvPacket);
                String s = Encoding.ASCII.GetString(rcvPacket, 0, rcvPacket.Length);
                Console.WriteLine("Message Received: " + s);
            }
            Console.ReadLine();
            client.Close(); 
        }
    }
}

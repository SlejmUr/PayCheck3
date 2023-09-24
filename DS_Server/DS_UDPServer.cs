using NetCoreServer;
using System.Net.Sockets;
using System.Net;
using PayCheckServerLib;

namespace DS_Server
{
    public class DS_UDPServer : UdpServer
    {
        public DS_UDPServer(string address, int port) : base(address, port) 
        {
        }
        protected override void OnStarted()
        {
            Console.WriteLine("DS_UDPServer started");
            ReceiveAsync();
        } 

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (!Directory.Exists("UDP_DS")) { Directory.CreateDirectory("UDP_DS"); }
            Console.WriteLine("UDP_DS Recieved: " + BitConverter.ToString(buffer, (int)offset, (int)size));
            File.WriteAllBytes("UDP_DS/" + Port + "_" + DateTime.Now.ToString("s").Replace(":", "-") + ".bytes", buffer[..(int)size]);
            Console.WriteLine(size);
            Console.WriteLine(BitConverter.ToInt32(buffer, 14));
            Console.WriteLine(BitConverter.ToInt32(buffer, 18));
            if (size == 22)
            {
                if (BitConverter.ToInt32(buffer, 14) == -1431655766 && BitConverter.ToInt32(buffer, 18) == -1145324613)
                {

                    var sent = SendAsync(endpoint, buffer, offset, size);
                    Console.WriteLine("Is sent back? " + sent);
                    //dont know why but it doesnt happy with this debugger print
                    Debugger.PrintInfo("Recieved ping ,sending back: "+ sent);
                }
            }

            /*
            if (IsEchoServer)
                SendAsync(endpoint, buffer, 0, size);*/
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"UDP server caught an error with code {error}");
        }
    }
}

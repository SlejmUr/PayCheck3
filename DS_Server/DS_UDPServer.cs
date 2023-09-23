using NetCoreServer;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace DS_Server
{
    public class DS_UDPServer : UdpServer
    {
        public DS_UDPServer(string address, int port) : base(address, port) { }
        protected override void OnStarted() => ReceiveAsync();

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (!Directory.Exists("UDP_DS")) { Directory.CreateDirectory("UDP_DS"); }
            Console.WriteLine("UDP_DS Recieved: " + BitConverter.ToString(buffer, (int)offset, (int)size));
            File.WriteAllBytes("UDP_DS/" + Port + "_" + DateTime.Now.ToString("s").Replace(":", "-") + ".bytes", buffer[..(int)size]);
            Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size));

            // Echo the message back to the sender
            //SendAsync(endpoint, buffer, 0, size);
        }

        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"UDP server caught an error with code {error}");
        }
    }
}

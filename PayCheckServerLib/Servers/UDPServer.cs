using NetCoreServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PayCheckServerLib
{
    public class PD3UDPServer : UdpServer
    {
        public PD3UDPServer(string address, int port) : base(address, port) { }
        protected override void OnStarted() => ReceiveAsync();

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (!Directory.Exists("UDP")) { Directory.CreateDirectory("UDP"); }
            Console.WriteLine("UDP Recieved: " + BitConverter.ToString(buffer, (int)offset, (int)size));
            File.WriteAllBytes("UDP/" + DateTime.Now.ToString("s").Replace(":", "-") + ".bytes", buffer[..(int)size]);
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

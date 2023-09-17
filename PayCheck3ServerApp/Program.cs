using PayCheckServerLib;

namespace PayCheck3ServerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server(s)!");
            ServerManager.Start();
            Console.ReadLine();
            ServerManager.Stop();
        } 
    }
}
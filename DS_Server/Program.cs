using Newtonsoft.Json;
using PayCheckServerLib;

namespace DS_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server!");
            MiddleManStarter.Start();
            Console.WriteLine("Enter q to quit.");
            string stop = "";
            while (stop != "q")
            {
                stop = Console.ReadLine();
            };
            MiddleManStarter.Stop();
        }

    }
}
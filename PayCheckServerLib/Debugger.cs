using LLibrary;
using PayCheckServerLib.Helpers;

namespace PayCheckServerLib
{
    public class Debugger
    {
        public static L logger = new(true);

        public static bool IsDebug = ArgumentHandler.Debug;

        public static void PrintWebsocket(string ToPrint)
        {
            Console.ForegroundColor = GetColorByType("info");
            Console.WriteLine($"[WEBSOCKET] {ToPrint}");
            logger.Log("WEBSOCKET", ToPrint == null ? "NULL" : ToPrint);
            Console.ResetColor();
        }

        public static void PrintInfo(string ToPrint, string prefix = "INFO")
        {
            Console.ForegroundColor = GetColorByType("info");
            Console.WriteLine($"[{prefix}] {ToPrint}");
            logger.Log(prefix, ToPrint == null ? "NULL" : ToPrint);
            Console.ResetColor();
        }

        public static void PrintDebug(string ToPrint, string prefix = "DEBUG")
        {
            if (IsDebug)
            {
                Console.ForegroundColor = GetColorByType("debug");
                Console.WriteLine($"[{prefix}] {ToPrint}");
                logger.Log(prefix, ToPrint == null ? "NULL" : ToPrint);
                Console.ResetColor();
            }
            else
            {
                logger.Log(prefix, ToPrint == null ? "NULL" : ToPrint);
            }
        }

        public static void PrintWarn(string ToPrint, string prefix = "WARN")
        {
            Console.ForegroundColor = GetColorByType("warning");
            Console.WriteLine($"[{prefix}] {ToPrint}");
            logger.Log(prefix, ToPrint == null ? "NULL" : ToPrint);
            Console.ResetColor();
        }

        public static void PrintError(string ToPrint, string prefix = "ERROR")
        {
            Console.ForegroundColor = GetColorByType("error");
            Console.WriteLine($"[{prefix}] {ToPrint}");
            logger.Log(prefix, ToPrint == null ? "NULL" : ToPrint);
            Console.ResetColor();
        }

        static ConsoleColor GetColorByType(string type)
        {
            switch (type)
            {
                case "warning":
                    return ConsoleColor.DarkYellow;
                case "error":
                    return ConsoleColor.DarkRed;
                case "debug":
                    return ConsoleColor.DarkBlue;
                case "info":
                default:
                    return ConsoleColor.White;
            }
        }
    }
}

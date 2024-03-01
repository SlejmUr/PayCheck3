using System.ComponentModel;


namespace PayCheckServerLib.Helpers
{
    public class ArgumentHandler
    {

#if DEBUG
        public static bool Debug { get; internal set; } = true;
#else
        public static bool Debug { get; internal set; } = false;
#endif
        public static bool AskHelp { get; internal set; } = false;
        public static bool UseBetaFiles { get; internal set; } = false;
        public static bool ForceUpdate { get; internal set; } = false;

        public static bool NoUpdate { get; internal set; } = false;

        public static void MainArg(string[] args)
        {
#if DEBUG
            Debug = true;
#else
            Debug = HasParameter(args, "-debug");
#endif
            AskHelp = HasParameter(args, "-help");
            UseBetaFiles = HasParameter(args, "-beta");
            ForceUpdate = HasParameter(args, "-forceupdate");
#if DEBUG
            NoUpdate = true;
#else
            NoUpdate = HasParameter(args, "-noupdate");
#endif

            if (UseBetaFiles && ForceUpdate)
            {
                Debugger.PrintError("Force update and Beta is a conflict. Resolved by Disabling Beta");
                UseBetaFiles = false;
            }
            if (NoUpdate && ForceUpdate)
            {
                Debugger.PrintError("Force Update and No Update is a conflict. Resolved by NoUpdate Priority");
                ForceUpdate = false;
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("\tHelp");
            Console.WriteLine();
            Console.WriteLine("Program arguments to change server behavour:");
            Console.WriteLine();
            Console.WriteLine("-debug \t\t\t\t Running the server with debug mode");
            Console.WriteLine("-help \t\t\t\t Showing this text.");
            Console.WriteLine("-beta \t\t\t\t Using Beta Files.");
            Console.WriteLine("-forceupdate \t\t\t Forcing to update files.");
            Console.WriteLine("-noupdate \t\t\t Skipping updating files.");
            Console.WriteLine();
            Console.ReadLine();
            Environment.Exit(1);
        }


        #region Functions
        static int IndexOfParam(string[] args, string param)
        {
            for (var x = 0; x < args.Length; ++x)
            {
                if (args[x].Equals(param, StringComparison.OrdinalIgnoreCase))
                    return x;
            }

            return -1;
        }

        static bool HasParameter(string[] args, string param)
        {
            return IndexOfParam(args, param) > -1;
        }

        static T GetParameter<T>(string[] args, string param, T defaultValue = default(T))
        {
            var index = IndexOfParam(args, param);

            if (index == -1 || index == (args.Length - 1))
                return defaultValue;

            var strParam = args[index + 1];

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFromString(strParam);
            }

            return default(T);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Server.Helpers
{
    public class DS_Helper
    {
        //  SessionId > DSSesver
        public static Dictionary<string, PayCheckServerLib.Jsons.GS.DSInformationServer> DS_Servers = new();

        public static PayCheckServerLib.Jsons.GS.DSInformationServer MakeServer()
        {

            return new();
        }
    }
}

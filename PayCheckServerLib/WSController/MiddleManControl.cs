using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.GS;
using System.Text;
using static PayCheckServerLib.PC3Server;

namespace PayCheckServerLib.WSController
{
    public class MiddleManControl
    {
        public static void Control(byte[] buffer, long offset, long size, PC3Session session)
        {
            if (size == 0)
                return;
            buffer = buffer.Take((int)size).ToArray();
            var Data = Encoding.UTF8.GetString(buffer);
            var dataSplit = Data.Split("-END-");
            var func = dataSplit[0];
            var data = dataSplit[1];
            string returnData = "DropThis-END-.";
            switch (func)
            {
                case "DSInfoRsp":
                    var ds = JsonConvert.DeserializeObject<DSInformationServer>(data);
                    GSController.DSInfo.Add(ds.SessionId, ds);
                    GSController.DSInfoSentList.Add(ds.SessionId);
                    break;
                default:
                    break;
            }
        }
    }
}

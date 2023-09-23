using DS_Server.Helpers;
using Newtonsoft.Json;

namespace DS_Server
{
    public class WSController
    {
        public static void ProcessWS(string Data, MiddleManClient client)
        {            
            /*
             * Data Built like:
             * 
             * FunctionName-END-OtherData
             * 
             */
            var dataSplit = Data.Split("-END-");
            var func = dataSplit[0];
            var data = dataSplit[1];
            string returnData = "DropThis-END-.";
            switch (func)
            {
                case "DSInfoReq":
                    var x = data.Split(",");
                    var gs = DS_Helper.MakeServer(x[0], x[1], x[2]);
                    returnData = "DSInfoRsp-END-"+JsonConvert.SerializeObject(gs);
                    break;
                default:
                    break;
            }

            client.Send(returnData);
        }
    }
}

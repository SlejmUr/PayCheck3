using ModdableWebServer.Attributes;
using ModdableWebServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons.GS;
using PayCheckServerLib.ModdableWebServerExtensions;
using System.Text;

namespace PayCheckServerLib.WSController
{
    public class MiddleManControl
    {
        [WS("/middleman")]
        public static void MiddleMan(WebSocketStruct socketStruct)
        {
            var key = "MiddleManId-" + socketStruct.Request.Headers["middleman"];
            if (socketStruct.IsConnected)
            {
                if (!MiddleMans.ContainsKey(key))
                {
                    MiddleMans.Add(key, socketStruct);
                }
            }
            else
            {
                MiddleMans.Remove(key);
            }

            if (socketStruct.WSRequest != null)
            {
                Control(socketStruct.WSRequest.Value.buffer, socketStruct.WSRequest.Value.offset, socketStruct.WSRequest.Value.size, socketStruct);
            }

        }

        public static Dictionary<string, WebSocketStruct> MiddleMans = new();

        public static void Control(byte[] buffer, long offset, long size, WebSocketStruct socketStruct)
        {
            if (size == 0)
                return;
            buffer = buffer.Skip((int)offset).Take((int)size).ToArray();
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
                    socketStruct.SendWebSocketText(returnData);
                    break;
                default:
                    break;
            }
        }
    }
}

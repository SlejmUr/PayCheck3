using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class DLCs
    {
        public class PutDLC
        {
            public string steamId { get; set; }
            public string appId { get; set; }
        }

        public partial class DLC_Value
        {
            [JsonProperty("data")]
            public Dictionary<string, string[]> Data { get; set; }

            [JsonProperty("dlcType")]
            public string DlcType { get; set; }
        }



        [HTTP("PUT", "/platform/public/namespaces/{namespace}/users/{UserId}/dlc/steam/sync")]
        public static bool PUT_DLC_SteamSync(HttpRequest request, PC3Server.PC3Session session)
        {
            //var body = JsonConvert.DeserializeObject<PutDLC>(request.Body);
            ResponseCreator response = new ResponseCreator(204);
            response.SetHeader("Content-Type", "application/json");
            session.SendResponse(response.GetResponse());
            return true;
        }


        [HTTP("GET", "/cloudsave/v1/namespaces/{namespace}/records/dlc-entitlements")]
        public static bool GETdlcentitlements(HttpRequest request, PC3Server.PC3Session session)
        {
            TopLevel<DLC_Value> dlc = new()
            {
                SetBy = "SERVER",
                CreatedAt = "2023-09-25T12:01:02.096Z",
                Key = "dlc-entitlements",
                Namespace = session.HttpParam["namespace"],
                UpdatedAt = "2023-09-25T12:01:02.096Z",
                Value = new()
                {
                    Data = new(),
                    DlcType = "PSN"
                }
            };
            var data = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText("Files/DLC_Entiltements.json"));
            dlc.Value.Data = data;
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            response.SetBody(JsonConvert.SerializeObject(dlc));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

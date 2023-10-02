using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Helpers;
using PayCheckServerLib.Jsons;
using PayCheckServerLib.Jsons.Basic;

namespace PayCheckServerLib.Responses
{
    public class Social
    {
        [HTTP("PUT", "/social/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")]
        public static bool PutStatItemsBulk(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var statReq = JsonConvert.DeserializeObject<List<StatItemsBulkReq>>(request.Body);
            var rsp = UserStatController.AddStat(statReq, token);
            ResponseCreator responsecreator = new ResponseCreator();
            responsecreator.SetBody(JsonConvert.SerializeObject(rsp));
            session.SendResponse(responsecreator.GetResponse());
            return true;
        }

        [HTTP("GET", "/social/v1/public/namespaces/{namespace}/users/{userId}/statitems?limit={limit}&offset=0")]
        public static bool GetUserStatItems(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);

            var stat = UserStatController.GetStat(token.UserId, token.Namespace);

            ResponseCreator response = new ResponseCreator();
            DataPaging<UserStatItemsData> responsedata = new()
            {
                Data = stat,
                Paging = { }
            };
            response.SetBody(JsonConvert.SerializeObject(responsedata));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/social/v1/public/namespaces/{namespace}/users/{userId}/statitems?statCodes={statcode}&limit=20&offset=0")]
        public static bool GetUserStatItemsInfamy(HttpRequest request, PC3Server.PC3Session session)
        {
            var auth = session.Headers["authorization"].Replace("Bearer ", "");
            var token = TokenHelper.ReadToken(auth);
            var statcode = session.HttpParam["statcode"];
            ResponseCreator response = new ResponseCreator();
            DataPaging<UserStatItemsData> responsedata = new()
            {
                Data = new()
                {
                },
                Paging = { }
            };

            var stat = UserStatController.GetStat(token.UserId, token.Namespace);
            foreach (var item in stat)
            {
                if (item.StatCode == statcode)
                    responsedata.Data.Add(item);
            }

            response.SetBody(JsonConvert.SerializeObject(responsedata));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

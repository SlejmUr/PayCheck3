using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Social
    {
        [HTTP("PUT", "/social/v1/public/namespaces/pd3/users/{userId}/statitems/value/bulk")]
        public static bool PutStatItemsBulk(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator responsecreator = new ResponseCreator();
            PutStatItemsBulk response = new()
            {
                Details = new PutStatItemsBulkDetailsData()
                {
                    CurrentValue = 1.0f
                },
                StatCode = "game-started",
                Success = true,
            };
            var tosend = new object[] { response };
            responsecreator.SetBody(JsonConvert.SerializeObject(tosend));
            session.SendResponse(responsecreator.GetResponse());
            return true;
        }

        [HTTP("GET", "/social/v1/public/namespaces/pd3/users/{userId}/statitems?limit={limit}&offset=0")]
        public static bool GetUserStatItems(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            DataPaging<GetUserStatItemsData> responsedata = new()
            {
                Data = new()
                {
                    new GetUserStatItemsData()
                    {
                        CreatedAt = "2023-08-05T03:03:55.595Z",
                        Namespace = "pd3",
                        StatCode = "game-started",
                        StatName = "Game Started",
                        UpdatedAt = "2023-08-05T03:03:55.598Z",
                        UserId = session.HttpParam["userId"],
                        Value = 1.0f
                    }
                },
                Paging = { }
            };
            response.SetBody(JsonConvert.SerializeObject(responsedata));
            session.SendResponse(response.GetResponse());
            return true;
        }

        [HTTP("GET", "/social/v1/public/namespaces/pd3/users/{userId}/statitems?statCodes=infamy-point&limit=20&offset=0")]
        public static bool GetUserStatItemsInfamy(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            DataPaging<GetUserStatItemsData> responsedata = new()
            {
                Data = new()
                {
                    new GetUserStatItemsData()
                    {
                        CreatedAt = "2023-08-05T03:03:55.595Z",
                        Namespace = "pd3",
                        StatCode = "infamy-point",
                        StatName = "Infamy Points",
                        UpdatedAt = "2023-08-05T03:03:55.598Z",
                        UserId = session.HttpParam["userId"],
                        Value = 60.0f,
                        Tags = new()
                        {
                            "Infamy"
                        }
                    }
                },
                Paging = { }
            };
            response.SetBody(JsonConvert.SerializeObject(responsedata));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

using NetCoreServer;
using Newtonsoft.Json;
using PayCheckServerLib.Jsons;

namespace PayCheckServerLib.Responses
{
    public class Friends
    {
        [HTTP("GET", "/friends/namespaces/pd3beta/me/platforms")]
        public static bool MePlatform(HttpRequest request, PC3Server.PC3Session session)
        {
            ResponseCreator response = new ResponseCreator();
            response.SetHeader("Content-Type", "application/json");
            //  No friends for now
            FriendsPlatfrom friendsPlatfrom = new FriendsPlatfrom()
            {
                Data = new()
            };
            response.SetBody(JsonConvert.SerializeObject(friendsPlatfrom));
            session.SendResponse(response.GetResponse());
            return true;
        }
    }
}

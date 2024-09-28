using Newtonsoft.Json.Linq;
using RestSharp;

namespace JsonRequester
{
    internal class Requests
    {

        public static JObject? GetResponse(string url_end)
        {
            var client = new RestClient(Consts.URL + url_end);
            client.AddDefault();
            return Rest.Get(client, new RestRequest());
        }


        public static JObject? PostResponse(string url_end, string json)
        {
            var client = new RestClient(Consts.URL + url_end);
            client.AddDefault();
            var req = new RestRequest();
            req.AddBody(json);
            return Rest.Post(client, req);
        }


        public static JObject? GetBulkRecords(string data)
        {
            var client = new RestClient(Consts.URL + "/cloudsave/v1/namespaces/pd3/records/bulk");
            client.AddDefault();
            var req = new RestRequest();
            req.AddBody("{\"keys\":[\"" + data + "\"]}");
            return Rest.Post(client, req);
        }
    }
}

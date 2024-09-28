using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;

namespace JsonRequester;

internal class Rest
{
    #region JObject
    public static JObject? Put(RestClient client, RestRequest request)
    {
        try
        {
            RestResponse response = client.PutAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JObject.Parse(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }

    public static JObject? Post(RestClient client, RestRequest request)
    {
        try
        {
            RestResponse response = client.PostAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JObject.Parse(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }

    public static JObject? Get(RestClient client, RestRequest request)
    {
        try
        {
            RestResponse response = client.GetAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JObject.Parse(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }
    #endregion
    #region T Class
    public static T? Put<T>(RestClient client, RestRequest request) where T : class
    {
        try
        {
            RestResponse response = client.PutAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }

    public static T? Post<T>(RestClient client, RestRequest request) where T : class
    {
        try
        {
            RestResponse response = client.PostAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex);

            return null;
        }
    }

    public static T? Get<T>(RestClient client, RestRequest request) where T : class
    {
        try
        {
            RestResponse response = client.GetAsync(request).Result;
            if (response.Content != null)
            {
                Console.WriteLine(response.StatusCode);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }
    #endregion
    #region StatusCode
    public static HttpStatusCode? Delete(RestClient client, RestRequest request)
    {
        try
        {
            RestResponse response = client.DeleteAsync(request).Result;
            return response.StatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");

            return null;
        }
    }
    #endregion
}

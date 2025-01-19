﻿using ModdableWebServer;
using NetCoreServer;


namespace PayCheckServerLib
{
    public class ResponseCreator
    {
        private HttpResponse response;
        public ResponseCreator(int status = 200)
        {
            New(status);
        }

        public ResponseCreator(int status, Dictionary<string, string> kv, string content)
        {
            New(status);
            SetHeaders(kv);
            SetBody(content);
        }

        public ResponseCreator(int status, Dictionary<string, string> kv, byte[] content)
        {
            New(status);
            SetHeaders(kv);
            SetBody(content);
        }

        public void New(int status = 200)
        {
            response = new();
            response.Clear();
            response.SetBegin(status);
			response.SetHeader("server", "PayCheck3"); // AccelByte sets the server header to "istio-envoy", this is just to add the ability for clients to know they are using PayCheck3
		}

        public void SetHeaders(Dictionary<string, string> kv)
        {
            foreach (var item in kv)
            {
                SetHeader(item.Key, item.Value);
            }
        }

        public void SetHeader(string key, string value)
        {
            response.SetHeader(key, value);
        }

        public void SetCookie(string name, string value, int maxAge = 86400000, string path = "", string domain = "", bool secure = true, bool strict = false, bool httpOnly = true)
        {
            response.SetCookie(name, value, maxAge, path, domain, secure, strict, httpOnly);
        }

        public void SetBody(string content)
        {
            response.SetBody(content);
        }

        public void SetBody(byte[] content)
        {
            response.SetBody(content);
        }

        public void SetResponse(HttpResponse rsp)
        {
            response = rsp;
        }

        public HttpResponse GetResponse()
        {
            //Debugger.logger.Debug("response:\n" + response);
            return response;
        }

    }
}

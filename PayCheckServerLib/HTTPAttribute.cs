namespace PayCheckServerLib
{
    public class HTTPAttribute : Attribute
    {
        public string method;
        public string url;

        public HTTPAttribute(string method, string url)
        {
            this.method = method;
            this.url = url;
        }
    }
}

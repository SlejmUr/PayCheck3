namespace PayCheckServerLib
{
    public class UrlHelper
    {
        #region Parameter url stuff
        public static bool Match(string url, string pattern, out Dictionary<string, string> vals)
        {
            vals = new();
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

            vals = new Dictionary<string, string>();
            string[] urlParts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string[] patternParts = pattern.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length != patternParts.Length) return false;

            for (int i = 0; i < urlParts.Length; i++)
            {
                string paramName = ExtractParameter(patternParts[i]);

                if (string.IsNullOrEmpty(paramName))
                {
                    // no pattern
                    if (!urlParts[i].Equals(patternParts[i]))
                    {
                        vals = new();
                        return false;
                    }
                }
                else
                {
                    vals.Add(
                        paramName.Replace("{", "").Replace("}", ""),
                        urlParts[i]);
                }
            }
            return true;
        }

        private static string ExtractParameter(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

            if (pattern.Contains("{"))
            {
                if (pattern.Contains("}"))
                {
                    int indexStart = pattern.IndexOf('{');
                    int indexEnd = pattern.LastIndexOf('}');
                    if (indexEnd - 1 > indexStart)
                    {
                        return pattern.Substring(indexStart, indexEnd - indexStart + 1);
                    }
                }
            }

            return null;
        }
        #endregion
    }
}

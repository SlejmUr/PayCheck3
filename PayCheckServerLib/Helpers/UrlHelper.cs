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
            string[] urlParts = SplitUrl(url);
            string[] patternParts = SplitUrl(pattern);

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
                        urlParts[i].Split('=').Last());
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

            return string.Empty;
        }
        
        private static string[] SplitUrl(string url)
        {
            string[] urlPaths = url.Split('/', StringSplitOptions.RemoveEmptyEntries);;
            urlPaths = SplitUrl(urlPaths, '?');
            urlPaths = SplitUrl(urlPaths, '&');

            return urlPaths;
        }

        private static string[] SplitUrl(string[] urlParts, char splitChar)
        {
            List<string> parts = new List<string>();

            foreach (var part in urlParts)
            {
                if (!part.Contains(splitChar))
                {
                    parts.Add(part);
                    continue;
                }

                string[] subParts = part.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                parts.AddRange(subParts);
            }

            return parts.ToArray();
        }
        
        #endregion
    }
}

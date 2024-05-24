namespace LazyRedpaw.FigmaToUnity
{
    public static class StringExtensions
    { 
        public static string ExtractString(this string str, string startStr, string endStr)
        {
            int startIndex = str.IndexOf(startStr) + startStr.Length;
            int endIndex = str.IndexOf(endStr, startIndex);
            return str.Substring(startIndex, endIndex - startIndex);
        }
    }
}


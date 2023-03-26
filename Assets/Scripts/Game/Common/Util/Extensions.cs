namespace Game.Common.Util
{
    public static class Extensions
    {
        public static string RemoveSuffix(this string s, string suffix)
        {
            return s.EndsWith(suffix) ? s[..^suffix.Length] : s;
        }
    }
}
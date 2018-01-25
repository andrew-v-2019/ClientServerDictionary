namespace Client
{
    public static class StringExtensions
    {
        public static string GetInputStringItem(this string s, int idx)
        {
            var split = s.Split(' ');
            var item = split[idx].Trim().ToLower();
            return item;
        }
    }
}

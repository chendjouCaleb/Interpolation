namespace TextBinding.Utilities
{
    public class Strings
    {
        public static string AsciiWhiteSpaces = "\u0020\u0009\u000A\u000C\u000D";
        public static bool IsWhiteSpace(char c)
        {
            return AsciiWhiteSpaces.Contains(c);
        }
    }
}
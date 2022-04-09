using System;
using TextBinding.Utilities;

namespace TextBinding
{
    
    public class Token
    {
        public Token(string value, TokenType type, TokenIndex startIndex)
        {
            Value = value;
            Type = type;
            StartIndex = startIndex;
        }

        public string Value { get; set; }
        public TokenType Type { get; set; }
        public TokenIndex StartIndex { get; set; }

        public override string ToString()
        {
            return $"Value: {Value}, Type:{Type}, Index: {StartIndex.Index};";
        }
    }
}
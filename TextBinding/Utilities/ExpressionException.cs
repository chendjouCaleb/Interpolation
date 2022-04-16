using System;

namespace TextBinding.Utilities
{
    public class ExpressionException: ApplicationException
    {
        public ExpressionError Error { get; }
        public TokenIndex Index { get; set; }

        public ExpressionException(ExpressionError error, TokenIndex index)
        {
            Error = error;
            Index = index;
        }

        public ExpressionException(ExpressionError error, TokenIndex index, string? message) : base(message)
        {
            Error = error;
            Index = index;
        }
    }
}
using System;
using System.Collections.Generic;

namespace TextBinding.Utilities
{
    public class TokenIndex
    {
        /// <summary>
        /// Line index of the token.
        /// </summary>
        public int Line { get; set; }
        
        /// <summary>
        /// Index of token relative to the current line.
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Global index of the token.
        /// </summary>
        public int Index { get; set; }

        public TokenIndex()
        {

        }

        public TokenIndex(TokenIndex index)
        {
            Line = index.Line;
            Col = index.Col;
            Index = index.Index;
        }

        public static TokenIndex Zero => new (0, 0, 0);

        public static TokenIndex To(int index) => new(index, 0, index);

        public TokenIndex(int index, int line, int col)
        {
            Index = index;
            Line = line;
            Col = col;
        }

        public override string ToString()
        {
            return $"[Index = {Index}; Line = {Line}; Col = {Col}]";
        }

        public bool Equals(TokenIndex? other)
        {
            return other != null && Line == other.Line && Col == other.Col && Index == other.Index;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TokenIndex) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line, Col, Index);
        }
    }
}
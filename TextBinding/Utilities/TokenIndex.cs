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
        public int LineIndex { get; set; }

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
            LineIndex = index.LineIndex;
            Index = index.Index;
        }

        public override string ToString()
        {
            return $"[Line = {Line}; LineIndex = {LineIndex}; Index = {Index}]";
        }
    }
}
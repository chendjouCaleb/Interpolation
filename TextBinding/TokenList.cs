using System.Collections;
using System.Collections.Generic;

namespace TextBinding
{
    public class TokenList:IEnumerable<Token>
    {
        
        private List<Token> _tokens = new ();
        
        
        public TokenList() {}
        public TokenList(IEnumerable<Token> tokens)
        {
            _tokens = new List<Token>(tokens);
        }
        
        public IEnumerator<Token> GetEnumerator() => _tokens.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _tokens.GetEnumerator();

        public int Count => _tokens.Count;
        
        
        public List<Token> ToList() => new List<Token>(_tokens);
        
        public void Add(Token item) => _tokens.Add(item);
        public Token this[int index] => _tokens[index];
        
        
    }
}
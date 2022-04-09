using System.Text;
using TextBinding.Utilities;

namespace TextBinding
{
    public class TextCollector
    {
        private TextIterator _iterator;

        private StringBuilder _builder;

        private TokenIndex? _startIndex;


        public TextCollector(TextIterator iterator)
        {
            _iterator = iterator;
            _builder = new ();
        }
        
        
        public void Take()
        {
            //Console.WriteLine($"I = {_iterator.Index.Index}; C={_iterator.Current}");
            Start();
            _builder.Append(_iterator.Current);
            _iterator.Next();
        }

        public void Take(char c)
        {
            Start();
            _builder.Append(c);
        }

        public void Take(char character, TokenIndex index)
        {
            if (_startIndex == null)
            {
                _startIndex = index;
            }
            
            _builder.Append(character);
        }

        public void Take(string value, TokenIndex startIndex)
        {
            _startIndex = startIndex;
            _builder.Append(value);
        }

        public void Start()
        {
            if (_startIndex == null)
            {
                _startIndex = _iterator.Index;
            }
        }


        public Token? Stop()
        {
            if (_builder.Length == 0)
            {
                return null;
            }
            
            Token token = new Token(_builder.ToString(), TokenType.Text, _startIndex);

            _builder.Clear();
            _startIndex = null;

            return token;
        }
    }
}
using System;
using System.Text;
using TextBinding.Utilities;

namespace TextBinding
{
    public class Tokenizer
    {
        private TokenList _tokens = new();
        private TextIterator _iterator;
        private TextCollector _collector;
        private string Operators = "*/-+%:?<>";
        private TextIterator _it => _iterator;

        private bool _isOpen;

        public TokenList Tokens => new TokenList(_tokens);

        public Tokenizer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new InvalidOperationException("Cannot iterate on null or empty text");
            }

            _iterator = new TextIterator(text);
            _collector = new TextCollector(_it);
        }

        public void Tokenize()
        {
            while (_it.Has)
            {
                if (_it.Current == '{')
                {
                    _isOpen = TakeOpen() != null;
                }

                else if (_it.Current == '}')
                {
                    _isOpen = TakeClose() == null;
                }

                else if (_isOpen)
                {
                    if (_it.IsDigit())
                    {
                        TakeNumber();
                    }
                    else if (_it.IsLetter() || _it.Is('_'))
                    {
                        TakeIdentifier();
                    }
                    else if (_it.IsIn(Operators))
                    {
                        TakeCurrent(TokenType.Operator);
                    }
                    else if (_it.Is(','))
                    {
                        TakeCurrent(TokenType.Comma);
                    }
                    else if (_it.IsIn("."))
                    {
                        TakeCurrent(TokenType.Dot);
                    }
                    else if (_it.Is('"'))
                    {
                        TakeString();
                    }
                    else if (_it.IsIn("("))
                    {
                        TakeCurrent(TokenType.ParenthesisOpen);
                    }
                    else if (_it.IsIn(")"))
                    {
                        TakeCurrent(TokenType.ParenthesisClose);
                    }
                    else if (_it.IsIn(Strings.AsciiWhiteSpaces))
                    {
                        SkipWhiteSpace();
                    }
                    else if (_it.Is('}'))
                    {
                        _isOpen = TakeClose() == null;
                    }
                }
                else
                {
                    TakeText();
                }
            }
        }

        public Token TakeText()
        {
            StringBuilder builder = new();
            TokenIndex index = _it.Index;

            while (_it.Has && _it.Current != '{')
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            Token token = new Token(builder.ToString(), TokenType.Text, index);
            _tokens.Add(token);
            return token;
        }

        public Token TakeNumber()
        {
            StringBuilder builder = new ();
            TokenIndex index = _it.Index;

            while (_it.Has && (_it.IsDigit() || _it.Current == '\''))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('.'))
            {
                builder.Append('.');
                _it.Next();
            }

            while (_it.Has && (_it.IsDigit() || _it.Current == '\''))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            Token token = new Token(builder.ToString(), TokenType.Number, index);
            _tokens.Add(token);
            return token;
        }

        public Token TakeIdentifier()
        {
            StringBuilder builder = new();
            TokenIndex index = _it.Index;

            while (_it.IsLetter() || _it.IsDigit() || _it.Is('_'))
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            Token token = new Token(builder.ToString(), TokenType.Identifier, index);

            _tokens.Add(token);
            return token;
        }

        public Token TakeCurrent(TokenType type)
        {
            TokenIndex index = _iterator.Index;
            Token token = new Token(_it.Current.ToString(), type, index);
            _tokens.Add(token);
            _it.Next();
            return token;
        }

        public Token TakeString()
        {
            TokenIndex index = _it.Index;
            // Skip current "
            _it.Next();
            StringBuilder builder = new();

            while (_it.Has && _it.Current != '"')
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('"'))
            {
                _it.Next();
            }

            Token token = new(builder.ToString(), TokenType.String, index);
            _tokens.Add(token);

            return token;
        }


        public Token? TakeOpen()
        {
            if (_it.Current == '{')
            {
                TokenIndex index = _it.Index;
                _it.Next();
                if (_it.Current == '{')
                {
                    _it.Next();
                    Token token = new Token("{{", TokenType.Open, index);
                    _tokens.Add(token);
                    return token;
                }
            }

            return null;
        }


        public Token? TakeClose()
        {
            if (_it.Current == '}')
            {
                TokenIndex index = _it.Index;
                _it.Next();
                if (_it.Current == '}')
                {
                    _it.Next();
                    Token token = new Token("}}", TokenType.Close, index);
                    _tokens.Add(token);
                    return token;
                }
            }

            return null;
        }

        public void SkipWhiteSpace()
        {
            while (_it.Has && _it.IsWhiteSpace())
            {
                _it.Next();
            }
        }
    }
}
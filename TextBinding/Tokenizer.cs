using System;
using System.Text;
using TextBinding.Utilities;

namespace TextBinding
{
    public class Tokenizer
    {
        private TokenList _tokens = new();
        public TextIterator Iterator { get; }
        private TextCollector _collector;
        public static string Operators = "[]().,:+-*/%!|&=<>?:";
        private TextIterator _it => Iterator;

        public TokenIndex Index => Iterator.Index;


        private bool _isOpen;

        public TokenList Tokens => new(_tokens);

        public Tokenizer(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new InvalidOperationException("Cannot iterate on null or empty text");
            }

            Iterator = new TextIterator(text);
            _collector = new TextCollector(_it);
        }

        public void Tokenize()
        {
            while (_it.Has)
            {
                if (_it.Current == '{')
                {
                    _isOpen = TryTakeOpen() != null;
                }

                else if (_it.Current == '}')
                {
                    _isOpen = TryTakeClose() == null;
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
                        _isOpen = TryTakeClose() == null;
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
            StringBuilder builder = new();
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

            Token token = new Token(builder.ToString(), TokenType.Id, index);

            _tokens.Add(token);
            return token;
        }

        public Token TakeCurrent(TokenType type)
        {
            TokenIndex index = _it.Index;
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
            StringBuilder builder = new("\"");

            while (_it.Has && _it.Current != '"')
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('"'))
            {
                builder.Append("\"");
                _it.Next();
            }

            Token token = new(builder.ToString(), TokenType.String, index);
            _tokens.Add(token);

            return token;
        }


        public Token TryTakeOpen()
        {
            TokenIndex index = _it.Index;
            Token? token;
            //Skip current {
            _it.Next();
            if (_it.Is('{'))
            {
                _it.Next();
                token = new("{{", TokenType.Open, index);
            }
            else
            {
                token = new("{", TokenType.Text, index);
            }

            _tokens.Add(token);
            return token;
        }


        public Token TryTakeClose()
        {
            TokenIndex index = _it.Index;
            
            // skip current }
            _it.Next();
            if (_it.Is('}'))
            {
                _it.Next();
                Token token = new ("}}", TokenType.Close, index);
                _tokens.Add(token);
                return token;
            }

            throw new ExpressionException(ExpressionError.UnknownToken, index);
        }

        public Token TakeOperator()
        {
            Assertions.IsTrue(_it.IsIn(Operators));
            var index = _it.Index;
            StringBuilder builder = new();
            while (_it.IsIn(Operators))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            Token token = new (builder.ToString(), TokenType.Operator, index);
            _tokens.Add(token);
            return token;
        }

        public Token TakeInteger()
        {
            Assertions.IsTrue(_it.IsIn("12346789"));
            var index = _it.Index;
            
            var builder = new StringBuilder();
            while (_it.IsIn("123467890"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            
            Token token = new (builder.ToString(), TokenType.Integer, index);
            _tokens.Add(token);
            return token;
        }

        public Token TakeZeroNumber()
        {
            Assertions.IsTrue(_it.Is('0'));
            TokenIndex index = _it.Index;
            _it.Next();
            if (_it.Is('x'))
            {
                return TakeHexadecimal(index);
            }else if (_it.Is('b'))
            {
                return TakeBinary(index);
            }else if (_it.Is('o'))
            {
                return TakeOctal(index);
            }

            return TakeDecimal(index);
        }


        public Token TakeChar()
        {
            Assertions.IsTrue(_it.Is('\''));
            TokenIndex index = _it.Index;
            // Skip '
            _it.Next();
            StringBuilder builder = new("'");
            while (_it.Has && _it.Current != '\'')
            {
                builder.Append(_it.Current);
                _it.Next();
            }

            if (_it.Is('\''))
            {
                builder.Append("'");
                _it.Next();
            }

            Token token = new(builder.ToString(), TokenType.Char, index);
            _tokens.Add(token);
            return token;
        }

        public Token TakeDecimal(TokenIndex index)
        {
            var builder = new StringBuilder("0");
            while (_it.IsIn("123467890"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            
            Token token = new (builder.ToString(), TokenType.Decimal, index);
            _tokens.Add(token);
            return token;
        }
        
        
        public Token TakeOctal(TokenIndex index)
        {
            var builder = new StringBuilder("0o");
            
            // Skip current o
            _it.Next();
            while (_it.IsIn("0123467"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            
            Token token = new (builder.ToString(), TokenType.Octal, index);
            _tokens.Add(token);
            return token;
        }
        
        
        public Token TakeBinary(TokenIndex index)
        {
            var builder = new StringBuilder("0b");
            
            // Skip current b
            _it.Next();
            while (_it.IsIn("01"))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            
            Token token = new (builder.ToString(), TokenType.Binary, index);
            _tokens.Add(token);
            return token;
        }
        
        
        
        public Token TakeHexadecimal(TokenIndex index)
        {
            var builder = new StringBuilder("0x");
            
            // Skip current x
            _it.Next();
            while (_it.IsInRange('0', '9') || _it.IsInRange('a', 'f') || _it.IsInRange('A', 'F'))
            {
                builder.Append(_it.Current);
                _it.Next();
            }
            
            Token token = new (builder.ToString(), TokenType.Hexadecimal, index);
            _tokens.Add(token);
            return token;
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
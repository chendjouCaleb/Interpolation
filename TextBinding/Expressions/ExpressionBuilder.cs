using System;
using System.Globalization;
using TextBinding.Utilities;

namespace TextBinding.Expressions
{
    public class ExpressionBuilder
    {
        private string _text;
        private Iterator<Token> _it;

        public ExpressionBuilder(Iterator<Token> it)
        {
            _it = it;
        }

        public void Build(BindingExpression expression)
        {
            while (_it.Has && _it.Current.Type != TokenType.Close)
            {
                Take(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Close)
            {
                _it.Next();
            }
        }


        public void Take(BindingExpression expression)
        {
            if (_it.Current.Type == TokenType.Operator)
            {
                TakeUnaryOperator(expression);
            }
            else if (_it.Current.Type == TokenType.Number)
            {
               TakeNumber(expression);
               TakeBinaryOperator(expression);
            }
            else if (_it.Current.Type == TokenType.ParenthesisOpen)
            {
                TakeSubExpression(expression);
                TakeBinaryOperator(expression);
            }
            else if (_it.Current.Type == TokenType.Id)
            {
                var item = HandleIdentifier();
                expression.Add(item);
                TakeBinaryOperator(expression);
            } 
            else
            {
                throw new InvalidOperationException("Invalid token for expression: " + _it.Current);
            }
        }

        private ValueExpressionItem HandleIdentifier()
        {
            string identifier = _it.Current.Value;
            _it.Next();
            if (_it.Current.Type == TokenType.ParenthesisOpen)
            {
               return TakeMethod(identifier);
            }
            return TakeProperty(identifier);
        }

        private MethodExpressionItem TakeMethod(string identifier)
        {
            // Skip '('.
            _it.Next();
            MethodExpressionItem item = new ()
            {
                Name = identifier,
                ExpressionType = ExpressionValueType.Method
            };

            while (_it.Has && _it.Current.Type != TokenType.ParenthesisClose)
            {
                item.ParamExpressions.Add(TakeMethodParamsExpression());
            }
            
            // Skip ')'.
            _it.Next();
            TakeChain(item);

            return item;
        }
        
        private PropertyExpressionItem TakeProperty(string identifier)
        {
            PropertyExpressionItem item = new()
            {
                Name = identifier,
                ExpressionType = ExpressionValueType.Property
            };
            TakeChain(item);
            return item;
        }


        private void TakeChain(ValueExpressionItem item)
        {
            if (_it.Has && _it.Current.Type == TokenType.Dot)
            {
                TakeNext(item, null);
            }
            
        }

        private ValueExpressionItem TakeNext(ValueExpressionItem? item, ValueExpressionItem? prev)
        {
            // Skip .;
            _it.Next();

            ValueExpressionItem next = HandleIdentifier();
            //_it.Next();
            if (prev != null)
            {
                prev.Field = next;   
            }
            else
            {
                item.Field = next;
            }

            if (_it.Has && _it.Current.Type == TokenType.Dot)
            {
                TakeNext(item, next);
            }

            return next;
        }

        private void TakeNumber(BindingExpression expression)
        {
            LiteralExpressionItem item = new(double.Parse(_it.Current.Value, CultureInfo.InvariantCulture), ExpressionValueType.Number)
            {
                Name = _it.Current.Value
            };
            _it.Next();
            
            if (_it.Has && _it.Current.Type == TokenType.Dot)
            {
                TakeNext(item, null);
            }

            expression.Add(item);
            //_it.Next();
        }

        private void TakeUnaryOperator(BindingExpression expression)
        {
            OperatorExpressionItem item = new()
            {
                Name = _it.Current.Value,
                IsBinary = false,
                IsUnary = true
            };
            expression.Add(item);
            _it.Next();
        }
        
        private void TakeBinaryOperator(BindingExpression expression)
        {
            if (_it.Has && _it.Current.Type == TokenType.Operator)
            {
                OperatorExpressionItem item = new()
                {
                    Name = _it.Current.Value,
                    IsBinary = true,
                    IsUnary = false,
                };
                expression.Add(item);
                _it.Next();
            }
        }
        
        private void TakeSubExpression(BindingExpression expression)
        {
            // skip current '('
            _it.Next();
            SubExpressionItem item = new ();
            while (_it.Has && _it.Current.Type != TokenType.ParenthesisClose)
            {
                Take(item.Expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.ParenthesisClose)
            {
                _it.Next();
            }
            expression.Add(item);
            TakeChain(item);
        }
        
        private BindingExpression TakeMethodParamsExpression()
        {
            BindingExpression expression = new ();
            while (_it.Has && _it.Current.Type != TokenType.Comma && _it.Current.Type != TokenType.ParenthesisClose)
            {
                Take(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Comma)
            {
                _it.Next();
            }

            return expression;
        }
    }
}
using System;
using System.Globalization;
using TextBinding.Operators;
using TextBinding.Utilities;

namespace TextBinding.Expressions
{
    public class ExpressionBuilder
    {
        private readonly Iterator<Token> _it;

        public ExpressionBuilder(Iterator<Token> it)
        {
            _it = it;
        }

        public BindingExpression Build()
        {
            SkipOpen();
            BindingExpression expression = new();
            Build(expression);
            return expression;
        }

        public void Build(BindingExpression expression)
        {
            SkipOpen();
            while (_it.Has && _it.Current.Type != TokenType.Close)
            {
                Take(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Close)
            {
                _it.Next();
            }
        }

        public BindingExpression Take(int count)
        {
            SkipOpen();
            BindingExpression expression = new();
            for (int i = 0; i < count; i++)
            {
                Take(expression);
            }

            return expression;
        }


        public void Take(BindingExpression expression)
        {
            //var x = -1 - 2 + +3;
            if (_it.Current.Type == TokenType.Operator)
            {
                TakeUnaryOperator(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Integer)
            {
                var item = TakeInteger(expression);
            }
            else if (_it.Has && _it.Current.Type == TokenType.ParenthesisOpen)
            {
                TakeSubExpression(expression);
            }
            else if (_it.Has && _it.Current.Type == TokenType.Id)
            {
                HandleWord(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Operator)
            {
                TakeBinaryOperator(expression);
            }

            // else
            // {
            //     throw new InvalidOperationException("Invalid token for expression: " + _it.Current);
            // }
        }

        private IntegerExpressionItem TakeInteger(BindingExpression expression)
        {
            IntegerExpressionItem item = new(int.Parse(_it.Current.Value, CultureInfo.InvariantCulture))
            {
                Name = _it.Current.Value
            };
            _it.Next();

            expression.Add(item);
            return item;
        }

        private void HandleWord(BindingExpression expression)
        {
            if (_it.Has && _it.Current.Value == "true")
            {
                BooleanExpressionItem item = new(true);
                _it.Next();
                expression.Add(item);
            }
            else if (_it.Has && _it.Current.Value == "false")
            {
                BooleanExpressionItem item = new(false);
                _it.Next();
                expression.Add(item);
            }
            else if (_it.Current.Value == "null")
            {
                NullExpressionItem item = new();
                _it.Next();
                expression.Add(item);
            }
            else
            {
                PropertyExpressionItem item = new();
                item.ExpressionType = ExpressionValueType.Property;
                item.Name = _it.Current.Value;
                _it.Next();
                expression.Add(item);
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

        private void SkipOpen()
        {
            if (_it.Has && _it.Current.Type == TokenType.Open)
            {
                _it.Next();
            }
        }

        private MethodExpressionItem TakeMethod(string identifier)
        {
            // Skip '('.
            _it.Next();
            MethodExpressionItem item = new()
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


        private OperatorExpressionItem? TakeUnaryOperator(BindingExpression expression)
        {
            if (!_it.Has || _it.Current.Type != TokenType.Operator)
            {
                return null;
            }

            Token token = _it.Current;
            OperatorExpressionItem? op;
            if (token.Value == "+")
            {
                op = new(Operator.Plus);
            }
            else if (token.Value == "-")
            {
                op = new(Operator.Minus);
            }
            else
            {
                throw new ExpressionException(ExpressionError.UnknownUnaryOperator, _it.Current.StartIndex,
                    $"L'operateur {token.Value} est inconnu.");
            }

            expression.Add(op);
            _it.Next();
            return op;
        }


        private OperatorExpressionItem? TakeBinaryOperator(BindingExpression expression)
        {
            if (!(_it.Has && _it.Current.Type == TokenType.Operator))
            {
                return null;
            }

            Token token = _it.Current;
            OperatorExpressionItem? item = null;

            if (token.Value == "+")
            {
                item = new(Operator.Add);
            }
            else if (token.Value == "-")
            {
                item = new(Operator.Subtract);
            }
            else if (token.Value == "*")
            {
                item = new(Operator.Mul);
            }
            else if (token.Value == "/")
            {
                item = new(Operator.Div);
            }
            else if (token.Value == "%")
            {
                item = new(Operator.Modulo);
            }
            else if (token.Value == "&&")
            {
                item = new(Operator.And);
            }
            else if (token.Value == "||")
            {
                item = new(Operator.Or);
            }
            else if (token.Value == "??")
            {
                item = new(Operator.NullCoalescing);
            }
            else if (token.Value == "==")
            {
                item = new(Operator.Equal);
            }
            else if (token.Value == "!=")
            {
                item = new(Operator.NotEqual);
            }
            else if (token.Value == "<")
            {
                item = new(Operator.LowerThan);
            }
            else if (token.Value == ">")
            {
                item = new(Operator.UpperThan);
            }
            else if (token.Value == "<=")
            {
                item = new(Operator.LowerOrEqualThan);
            }
            else if (token.Value == ">=")
            {
                item = new(Operator.UpperOrEqualThan);
            }else if (token.Value == "?")
            {
                item = new(Operator.Ternary);
            }

            if (item != null)
            {
                expression.Add(item);
                _it.Next();
            }

            return item;
        }

        private void TakeSubExpression(BindingExpression expression)
        {
            // skip current '('
            _it.Next();
            SubExpressionItem item = new();
            while (_it.Has && _it.Current.Type != TokenType.ParenthesisClose)
            {
                Take(item.Expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.ParenthesisClose)
            {
                _it.Next();
            }

            expression.Add(item);
            //TakeChain(item);
        }

        private BindingExpression TakeMethodParamsExpression()
        {
            BindingExpression expression = new();
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
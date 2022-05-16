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

            if (_it.Has && _it.Current.Type != TokenType.Close)
            {
                throw new InvalidOperationException("Invalid token for expression: " + _it.Current);
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
            else if (_it.Has && _it.Current.Type == TokenType.Boolean)
            {
                TakeBoolean(expression);
            }
            else if (_it.Has && _it.Current.Type == TokenType.Null)
            {
                TakeNull(expression);
            }
            else if (_it.Has && _it.Current.Type == TokenType.Id)
            {
                HandleMember(expression);
            }
            else if (_it.Has && _it.Current.Type == TokenType.Colon)
            {
                TakeTernarySeparator(expression);
            }else if (_it.Has && _it.Current.Type == TokenType.MemberAccess)
            {
                TakeMemberAccess(expression);
            }else if (_it.Has && _it.Current.Type == TokenType.NullConditionalMemberAccess)
            {
                TakeMemberAccess(expression);
            }

            if (_it.Has && _it.Current.Type == TokenType.Operator)
            {
                TakeBinaryOperator(expression);
            }
            
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


        private void TakeBoolean(BindingExpression expression)
        {
            BooleanExpressionItem item = new(_it.Current.Value == "true");
            _it.Next();
            expression.Add(item);
        }

        private void TakeNull(BindingExpression expression)
        {
            NullExpressionItem item = new();
            _it.Next();
            expression.Add(item);
        }

        private PunctuatorExpressionItem TakeTernarySeparator(BindingExpression expression)
        {
            PunctuatorExpressionItem item = new(":");
            _it.Next();
            expression.Add(item);
            return item;
        }
        
        private MemberAccessExpressionItem TakeMemberAccess(BindingExpression expression)
        {
            MemberAccessExpressionItem item = new(_it.Current.Type == TokenType.NullConditionalMemberAccess);
            _it.Next();
            expression.Add(item);
            return item;
        }


        private ValueExpressionItem HandleMember(BindingExpression expression)
        {
            string identifier = _it.Current.Value;
            _it.Next();
            if (_it.Current.Type == TokenType.ParenthesisOpen)
            {
                return TakeMethod(identifier, expression);
            }

            return TakeProperty(identifier, expression);
        }

        private PropertyExpressionItem TakeProperty(string identifier, BindingExpression expression)
        {
            PropertyExpressionItem item = new()
            {
                Name = identifier,
                ExpressionType = ExpressionValueType.Property
            };
            expression.Add(item);
            return item;
        }


        private MethodExpressionItem TakeMethod(string identifier, BindingExpression expression)
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
            expression.Add(item);

            return item;
        }


        private void SkipOpen()
        {
            if (_it.Has && _it.Current.Type == TokenType.Open)
            {
                _it.Next();
            }
        }


        private void TakeChain(ValueExpressionItem item)
        {
            if (_it.Has && _it.Current.Type == TokenType.MemberAccess)
            {
                TakeNext(item, null);
            }
        }

        private ValueExpressionItem TakeNext(ValueExpressionItem? item, ValueExpressionItem? prev)
        {
            // Skip .;
            // _it.Next();
            //
            // ValueExpressionItem next = HandleMember();
            // //_it.Next();
            // if (prev != null)
            // {
            //     prev.Field = next;
            // }
            // else
            // {
            //     item.Field = next;
            // }
            //
            // if (_it.Has && _it.Current.Type == TokenType.Dot)
            // {
            //     TakeNext(item, next);
            // }
            //
            // return next;
            throw new NotImplementedException();
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
            }
            else if (token.Value == "?")
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

        private OperatorExpressionItem? TakeOperator(BindingExpression expression, Operator @operator)
        {
            Token token = _it.Current;
            OperatorExpressionItem item = new(@operator);
            _it.Next();
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
                //throw new InvalidOperationException();
            }

            if (_it.Has && _it.Current.Type == TokenType.Comma)
            {
                _it.Next();
            }

            return expression;
        }
    }
}
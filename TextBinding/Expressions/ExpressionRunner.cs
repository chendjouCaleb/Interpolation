using System;
using System.Collections.Generic;
using TextBinding.Operators;
using TextBinding.Utilities;

namespace TextBinding.Expressions
{
    public class ExpressionRunner
    {
        private Iterator<IExpressionItem> _it;

        public ExpressionRunner(List<IExpressionItem> items)
        {
            _it = new Iterator<IExpressionItem>(items);
        }

        public object Call()
        {
            object? result = null;
            if (_it.Has)
            {
                result = Call(_it.Current);
                //_it.Next();
            }

            while (_it.Has && _it.Current.IsArithmeticOperator)
            {
                result = CallOperator(result, _it.Current as OperatorExpressionItem);
            }

            Console.WriteLine("Result: " + result);

            return result;
        }

        public object Call(IExpressionItem item)
        {
            if (item is OperatorExpressionItem op && op.IsUnary)
            {
                return CallUnaryOperator(op);
            }else if (item is ValueExpressionItem value)
            {
                return CallValue(value);
            }

            throw new InvalidOperationException("Uncallable expression item type: " + item.GetType());
        }

        public object CallValue(ValueExpressionItem item)
        {
            object result = null;
            if (item is SubExpressionItem subExpressionItem)
            {
               result = new ExpressionRunner(new List<IExpressionItem>(subExpressionItem.Expression.Items)).Call();
            }
            else if (item is LiteralExpressionItem value)
            {
                result = value.Value;
            }
            else
            {
                throw new InvalidOperationException("Handle expression type: " + item);
            }
            _it.Next();
            return result;
        }


        private object CallUnaryOperator(OperatorExpressionItem op)
        {
            DoubleOperatorOverload operatorOverload = new();
            _it.Next();
            object value = CallValue(_it.Current as ValueExpressionItem);
            object result = null;
            if (op.Name == "+")
            {
                result = operatorOverload.Plus(value);
            }
            else if (op.Name == "-")
            {
                result = operatorOverload.Minus(value);
            }
            else
            {
                throw new InvalidOperationException("Unhandled unary operator: " + op.Name);
            }

            _it.Next();
            return result;
        }

        private object CallOperator(object currentValue, OperatorExpressionItem op)
        {
            DoubleOperatorOverload operatorOverload = new();
            object result = null;
            // Skip current op.
            _it.Next();

            object value = Call(_it.Current);
            _it.Next();

            while (_it.Has && _it.Current is OperatorExpressionItem subOp && subOp.Order() > op.Order())
            {
                value = CallOperator(value, subOp);
            }

            if (op.Name == "+")
            {
                result = operatorOverload.Add(currentValue, value);
            }
            else if (op.Name == "-")
            {
                result = operatorOverload.Subtract(currentValue, value);
            }
            else if (op.Name == "*")
            {
                result = operatorOverload.Multiply(currentValue, value);
            }
            else if (op.Name == "/")
            {
                result = operatorOverload.Divide(currentValue, value);
            }
            else if (op.Name == "%")
            {
                result = operatorOverload.Modulo(currentValue, value);
            }

            return result;
        }
    }
}
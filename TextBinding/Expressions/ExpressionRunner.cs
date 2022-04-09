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
            if (_it.Has && _it.Current.IsCallable)
            {
                result = Call((ValueExpressionItem)_it.Current);
                _it.Next();
            }

            while (_it.Has && _it.Current.IsArithmeticOperator)
            {
                result = CallOperator(result, _it.Current as OperatorExpressionItem);
            }

            Console.WriteLine("Result: " + result);

            return result;
        }

        public object Call(ValueExpressionItem item)
        {
            if (item is SubExpressionItem subExpressionItem)
            {
                return new ExpressionRunner(new List<IExpressionItem>(subExpressionItem.Expression.Items)).Call();
            }else if (item is LiteralExpressionItem value)
            {
                return value.Value;
            }

            throw new InvalidOperationException("Handle expression type: " + item);
        }
        
        

        private object CallOperator(object currentValue, OperatorExpressionItem op)
        {
            DoubleOperatorOverload operatorOverload = new();
            object result = null;
            // Skip current op.
            _it.Next();

            object value = Call(_it.Current as ValueExpressionItem);
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
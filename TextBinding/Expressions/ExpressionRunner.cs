using System;
using System.Collections.Generic;
using TextBinding.Operators;
using TextBinding.Utilities;

namespace TextBinding.Expressions
{
    public class ExpressionRunner
    {
        private readonly Iterator<IExpressionItem> _it;

        private readonly OperatorFactory _operatorFactory =
            new(new[] {typeof(RealOperatorOverload),
                typeof(BooleanOperatorOverload),
                typeof(IntegerOperatorOverload)});

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

            while (_it.Has && _it.Current is OperatorExpressionItem op)
            {
                result = CallOperator(result, op);
            }

            Console.WriteLine("Result: " + result);
            // Console.WriteLine("Stop: " + _it.Current.GetType());

            return result;
        }

        public object Call(IExpressionItem item)
        {
            Console.WriteLine(item.GetType());
            if (item is OperatorExpressionItem op && op.Operator.Type == OperatorType.Unary)
            {
                return CallUnaryOperator(op);
            }
            else if (item is ValueExpressionItem value)
            {
                return CallValue(value);
            }

            throw new InvalidOperationException("Uncallable expression item type: " + item.GetType());
        }

        public object CallValue(ValueExpressionItem item)
        {
            object result;
            if (item is SubExpressionItem subExpressionItem)
            {
                result = new ExpressionRunner(new List<IExpressionItem>(subExpressionItem.Expression.Items)).Call();
            }
            else if (item is IntegerExpressionItem value)
            {
                result = value.Value;
            } else if (item is BooleanExpressionItem boolean)
            {
                result = boolean.Value;
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
            _it.Next();
            object value = CallValue(_it.Current as ValueExpressionItem);
            OperatorMethod? method = _operatorFactory.Find(op.Operator.Name, value.GetType());
          
            if (method != null)
            {
                return method.Call(value);
            }

            string m = $"The are no overload for operator: {op.Name}, Type: {value.GetType()}.";
            throw new InvalidOperationException(m);
        }

        private object CallOperator(object currentValue, OperatorExpressionItem op)
        {
            // Skip current op.
            _it.Next();
            Console.WriteLine("Current: " + _it.Current);
            object value = Call(_it.Current);
            //_it.Next();


            while (_it.Has && _it.Current is OperatorExpressionItem subOp && subOp.Operator.Order > op.Operator.Order)
            {
                value = CallOperator(value, subOp);
            }

            if (op.Name == "&&")
            {
                return (bool) currentValue && (bool) value;
            }
            
            if (op.Name == "||")
            {
                return (bool) currentValue || (bool) value;
            }

            OperatorMethod? method = _operatorFactory.Find(op.Name, currentValue.GetType(), value.GetType());

            if (method != null)
            {
                return method.Call(currentValue, value);
            }

            string m = $"No Overload for operator: {op.Name}, Type: {currentValue.GetType()}, Type: {value.GetType()}";
            throw new InvalidOperationException(m);
        }
    }
}
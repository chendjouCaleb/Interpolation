using System;
using System.Collections.Generic;
using System.Reflection;
using TextBinding.Operators;
using TextBinding.Utilities;

namespace TextBinding.Expressions
{
    public class ExpressionRunner
    {
        private readonly Iterator<IExpressionItem> _it;
        private object Model { get; }

        private readonly OperatorFactory _operatorFactory =
            new(new[] {typeof(RealOperatorOverload),
                typeof(BooleanOperatorOverload),
                typeof(IntegerOperatorOverload)});

        public ExpressionRunner(List<IExpressionItem> items, object model)
        {
            _it = new Iterator<IExpressionItem>(items);
            Model = model;
        }

        public object Call()
        {
            object? result = null;
            if (_it.Has)
            {
                result = Call(_it.Current);
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
            if (item is OperatorExpressionItem op && op.Operator.Type == OperatorType.Unary)
            {
                return CallUnaryOperator(op);
            }
            else if (item is ValueExpressionItem value)
            {
                object result = CallValue(value);
                return CallChain(result);
            }

            throw new InvalidOperationException("Uncallable expression item type: " + item.GetType());
        }

        public object CallValue(ValueExpressionItem item)
        {
            object result;
            if (item is SubExpressionItem subExpressionItem)
            {
                result = new ExpressionRunner(new List<IExpressionItem>(subExpressionItem.Expression.Items), Model).Call();
            }
            else if (item is IntegerExpressionItem value)
            {
                result = value.Value;
            } else if (item is BooleanExpressionItem boolean)
            {
                result = boolean.Value;
            }else if (item is NullExpressionItem)
            {
                result = null;
            }else if (item is PropertyExpressionItem property)
            {
                result = CallProperty(Model, property);
            }else if (item is MethodExpressionItem method)
            {
                result = CallMethod(Model, method);
            }
            else
            {
                throw new InvalidOperationException("Handle expression type: " + item);
            }

            _it.Next();
            return result;
        }

        private object CallChain(object current)
        {
            object result = current;
            while (_it.Has && _it.Current is MemberAccessExpressionItem)
            {
                _it.Next();
                if (_it.Current is PropertyExpressionItem property)
                {
                    result = CallProperty(result, property);
                    _it.Next();
                }else if (_it.Current is MethodExpressionItem method)
                {
                    result = CallMethod(result, method);
                }
            }

            return result;
        }
        

        private object CallProperty(object prev, PropertyExpressionItem property)
        {
            PropertyInfo? info = prev.GetType().GetProperty(property.Name);
            if (info == null)
            {
                throw new InvalidOperationException($"The model dont have '{property.Name}' property ");
            }

            return info.GetValue(prev);   
        }

        private object CallMethod(object prev, MethodExpressionItem method)
        {
            MethodInfo? info = prev.GetType().GetMethod(method.Name);
            if (info == null)
            {
                throw new InvalidOperationException($"The model dont have '{method.Name}' method.");
            }

            return info.Invoke(prev, null);
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
            object value1 = null;

            while (_it.Has && _it.Current is OperatorExpressionItem subOp && subOp.Operator.Order > op.Operator.Order)
            {
                value = CallOperator(value, subOp);
            }

            if (_it.Has && _it.Current is PunctuatorExpressionItem punctuator && punctuator.Value == ":")
            {
                //Skip current ':'.
                _it.Next();
                Console.WriteLine("deux points");
                value1 = Call(_it.Current);
                
                while (_it.Has && _it.Current is OperatorExpressionItem subOp && subOp.Operator.Order > op.Operator.Order)
                {
                    value1 = CallOperator(value1, subOp);
                }
            }

            if (op.Name == "&&")
            {
                return (bool) currentValue && (bool) value;
            }
            
            if (op.Name == "||")
            {
                return (bool) currentValue || (bool) value;
            }

            if (op.Name == "?")
            {
                if ((bool) currentValue)
                {
                    return value;
                }

                return value1!;
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
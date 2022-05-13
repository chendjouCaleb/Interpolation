using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TextBinding.Operators
{
    public class OperatorFactory
    {
        public List<Type> OperatorTypes { get; } = new();

        private List<OperatorMethod> _methods = new();

        private OperatorRules _operatorRules = new();

        public OperatorFactory(IEnumerable<Type> operatorTypes)
        {
            AddOperatorTypes(operatorTypes);
            foreach (Type type in OperatorTypes)
            {
                HandleType(type);
            }
        }

        public OperatorFactory()
        {
        }

        private void AddOperatorTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (!OperatorTypes.Contains(type))
                {
                    OperatorTypes.Add(type);
                }
            }
        }

        private void HandleType(Type type)
        {
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                if (methodInfo.GetCustomAttribute<OperatorMethodAttribute>() != null)
                {
                    AddMethod(methodInfo);  
                }
            }
        }

        public void AddMethod(MethodInfo info)
        {
            if (info == null!)
            {
                OperatorThrowHelper.ThrowNullMethod();
            }

            OperatorMethodAttribute? attr = info.GetCustomAttribute<OperatorMethodAttribute>();

            if (attr == null)
            {
                OperatorThrowHelper.ThrowNonDecoratedMethod(info);
            }
            
            if (!info.IsStatic)
            {
                OperatorThrowHelper.ThrowNonStaticMethod(info);
            }

            if (!info.IsPublic)
            {
                OperatorThrowHelper.ThrowNonPublicMethod(info);
            }

            if (info.ReturnType == typeof(void))
            {
                OperatorThrowHelper.ThrowVoidMethod(info);
            }

            if (info.GetParameters().Length == 0)
            {
                OperatorThrowHelper.ThrowNoParamMethod(info);
            }

            Type type1 = info.GetParameters()[0].ParameterType;
            Type? type2 = null;

            if (info.GetParameters().Length > 1)
            {
                type2 = info.GetParameters()[0].ParameterType;
            }

            CheckOperatorRule(info, attr!.Operator);
            

            OperatorMethod duplicate = First(attr.Operator, type1, type2);
            if (duplicate != null)
            {
                OperatorThrowHelper.ThrowDuplicateSignature(duplicate, info);
            }

            
            OperatorMethod method = new()
            {
                
                Operator = attr!.Operator,
                Method = info,
                ReturnType = info.ReturnType,
                Type = type1,
                OtherType = type2
            };
            
            _methods.Add(method);
        }


        public void CheckOperatorRule(MethodInfo info, string op)
        {
            OperatorRule? rule = _operatorRules.Find(op, info.GetParameters().Length);
            if (rule == null)
            {
                OperatorThrowHelper.ThrowUnknownOperator(info, op);
            }

            if (rule.ReturnType != null && info.ReturnType != rule.ReturnType)
            {
                OperatorThrowHelper.ThrowInvalidReturnType(info, rule);
            }
        }
        public OperatorMethod? First(string op, Type type1, Type? type2)
        {
            return _methods.FirstOrDefault(m => m.Operator == op && m.Type == type1 && m.OtherType == type2);
        }

        public OperatorMethod? Find(string op, Type type)
        {
            return _methods.FirstOrDefault(m => m.Operator == op && m.Type == type);
        }

        public OperatorMethod? Find(string op, Type type, Type otherType)
        {
            return _methods.FirstOrDefault(m => m.Operator == op && m.Type == type && m.OtherType == otherType);
        }

        public bool HasOperator(string @operator, Type type)
        {
            throw new NotImplementedException();
        }

        public bool HasOperator(string @operator, Type type1, Type type2)
        {
            throw new NotImplementedException();
        }
    }
}
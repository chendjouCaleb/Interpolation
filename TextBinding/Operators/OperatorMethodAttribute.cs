using System;

namespace TextBinding.Operators
{
    
    [AttributeUsage(AttributeTargets.Method)]
    public class OperatorMethodAttribute: Attribute
    {
        public string Operator { get; }

        public OperatorMethodAttribute(string @operator)
        {
            Operator = @operator;
        }
    }
}
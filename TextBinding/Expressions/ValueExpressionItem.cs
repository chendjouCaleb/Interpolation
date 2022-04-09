using System;
using System.Text;

namespace TextBinding.Expressions
{
    public abstract class ValueExpressionItem:IExpressionItem
    {
        public string Name { get; set; }

        public ExpressionValueType ExpressionType { get; set; }

        public Type? ReturnType { get; set; }

        public ValueExpressionItem? Field { get; set; }

        public abstract string Serialize();
        
        public override string? ToString()
        {
            StringBuilder builder = new(Serialize());
            ValueExpressionItem? next = Field;
            while (next != null)
            {
                builder.Append('.');
                builder.Append(next.Serialize());
                next = next.Field;
            }
            return builder.ToString();
        }

        public abstract bool IsCallable { get; }
        public bool IsArithmeticOperator => false;
    }
}
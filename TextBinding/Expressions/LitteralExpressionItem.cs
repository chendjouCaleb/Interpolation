using System;

namespace TextBinding.Expressions
{
    public class LiteralExpressionItem:ValueExpressionItem
    {
        public object? Value { get; set; }

        public LiteralExpressionItem(object? value, ExpressionValueType type)
        {
            Value = value;
            ExpressionType = type;
        }

        public override string Serialize()
        {
            if (ExpressionType == ExpressionValueType.String)
            {
                return '"' + Name + '"';
            }

            if (ExpressionType == ExpressionValueType.Null)
            {
                return "null";
            }
            
            if (ExpressionType == ExpressionValueType.Number)
            {
                return Name;
            }

            throw new InvalidOperationException("Cannot handle litteral: " + ExpressionType);
        }
        
        public override bool IsCallable => true;
    }
}
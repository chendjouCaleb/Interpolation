using System;

namespace TextBinding.Expressions
{
    public class IntegerExpressionItem:ValueExpressionItem
    {
        public int Value { get; }

        public IntegerExpressionItem(int value)
        {
            Value = value;
            ExpressionType = ExpressionValueType.Integer;
        }

        public override string Serialize()
        {
            return Value.ToString();
        }
        
        public override bool IsCallable => true;
    }
}
using System;

namespace TextBinding.Expressions
{
    public class BooleanExpressionItem:ValueExpressionItem
    {
        public bool Value { get; }

        public BooleanExpressionItem(bool value)
        {
            Value = value;
            ExpressionType = ExpressionValueType.Boolean;
        }

        public override string Serialize()
        {
            return Value.ToString();
        }
        
        public override bool IsCallable => true;
    }
}
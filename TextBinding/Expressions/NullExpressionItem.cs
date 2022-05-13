using System;

namespace TextBinding.Expressions
{
    public class NullExpressionItem:ValueExpressionItem
    {
        public NullExpressionItem()
        {
            ExpressionType = ExpressionValueType.Null;
        }

        public override string Serialize()
        {
            return "null";
        }
        
        public override bool IsCallable => true;
    }
}
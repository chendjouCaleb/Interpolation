using System;

namespace TextBinding.Expressions
{
    public class PropertyExpressionItem:ValueExpressionItem
    {
        public override string Serialize()
        {
            return Name;
        }
        
        public override bool IsCallable => true;
    }
}
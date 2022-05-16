using System;
using System.Reflection;

namespace TextBinding.Expressions
{
    public class PropertyExpressionItem:MemberExpressionItem
    {
        public PropertyInfo PropertyInfo { get; set; }
        
        
        public override string Serialize()
        {
            return Name;
        }
        
        public override bool IsCallable => true;
    }
}
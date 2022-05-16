using System;

namespace TextBinding.Expressions
{
    public abstract class MemberExpressionItem:ValueExpressionItem
    {
        public string Name { get; set; }
        public Type ReturnType { get; set; }
    }
}
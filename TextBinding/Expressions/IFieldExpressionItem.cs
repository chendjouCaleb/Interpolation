using System;

namespace TextBinding.Expressions
{
    public interface IFieldExpressionItem
    {
        public string Name { get; set; }
        public Type ReturnType { get; set; }
        public IFieldExpressionItem? Next { get; set; }
    }
}
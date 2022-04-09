using System;
using System.Collections.Generic;
using System.Linq;

namespace TextBinding.Expressions
{
    public class BindingExpression:IBindingItem
    {
        private List<IExpressionItem> _items = new();
        public List<IExpressionItem> Items => new (_items);

        public Type ReturnType;
        public void Add(IExpressionItem item)
        {
            _items.Add(item);
        }
        

        public string Execute()
        {
            ExpressionRunner runner = new (_items);
            return runner.Call().ToString();
        }
        
        public override string ToString()
        {
            return string.Join("", _items);
        }
    }
}
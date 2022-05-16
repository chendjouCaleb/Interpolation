using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TextBinding.Expressions
{
    public class BindingExpression : IBindingItem, IEnumerable<IExpressionItem>

    {
        private List<IExpressionItem> _items = new();
        public List<IExpressionItem> Items => new(_items);

        public Type ReturnType;

        public BindingExpression()
        {
        }

        public void Add(IExpressionItem item)
        {
            _items.Add(item);
        }


        public string Execute(object Model)
        {
            ExpressionRunner runner = new(_items, Model);
            object result = runner.Call();
            if (result != null)
            {
                return result.ToString();
            }

            return "null";
        }

        public IExpressionItem First => _items.First();
        public IExpressionItem Last => _items.Last();

        public IEnumerator<IExpressionItem> GetEnumerator() => _items.GetEnumerator();

        public override string ToString()
        {
            return string.Join("", _items);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IExpressionItem this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public T At<T>(int index) where T : IExpressionItem
        {
            if (index < 0)
            {
                index = Count - index;
            }

            return (T)_items[index] ;
        }
    }
}
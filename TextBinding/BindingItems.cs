using System.Collections.Generic;
using System.Text;
using TextBinding.Expressions;

namespace TextBinding
{
    public class BindingItems
    {
        private List<IBindingItem> _items = new();
        public object Model { get; set; }

        public BindingItems(object model)
        {
            Model = model;
        }

        public void AddItem(IBindingItem item)
        {
            _items.Add(item);
        }

        public int Count => _items.Count;

        public string Execute()
        {
            StringBuilder builder = new();
            foreach (IBindingItem item in _items)
            {
                if (item is TextBindingItem textItem)
                {
                    builder.Append(textItem.Text);
                }else if (item is BindingExpression expression)
                {
                    ExpressionRunner runner = new (expression.Items, Model);
                    builder.Append(runner.Call());
                }
            }

            return builder.ToString();
        }
    }
}
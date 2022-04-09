using System.Collections.Generic;
using System.Text;

namespace TextBinding
{
    public class BindingItems
    {
        private List<IBindingItem> _items = new();

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
                builder.Append(item.Execute());
            }

            return builder.ToString();
        }
    }
}
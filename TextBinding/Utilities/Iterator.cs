using System.Collections.Generic;

namespace TextBinding.Utilities
{
    public class Iterator<T>
    {
        private IList<T> _items;

        public Iterator(IList<T> items)
        {
            _items = items;
        }

        public int Index { get; private set; }
        public int Count => _items.Count;
        public int Length => _items.Count;

        public bool HasMore => Index < _items.Count - 1;
        public bool Has => Index < _items.Count;

        public T Current => _items[Index];

        public void Next()
        {
            Index++;
        }
    }
}
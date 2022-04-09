namespace TextBinding
{
    public class TextBindingItem:IBindingItem
    {
        private string _text;

        public TextBindingItem(string text)
        {
            _text = text;
        }

        public string Execute()
        {
            return _text;
        }
    }
}
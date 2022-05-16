namespace TextBinding
{
    public class TextBindingItem:IBindingItem
    {
        public string Text { get;}

        public TextBindingItem(string text)
        {
            Text = text;
        }

    }
}
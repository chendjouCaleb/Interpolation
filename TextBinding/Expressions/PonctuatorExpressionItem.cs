namespace TextBinding.Expressions
{
    public class PunctuatorExpressionItem:IExpressionItem
    {
        public PunctuatorExpressionItem(string value)
        {
            Value = value;
        }

        public string Value { get; }
        public bool IsCallable => false;
    }
}
namespace TextBinding.Expressions
{
    public class MemberAccessExpressionItem:IExpressionItem
    {
        public MemberAccessExpressionItem(bool conditional)
        {
            Conditional = conditional;
        }

        public bool Conditional { get;  }
        public bool IsCallable => false;
    }
}
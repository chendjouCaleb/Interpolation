namespace TextBinding.Expressions
{
    public interface IExpressionItem
    {
        public bool IsCallable { get; }
        bool IsArithmeticOperator { get; }
    }
}
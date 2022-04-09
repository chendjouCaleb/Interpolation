namespace TextBinding
{
    public enum TreeItemType
    {
        Text,
        Call,
        Literal,
        Operator,
        Function,
        Expression,
        Number,
        String
    }

    public enum ExpressionValueType
    {
        Null,
        Method,
        Property,
        SubExpression,
        Number,
        String
    }
}
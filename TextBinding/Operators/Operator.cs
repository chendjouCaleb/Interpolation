namespace TextBinding.Operators
{
    public enum OperatorType
    {
        Unary,
        Binary,
        Ternary
    };
    
    public class Operator
    {
        
        public string Name { get; init; }
        public int Order { get; set; }
        public OperatorType Type { get; init; }

        public static Operator Negation = new (){Name = "!", Type = OperatorType.Unary, Order = 17};
        public static Operator Plus = new (){Name = "+", Type = OperatorType.Unary, Order = 17};
        public static Operator Minus = new (){Name = "-", Type = OperatorType.Unary, Order = 17};
        
        public static Operator MemberAccess = new (){Name = ".", Type = OperatorType.Unary, Order = 17};
        public static Operator InvocationAccess = new (){Name = "(", Type = OperatorType.Unary, Order = 17};
        public static Operator Indexer = new (){Name = ".", Type = OperatorType.Unary, Order = 17};
        public static Operator ConditionalNullMemberAccess = new (){Name = "?.", Type = OperatorType.Unary, Order = 17};
        public static Operator ConditionalNullIndexer = new (){Name = "?.", Type = OperatorType.Unary, Order = 17};
        
        public static Operator Add = new () {Name = "+", Type = OperatorType.Binary, Order = 10};
        public static Operator Subtract = new (){Name = "-", Type = OperatorType.Binary, Order = 10};
        
        public static Operator Div = new () {Name = "/", Type = OperatorType.Binary, Order = 11};
        public static Operator Mul = new () {Name = "*", Type = OperatorType.Binary, Order = 11};
        public static Operator Modulo = new () {Name = "%", Type = OperatorType.Binary, Order = 11};
        
        public static Operator UpperThan = new () {Name = ">", Type = OperatorType.Binary, Order = 9};
        public static Operator LowerThan = new () {Name = "<", Type = OperatorType.Binary, Order = 9};
        public static Operator UpperOrEqualThan = new () {Name = ">=", Type = OperatorType.Binary, Order = 9};
        public static Operator LowerOrEqualThan = new () {Name = "<=", Type = OperatorType.Binary, Order = 9};

        
        public static Operator Equal = new () {Name = "==", Type = OperatorType.Binary, Order = 8};
        public static Operator NotEqual = new () {Name = "!=", Type = OperatorType.Binary, Order = 8};

        public static Operator Or = new () {Name = "||", Type = OperatorType.Binary, Order = 3};
        public static Operator And = new () {Name = "&&", Type = OperatorType.Binary, Order = 3};
        
        public static Operator NullCoalescing = new () {Name = "??", Type = OperatorType.Binary, Order = 2};
        public static Operator Ternary = new () {Name = "?", Type = OperatorType.Binary, Order = 1};
    }
}
using System.Linq;
using TextBinding.Operators;

namespace TextBinding.Expressions
{
    public class OperatorExpressionItem : IExpressionItem
    {
        // private static string[] EqualityOperators = {"==", "!="};
        // private static string[] RelationalOperators = {"<", ">", "<=", ">="};
        // private static string[] AdditiveOperators = {"+", "-"};
        // private static string[] MultiplicativeOperators = {"*", "/", "%"};
        public Operator Operator { get; init; }

        public OperatorExpressionItem(Operator @operator)
        {
            Operator = @operator;
        }

        public string Name => Operator.Name;

        public override string ToString()
        {
            return Operator.Name;
        }

        

        public bool IsCallable => false;
    }
}
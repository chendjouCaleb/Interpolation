using System.Linq;

namespace TextBinding.Expressions
{
    public class OperatorExpressionItem : IExpressionItem
    {
        private string[] EqualityOperators = {"==", "!="};
        private string[] RelationalOperators = {"<", ">", "<=", ">="};
        private string[] AdditiveOperators = {"+", "-"};
        private string[] MultiplicativeOperators = {"*", "/", "%"};
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public int Order()
        {
            if (EqualityOperators.Contains(Name))
            {
                return 8;
            }
            else if (RelationalOperators.Contains(Name))
            {
                return 9;
            }
            else if (AdditiveOperators.Contains(Name))
            {
                return 10;
            }

            if (MultiplicativeOperators.Contains(Name))
            {
                return 11;
            }

            return 1111;
        }

        public bool IsCallable => false;
        public bool IsArithmeticOperator => AdditiveOperators.Contains(Name) || MultiplicativeOperators.Contains(Name);
    }
}
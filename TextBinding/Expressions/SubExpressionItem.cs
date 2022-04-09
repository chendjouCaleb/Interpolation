using System.Text;

namespace TextBinding.Expressions
{
    public class SubExpressionItem:ValueExpressionItem
    {
        public readonly BindingExpression Expression = new ();
        
        public override string Serialize()
        {
            StringBuilder builder = new ('(' + Expression.ToString() + ')');
            return builder.ToString();
        }
        
        public override bool IsCallable => true;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace TextBinding.Expressions
{
    public class MethodExpressionItem:ValueExpressionItem
    {
        public List<BindingExpression> ParamExpressions { get; } = new();

        public override string Serialize()
        {
            StringBuilder builder = new (Name + '(');

            builder.Append(string.Join(", ", ParamExpressions.Select(p => p.ToString())));
            
            
            builder.Append(')');

            return builder.ToString();
        }

        public override bool IsCallable => true;
    }
}
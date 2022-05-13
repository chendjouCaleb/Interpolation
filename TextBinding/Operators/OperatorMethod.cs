using System;
using System.Reflection;
using System.Text;

namespace TextBinding.Operators
{
    public class OperatorMethod
    {
        public Type ReturnType { get; set; }

        public MethodInfo Method { get; set; }

        public Type Type { get; set; }

        public Type? OtherType { get; set; }

        public string Operator { get; set; }

        public object Call(object a)
        {
            return Method.Invoke(null, new[] {a});
        }

        public object Call(object a, object b)
        {
            return Method.Invoke(null, new[] {a, b});
        }

        public override string ToString()
        {
            StringBuilder builder = new("[ operator: ");
            builder.Append(Operator);
            builder.Append(", Name: ");
            builder.Append(Method.Name);

            builder.Append(", LeftType: ");
            builder.Append(Type.Name);

            if (OtherType != null)
            {
                builder.Append(", RightType: ");
                builder.Append(OtherType.Name);
            }

            builder.Append(" ]");

            return builder.ToString();
        }
    }
}
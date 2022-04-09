using System;

namespace TextBinding.Operators
{
    public interface IArithmeticOperatorOverload
    {
        public Type LeftType { get; }
        public Type RightType { get; }

        public object Add(object a, object b);

        public object Subtract(object a, object b);

        public object Multiply(object a, object b);

        public object Divide(object a, object b);

        public object Modulo(object a, object b);

    }
}
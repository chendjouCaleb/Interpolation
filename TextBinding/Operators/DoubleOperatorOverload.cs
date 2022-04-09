using System;

namespace TextBinding.Operators
{
    public class DoubleOperatorOverload:IArithmeticOperatorOverload
    {
        public Type LeftType => typeof(double);
        public Type RightType => typeof(double);
        public object Add(object a, object b)
        {
            return (double) a + (double) b;
        }

        public object Subtract(object a, object b)
        {
            return (double) a - (double) b;
        }

        public object Multiply(object a, object b)
        {
            return (double) a * (double) b;
        }

        public object Divide(object a, object b)
        {
            return (double) a / (double) b;
        }

        public object Modulo(object a, object b)
        {
            return (double) a % (double) b;
        }
    }
}
using System;

namespace TextBinding.Operators
{
    public class IntegerOperatorOverload
    {
        [OperatorMethod("+")]
        public static int Plus(int a) => +a;
        
        [OperatorMethod("-")]
        public static int Minus(int a) => -a;

        [OperatorMethod("+")]
        public static int Add(int a, int b) => a + b;
        
        [OperatorMethod("-")]
        public static int Subtract(int a, int b) => a - b;
        
        [OperatorMethod("*")]
        public static int Multiply(int a, int b) => a * b;
        
        [OperatorMethod("/")]
        public static int Divide(int a, int b) => a / b;
        
        [OperatorMethod("%")]
        public static int Modulo(int a, int b) => a % b;
        
        [OperatorMethod("==")]
        public static bool Equal(int a, int b) => Math.Abs(a - b) < 0.0000001;
        
        [OperatorMethod("!=")]
        public static bool NotEqual(int a, int b) => Math.Abs(a - b) > 0.0000001;

        [OperatorMethod(">")]
        public static bool UpperThan(int a, int b) => a > b;
        
        [OperatorMethod(">=")]
        public static bool UpperOrEqualThan(int a, int b) => a >= b;
        
        [OperatorMethod("<")]
        public static bool LowerThan(int a, int b) => a < b;
        
        [OperatorMethod("<=")]
        public static bool LowerOrEqualThan(int a, int b) => a <= b;
    }
}
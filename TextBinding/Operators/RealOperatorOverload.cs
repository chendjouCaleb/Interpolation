using System;

namespace TextBinding.Operators
{
    public class RealOperatorOverload
    {
        [OperatorMethod("+")]
        public static double Plus(double a) => +a;
        
        [OperatorMethod("-")]
        public static double Minus(double a) => -a;

        [OperatorMethod("+")]
        public static double Add(double a, double b) => a + b;
        
        [OperatorMethod("-")]
        public static double Subtract(double a, double b) => a - b;
        
        [OperatorMethod("*")]
        public static double Multiply(double a, double b) => a * b;
        
        [OperatorMethod("/")]
        public static double Divide(double a, double b) => a / b;
        
        [OperatorMethod("%")]
        public static double Modulo(double a, double b) => a % b;
        
        [OperatorMethod("==")]
        public static bool Equal(double a, double b) => Math.Abs(a - b) < 0.0000001;
        
        [OperatorMethod("!=")]
        public static bool NotEqual(double a, double b) => Math.Abs(a - b) > 0.0000001;

        [OperatorMethod(">")]
        public static bool UpperThan(double a, double b) => a > b;
        
        [OperatorMethod(">=")]
        public static bool UpperOrEqualThan(double a, double b) => a >= b;
        
        [OperatorMethod("<")]
        public static bool LowerThan(double a, double b) => a < b;
        
        [OperatorMethod("<=")]
        public static bool LowerOrEqualThan(double a, double b) => a <= b;

    }
}
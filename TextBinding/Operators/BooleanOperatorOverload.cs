namespace TextBinding.Operators
{
    public class BooleanOperatorOverload
    {
        [OperatorMethod("==")]
        public static bool Equal(bool a, bool b) => a == b;
        
        [OperatorMethod("!=")]
        public static bool NotEqual(bool a, bool b) => a != b;
        
    }
}
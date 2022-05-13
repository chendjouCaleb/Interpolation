using System.Reflection;

namespace TextBinding.Operators
{
    public static class OperatorThrowHelper
    {
        public static void ThrowNonPublicMethod(MethodInfo info)
        {
            string m = $"Operator Method: {MethodToString(info)} should be public.";
            throw new OperatorMethodException(OperatorMethodError.NonPublicMethod, m);
        }
        
        public static void ThrowNonStaticMethod(MethodInfo info)
        {
            string m = $"Operator Method: {MethodToString(info)} should be static.";
            throw new OperatorMethodException(OperatorMethodError.NonStaticMethod, m);
        }

        public static void ThrowNullMethod()
        {
            var m = "Cannot add null method as operator overload.";
            throw new OperatorMethodException(OperatorMethodError.NullMethod, m);
        }

        public static void ThrowNonDecoratedMethod(MethodInfo info)
        {
            var attrName = typeof(OperatorMethodAttribute).FullName;
            var m = $"Operator Method: {MethodToString(info)} should be decorated by ${attrName}.";
            throw new OperatorMethodException(OperatorMethodError.NonDecoratedMethod, m);
        }
        
        public static void ThrowVoidMethod(MethodInfo info)
        {
            var m = $"Operator Method: {MethodToString(info)} should not return void.";
            throw new OperatorMethodException(OperatorMethodError.VoidReturn, m);
        }
        
        public static void ThrowNoParamMethod(MethodInfo info)
        {
            var m = $"Operator Method: {MethodToString(info)} should have at least one param.";
            throw new OperatorMethodException(OperatorMethodError.ZeroParam, m);
        }

        public static void ThrowDuplicateSignature(OperatorMethod method, MemberInfo duplicate)
        {
            var m = $"Duplication operator signature. Method 1 : {MethodToString(method.Method)}, " +
                    $"Method2 : {MethodToString(duplicate)}.";
            throw new OperatorMethodException(OperatorMethodError.DuplicationOperatorSignature, m);
        }

        public static void ThrowRequireOneParam(MemberInfo info, OperatorRule rule)
        {
            string m = $"Invalid Method: {MethodToString(info)}. The operator: {rule.Name} has only one operand.";
            throw new OperatorMethodException(OperatorMethodError.RequireOneOperand, m);
        }
        
        public static void ThrowRequireTwoParam(MemberInfo info, OperatorRule rule)
        {
            string m = $"Invalid Method: {MethodToString(info)}. The operator: {rule.Name} has two operands.";
            throw new OperatorMethodException(OperatorMethodError.RequireTwoOperand, m);
        }
        
        public static void ThrowUnknownOperator(MethodInfo info, string op)
        {
            int count = info.GetParameters().Length;
            string m = $"Invalid Method: {MethodToString(info)}. The operator: {op}, with {count} parameters is unknown.";
            throw new OperatorMethodException(OperatorMethodError.UnknownOperator, m);
        }
        
        

        public static void ThrowInvalidReturnType(MemberInfo info, OperatorRule rule)
        {
            string m = $"Error for method: {MethodToString(info)}. Return type of operator: ${rule.Name} " +
                       $"should be: {rule.ReturnType}.";
            throw new OperatorMethodException(OperatorMethodError.InvalidReturnType, m);
        }
        

        public static string MethodToString(MemberInfo info)
        {
            return $"[Name: {info.Name}, Type: {info.DeclaringType.FullName}]";
        }
    }
}
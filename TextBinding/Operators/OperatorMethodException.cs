using System;
using System.Runtime.Serialization;

namespace TextBinding.Operators
{
    public enum OperatorMethodError
    {
        NullMethod,
        NonDecoratedMethod,
        NonStaticMethod,
        NonPublicMethod,
        UnknownOperator,
        DuplicationOperatorSignature,
        VoidReturn,
        ZeroParam,
        InvalidReturnType,
        InvalidParamCount,
        RequireTwoOperand,
        RequireOneOperand
    }
    public class OperatorMethodException:ApplicationException
    {
        public OperatorMethodError Error { get;  }
        public OperatorMethodException(OperatorMethodError error)
        {
            Error = error;
        }

        public OperatorMethodException(OperatorMethodError error, string? message) : base(message)
        {
            Error = error;
        }
    }
}
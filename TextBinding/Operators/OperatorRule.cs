using System;
using System.Data;

namespace TextBinding.Operators
{
    public class OperatorRule
    {
        public int Id { get; init; }

        public int? Inverse { get; set; }
        public string Name { get; init; }
        public Type? ReturnType { get; init; }
        public int ParamCount { get; init; } 

        public static OperatorRule operator |(OperatorRule rule1, OperatorRule rule2) => rule1;
    }
}
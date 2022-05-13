using System;
using System.Collections.Generic;
using System.Linq;

namespace TextBinding.Operators
{
    public class OperatorList
    {
        private readonly List<Operator> _operators;

        public OperatorList()
        {
            _operators = new(new[]
            {
                Operator.Plus,
                Operator.Minus,
                Operator.Add, Operator.Subtract, Operator.Div, Operator.Mul
            });
        }

        public Operator GetOperator(string name, OperatorType type)
        {
            Operator? @operator = _operators.Find(o => o.Name == name && o.Type == type);

            if (@operator == null)
            {
                throw new InvalidOperationException($"Operator [Name={name}], Type={type}] not found.");
            }

            return @operator;
        }
    }
}
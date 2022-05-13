using System.Collections;
using System.Collections.Generic;

namespace TextBinding.Operators
{
    public class OperatorRules: IEnumerable<OperatorRule>
    {
        private List<OperatorRule> _rules = new( new []
        {
            new OperatorRule {Id = 1, Name = "+", ParamCount = 1},
            new OperatorRule {Id = 2, Name = "-", ParamCount = 1},
            new OperatorRule {Id = 3, Name = "!", ParamCount = 1},

            new OperatorRule {Id = 4, Name = "+", ParamCount = 2},
            new OperatorRule {Id = 5, Name = "-", ParamCount = 2},
            new OperatorRule {Id = 6, Name = "*", ParamCount = 2},
            new OperatorRule {Id = 7, Name = "/", ParamCount = 2},
            new OperatorRule {Id = 8, Name = "%", ParamCount = 2},
            
            new OperatorRule {Id = 9, Inverse=10, Name = "true", ParamCount = 1, ReturnType = typeof(bool)},
            new OperatorRule {Id = 10, Inverse = 9, Name = "false", ParamCount = 1, ReturnType = typeof(bool)},
            
            new OperatorRule {Id = 11, Inverse  = 12, Name = "<", ParamCount = 2},
            new OperatorRule {Id = 12, Inverse  = 11, Name = ">", ParamCount = 2},
            
            new OperatorRule {Id = 13, Inverse  = 14, Name = "<=", ParamCount = 2},
            new OperatorRule {Id = 14, Inverse  = 13, Name = ">=", ParamCount = 2},
            
            new OperatorRule {Id = 15, Inverse  = 16, Name = "==", ParamCount = 2},
            new OperatorRule {Id = 16, Inverse  = 15, Name = "!=", ParamCount = 2},
            
            new OperatorRule {Id = 17, Name = "&", ParamCount = 2},
            new OperatorRule {Id = 18, Name = "|", ParamCount = 2},
            new OperatorRule {Id = 19, Name = "^", ParamCount = 2},
        });

        public IEnumerator<OperatorRule> GetEnumerator() => _rules.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public OperatorRule? Find(string name, int paramCount)
        {
            return _rules.Find(o => o.Name == name && o.ParamCount == paramCount);
        }

        public List<OperatorRule> FindAll(string name)
        {
            return _rules.FindAll(r => r.Name == name);
        }
    }
}
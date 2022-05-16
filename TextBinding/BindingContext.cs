using TextBinding.Operators;

namespace TextBinding
{
    public class BindingContext
    {
        public object Target { get; set; }
        public OperatorRules OperatorRules { get; set; }
        public OperatorFactory OperatorFactory { get; set; }
    }
}
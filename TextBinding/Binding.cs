using System;
using System.Collections.Generic;

namespace TextBinding
{
    public class Binding<T>
    {
        private Dictionary<string, BindingTemplate> _templates = new();
        public T Value { get; set; }

        public void AddText(string name, string template)
        {
            throw new NotImplementedException();
        }

        private void CompileText(string text)
        {
            Tokenizer tokenizer = new Tokenizer(text);
        }

        public void Compile()
        {
            throw new NotImplementedException();
        }

        public string Get(string templateName)
        {
            throw new NotImplementedException();
        }
    }
}
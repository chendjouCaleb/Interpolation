using System;
using System.Collections.Generic;
using TextBinding.Expressions;
using TextBinding.Utilities;

namespace TextBinding
{
    public class BindingItemsBuilder
    {
        private Iterator<Token> _it;
        public BindingItems Items { get; private set; }

        public BindingItemsBuilder(IList<Token> tokens)
        {
            _it = new (tokens);
            Items = new BindingItems();
        }


        public void Build()
        {
            while (_it.Has)
            {
                if (_it.Current.Type == TokenType.Text)
                {
                    Items.AddItem(new TextBindingItem(_it.Current.Value));
                    _it.Next();
                }
                else if (_it.Current.Type == TokenType.Open)
                {
                   
                    BindingExpression expression = new ();
                    ExpressionBuilder builder = new (_it);
                    builder.Build(expression);
                    Items.AddItem(expression);

                    if (_it.Has && _it.Current.Type == TokenType.Close)
                    {
                        _it.Next();
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unknown token type: " + _it.Current);
                }
            }
        }
        
        
    }
}
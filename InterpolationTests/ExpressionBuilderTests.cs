using System;
using NUnit.Framework;
using TextBinding;
using TextBinding.Expressions;
using TextBinding.Operators;
using TextBinding.Utilities;

namespace InterpolationTests
{
    public class ExpressionBuilderTests
    {
        [Test]
        public void Create()
        {
            ExpressionBuilder builder = new(GetTokens("{{1+1}}"));
        }


        [Test]
        public void TakeInteger()
        {
            ExpressionBuilder builder = new(GetTokens("{{1}}"));
            IExpressionItem item = builder.Take(1).Last;

            Assert.True(item is IntegerExpressionItem);
        }

        [Test]
        public void TakeUnaryOperator()
        {
            ExpressionBuilder builder = new(GetTokens("{{+1}}"));
            var item = builder.Build()[0];

            Assert.AreEqual(typeof(OperatorExpressionItem), item.GetType());
            OperatorExpressionItem op = (item as OperatorExpressionItem)!;
            Assert.NotNull(op);
            Assert.AreEqual("+", op.Operator.Name);
            Assert.AreEqual(OperatorType.Unary, op.Operator.Type);
        }

        [Test]
        [TestCase("{{+1+1}}", "+", 2)]
        [TestCase("{{+1-1}}", "-", 2)]
        [TestCase("{{+1%1}}", "%", 2)]
        [TestCase("{{+1*1}}", "*", 2)]
        [TestCase("{{+1/1}}", "/", 2)]
        [TestCase("{{true && false}}", "&&", 1)]
        [TestCase("{{true || false}}", "||", 1)]
        //[TestCase("{{x ?? 10}}", "??", 1)]
        [TestCase("{{x == 10}}", "==", 1)]
        [TestCase("{{x != 10}}", "!=", 1)]
        [TestCase("{{x < y}}", "<", 1)]
        [TestCase("{{x > y}}", ">", 1)]
        [TestCase("{{x <= y}}", "<=", 1)]
        [TestCase("{{x > y}}", ">", 1)]
        [TestCase("{{x >= y}}", ">=", 1)]
       [TestCase("{{x ? 10:5}}", "?", 1)]
        public void TakeBinaryOperator(string text, string name, int index)
       {
           
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<OperatorExpressionItem>(index);

            Assert.NotNull(item);
            Assert.AreEqual(name, item.Operator.Name);
            Assert.AreEqual(OperatorType.Binary, item.Operator.Type);
        }


        [Test]
        [TestCase("{{x.name}}",  false)]
        [TestCase("{{x?.name}}", true)]
        [TestCase("{{1.name}}", false)]
        [TestCase("{{1?.name}}",  true)]
        [TestCase("{{(x).name}}",  false)]
        public void TakeMemberAccess(string text, bool conditional)
        {
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<MemberAccessExpressionItem>(1);

            Assert.NotNull(item);
            Assert.AreEqual(conditional, item.Conditional);
        }

        [Test]
        [TestCase("{{true}}", true)]
        [TestCase("{{false}}", false)]
        public void TakeBoolean(string text, bool value)
        {
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<BooleanExpressionItem>(0);

            Assert.NotNull(item);
            Assert.AreEqual(value, item.Value);
            Assert.AreEqual(ExpressionValueType.Boolean, item.ExpressionType);
        }

        [Test]
        public void TakeNullValue()
        {
            ExpressionBuilder builder = new(GetTokens("{{null}}"));
            var item = builder.Build().At<NullExpressionItem>(0);

            Assert.NotNull(item);
            Assert.AreEqual(ExpressionValueType.Null, item.ExpressionType);
        }

        [Test]
        public void TakeMember()
        {
            string text = "{{value}}";
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<PropertyExpressionItem>(0);

            Assert.NotNull(item);
            Assert.AreEqual("value", item.Name);
            Assert.AreEqual(ExpressionValueType.Property, item.ExpressionType);
        }

        [Test]
        public void TakeMethod()
        {
            string text = "{{value()}}";
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<MethodExpressionItem>(0);
        
            Assert.NotNull(item);
            Assert.AreEqual("value", item.Name);
            Assert.AreEqual(ExpressionValueType.Method, item.ExpressionType);
        }
        
        [Test]
        public void TakeMethodWithParams()
        {
            string text = "{{value(1+1, 3)}}";
            ExpressionBuilder builder = new(GetTokens(text));
            var item = builder.Build().At<MethodExpressionItem>(0);
        
            Assert.NotNull(item);
            Assert.AreEqual("value", item.Name);
            Assert.AreEqual(ExpressionValueType.Method, item.ExpressionType);
            Assert.AreEqual(2, item.ParamExpressions.Count);
        }
        
        
        
        

        private Iterator<Token> GetTokens(string text)
        {
            Tokenizer tokenizer = new(text);
            tokenizer.Tokenize();

            return new(tokenizer.Tokens.ToList());
        }
    }
}
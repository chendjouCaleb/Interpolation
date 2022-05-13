using NUnit.Framework;
using TextBinding;

namespace InterpolationTests
{
    public class TokenizerTests
    {
        [Test]
        public void TextBeforeOpenShouldBeTakenAsText()
        {
            string text = "1+1-3+(1.c)";
            Tokenizer tokenizer = new (text + "{{");
            tokenizer.Tokenize();
            
            Assert.AreEqual(2, tokenizer.Tokens.Count);
            Token token = tokenizer.Tokens[0];
            
            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
        }


        [Test]
        public void TextAfterCloseShouldBeTakenAsText()
        {
            string text = "1+1-3+(1.c)";
            string expression = " Welcome! {{1+3*3}}";
            Tokenizer tokenizer = new (expression + text);
            tokenizer.Tokenize();

            Token token = tokenizer.Tokens.Last;
            
            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
        }
        
        
        [Test]
        public void SingleOpenBrace_ShouldBeTakenAsText()
        {
            string text = " Welcome! {";
            Tokenizer tokenizer = new ( text);
            tokenizer.Tokenize();

            Token token = tokenizer.Tokens.Last;
            
            Assert.AreEqual("{", token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
        }

        [Test]
        public void OpenBraces_ShouldOpen()
        {
            string text = " Welcome! {{ 1+1 }}";
            Tokenizer tokenizer = new (text);
            tokenizer.Take();
            tokenizer.Take();

            Token token = tokenizer.Tokens.Last;
            
            Assert.True(tokenizer.IsOpen);
            Assert.AreEqual("{{", token.Value);
            Assert.AreEqual(TokenType.Open, token.Type);
        }
        
        [Test]
        public void CloseBraces_ShouldClose()
        {
            string text = " Welcome! {{ 1 }} text";
            Tokenizer tokenizer = new (text);
            tokenizer.Take(4);

            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(4, tokenizer.Tokens.Count);
            Assert.False(tokenizer.IsOpen);
            Assert.AreEqual("}}", token.Value);
            Assert.AreEqual(TokenType.Close, token.Type);
        }

        [Test]
        [TestCase("{{+}}", "+")]
        [TestCase("{{/}}", "/")]
        [TestCase("{{%}}", "%")]
        [TestCase("{{*}}", "*")]
        [TestCase("{{.}}", ".")]
        [TestCase("{{++}}", "++")]
        [TestCase("{{?.}}", "?.")]
        [TestCase("{{+?.+}}", "+?.+")]
        public void TakeOperatorAtOperator(string text, string op)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);

            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(op, token.Value);
            Assert.AreEqual(TokenType.Operator, token.Type);
        }
        
        [Test]
        [TestCase("{{(}}", "(", TokenType.ParenthesisOpen)]
        [TestCase("{{((}}", "(", TokenType.ParenthesisOpen)]
        [TestCase("{{)}}", ")", TokenType.ParenthesisClose)]
        [TestCase("{{))}}", ")", TokenType.ParenthesisClose)]
        public void TakeParenthesisAtParenthesis(string text, string op, TokenType type)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);

            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(op, token.Value);
            Assert.AreEqual(type, token.Type);
        }
        
        [Test]
        [TestCase("{{ [ }}", "[")]
        [TestCase("{{ [[ }}", "[")]
        [TestCase("{{ ] }}", "]")]
        [TestCase("{{ ]] }}", "]")]
        public void TakeBracketAtBracket(string text, string op)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);

            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(op, token.Value);
            Assert.AreEqual(TokenType.Brackets, token.Type);
        }
        
        
        [Test]
        [TestCase("{{ , }}", ",")]
        [TestCase("{{ ; }}", ";")]
        [TestCase("{{ : }}", ":")]
        public void TakePunctuationAtPunctuator(string text, string op)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);

            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(op, token.Value);
            Assert.AreEqual(TokenType.Punctuator, token.Type);
        }


        [Test]
        [TestCase("{{0x23}}", "0x23", TokenType.Hexadecimal)]
        [TestCase("{{0x}}", "0x", TokenType.Hexadecimal)]
        [TestCase("{{0o23}}", "0o23", TokenType.Octal)]
        [TestCase("{{0o}}", "0o", TokenType.Octal)]
        [TestCase("{{0b111}}", "0b111", TokenType.Binary)]
        [TestCase("{{0b}}", "0b", TokenType.Binary)]
        [TestCase("{{0d123}}", "0d123", TokenType.Decimal)]
        [TestCase("{{0123}}", "0123", TokenType.Decimal)]
        [TestCase("{{0}}", "0", TokenType.Decimal)]
        public void TakeIntegerAtZero(string text, string val, TokenType type)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);
            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(val, token.Value);
            Assert.AreEqual(type, token.Type);
        }
        
        [Test]
        [TestCase("{{10}}", "10")]
        [TestCase("{{101}}", "101")]
        [TestCase("{{90000}}", "90000")]
        public void TakeRealAtDigitOtherThanZero(string text, string val)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);
            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(val, token.Value);
            Assert.AreEqual(TokenType.Integer, token.Type);
        }
        
        
        [TestCase("{{\"welcome !\"}}", "\"welcome !\"")]
        public void TakeString(string text, string val)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);
            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(val, token.Value);
            Assert.AreEqual(TokenType.String, token.Type);
        }
        
        
        [TestCase("{{'c'}}", "'c'")]
        public void TakeChar(string text, string val)
        {
            Tokenizer tokenizer = new (text);
            tokenizer.Take(2);
            Token token = tokenizer.Tokens.Last;
            Assert.AreEqual(val, token.Value);
            Assert.AreEqual(TokenType.Char, token.Type);
        }
    }
}
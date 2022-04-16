using NUnit.Framework;
using TextBinding;
using TextBinding.Utilities;

namespace InterpolationTests
{
    public class TokenizerTests
    {
        

        [SetUp]
        public void BeforeEach()
        {
        }

        [Test]
        public void Create()
        {
            string text = "Welcome {{1+1}}";
            Tokenizer tokenizer = new(text);

            Assert.AreEqual(text, tokenizer.Iterator.Text);
            Assert.AreEqual(TokenIndex.Zero, tokenizer.Iterator.Index);
        }

        [Test]
        public void TakeText()
        {
            string text = "Hello word!";
            Tokenizer tokenizer = new(text);
            Token token = tokenizer.TakeText();

            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(new TokenIndex(text.Length, 0, text.Length), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }
        
        [Test]
        public void TakeExpressionWithoutOpenBraceAsText()
        {
            string text = "1+3}}";
            Tokenizer tokenizer = new(text);
            Token token = tokenizer.TakeText();

            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(new TokenIndex(text.Length, 0, text.Length), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }

        [Test]
        public void TakeTextShouldStopBeforeExpressionStart()
        {
            string text = "Hello word!";
            string expression = "{{ 1+1 }}";
            Tokenizer tokenizer = new(text + expression);
            Token token = tokenizer.TakeText();

            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(new TokenIndex(text.Length, 0, text.Length), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }


        [Test]
        public void TakeExpressionOpen()
        {
            string expression = "{{ 1+1 }}";
            Tokenizer tokenizer = new(expression);
            Token? token = tokenizer.TryTakeOpen();

            Assert.NotNull(token);
            Assert.AreEqual("{{", token?.Value);
            Assert.AreEqual(TokenType.Open, token?.Type);
            Assert.AreEqual(TokenIndex.Zero, token?.StartIndex);

            Assert.AreEqual(new TokenIndex(2, 0, 2), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }

        [Test]
        public void TakeExpressionOpen_WithOneBrace_ShouldReturnTextToken()
        {
            string expression = "{ 1+1 }}";
            Tokenizer tokenizer = new(expression);
            Token token = tokenizer.TryTakeOpen();

            Assert.AreEqual("{", token.Value);
            Assert.AreEqual(TokenType.Text, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(new TokenIndex(1, 0, 1), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }

        [Test]
        public void TakeExpressionClose()
        {
            string expression = "}}";
            Tokenizer tokenizer = new(expression);
            Token? token = tokenizer.TryTakeClose();

            Assert.NotNull(token);
            Assert.AreEqual("}}", token?.Value);
            Assert.AreEqual(TokenType.Close, token?.Type);
            Assert.AreEqual(TokenIndex.Zero, token?.StartIndex);

            Assert.AreEqual(new TokenIndex(2, 0, 2), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }

        [Test]
        public void TryTakeExpressionWithOneBraceShouldThrow()
        {
            string expression = "}";
            Tokenizer tokenizer = new(expression);
            ExpressionException ex = Assert.Throws<ExpressionException>(() => tokenizer.TryTakeClose())!;
            
            Assert.AreEqual(ExpressionError.UnknownToken, ex.Error);
            Assert.AreEqual(TokenIndex.Zero, ex.Index);
        }


        [Test]
        public void TakeId()
        {
            string[] texts = {"text", "_text", "_text11", "_32", "_e_a1", "true", "false"};

            foreach (string text in texts)
            {
                Tokenizer tokenizer = new(text);
                Token? token = tokenizer.TakeIdentifier();
                Assert.NotNull(token);
                Assert.AreEqual(text, token.Value);
                Assert.AreEqual(TokenType.Id, token.Type);
                Assert.AreEqual(TokenIndex.Zero, token?.StartIndex);

                Assert.AreEqual(new TokenIndex(text.Length, 0, text.Length), tokenizer.Index);
                Assert.AreEqual(tokenizer.Tokens.Count, 1);
            }
        }

        [Test]
        public void TakeSingleOperator()
        {
            string op = "[]().,:+-*/%!|&=<>?";
            foreach (char c in op)
            {
                string text = c.ToString();
                Tokenizer tokenizer = new(text);
                Token token = tokenizer.TakeOperator();
                Assert.NotNull(token);
                Assert.AreEqual(text, token.Value);
                Assert.AreEqual(TokenType.Operator, token.Type);
                Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

                Assert.AreEqual(TokenIndex.To(text.Length), tokenizer.Index);
                Assert.AreEqual(tokenizer.Tokens.Count, 1);
            }
        }
        
        [Test]
        public void TakeMultiCharOperator()
        {
            string[] ops = {"[", "?.", "==", "<=", "<=", "&&", "||", "(("};
            foreach (string text in ops)
            {
                Tokenizer tokenizer = new(text);
                Token token = tokenizer.TakeOperator();
                Assert.NotNull(token);
                Assert.AreEqual(text, token.Value);
                Assert.AreEqual(TokenType.Operator, token.Type);
                Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

                Assert.AreEqual(TokenIndex.To(text.Length), tokenizer.Index);
                Assert.AreEqual(tokenizer.Tokens.Count, 1);
            }
        }
        
        
        [Test]
        public void TakeOperatorShouldStopAtNotOperatorChar()
        {
            string[] ops = {"?.k", "==", "<=1", "<=e", "&&_", "||ç", "((a"};
            foreach (string text in ops)
            {
                Tokenizer tokenizer = new(text);
                Token token = tokenizer.TakeOperator();
                Assert.NotNull(token);
                Assert.AreEqual(text.Substring(0, 2), token.Value);
                Assert.AreEqual(TokenType.Operator, token.Type);
                Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

                Assert.AreEqual(TokenIndex.To(2), tokenizer.Index);
                Assert.AreEqual(tokenizer.Tokens.Count, 1);
            }
        }
        
      
        [Test]
        public void TakeInteger()
        {
            string[] texts = {"12", "132", "3212"};
            foreach (string text in texts)
            {
                Tokenizer tokenizer = new(text);
                Token? token = tokenizer.TakeInteger();

                Assert.NotNull(token);
                Assert.AreEqual(text, token.Value);
                Assert.AreEqual(TokenType.Integer, token.Type);
                Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

                Assert.AreEqual(TokenIndex.To(text.Length), tokenizer.Index);
                Assert.AreEqual(tokenizer.Tokens.Count, 1);
            }
        }


        [Test]
        public void TakeDecimal()
        {
            string text = "01234";
            Tokenizer tokenizer = new(text);
            Token token = tokenizer.TakeZeroNumber();

            Assert.NotNull(token);
            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(TokenType.Decimal, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(TokenIndex.To(text.Length), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }
        
        
        
        [Test]
        [TestCase("0o1234", "0o1234")]
        [TestCase("0o123489", "0o1234")]
        [TestCase("0o1234-", "0o1234")]
        public void TakeOctal(string inputText, string tokenValue)
        {
            Tokenizer tokenizer = new(inputText);
            Token token = tokenizer.TakeZeroNumber();

            AssertFirstToken(token, tokenValue, tokenizer, TokenType.Octal);
        }
        
        [Test]
        [TestCase("0x1234", "0x1234")]
        [TestCase("0x123489", "0x123489")]
        [TestCase("0xe1234A", "0xe1234A")]
        [TestCase("0x1234Z", "0x1234")]
        [TestCase("0x1e2E3Fef4g", "0x1e2E3Fef4")]
        [TestCase("0xeee-", "0xeee")]
        public void TakeHexadecimal(string inputText, string tokenValue)
        {
            Tokenizer tokenizer = new(inputText);
            Token token = tokenizer.TakeZeroNumber();

            AssertFirstToken(token, tokenValue, tokenizer, TokenType.Hexadecimal);
        }
        
        
        [Test]
        [TestCase("0b10114", "0b1011")]
        [TestCase("0b00012", "0b0001")]
        [TestCase("0b0001a", "0b0001")]
        [TestCase("0b0001", "0b0001")]
        [TestCase("0b0001-", "0b0001")]
        public void TakeBinary(string inputText, string tokenValue)
        {
            Tokenizer tokenizer = new(inputText);
            Token token = tokenizer.TakeZeroNumber();

            AssertFirstToken(token, tokenValue, tokenizer, TokenType.Binary);
        }

        
        [Test]
        [TestCase("'a'b", "'a'")]
        public void TakeCharLiteral(string inputText, string tokenValue)
        {
            Tokenizer tokenizer = new(inputText);
            Token token = tokenizer.TakeChar();

            AssertFirstToken(token, tokenValue, tokenizer, TokenType.Char);
        }

        [Test]
        [TestCase("\"welcome\"+", "\"welcome\"")]
        public void TakeStringLiteral(string inputText, string tokenValue)
        {
            Tokenizer tokenizer = new(inputText);
            Token token = tokenizer.TakeString();

            AssertFirstToken(token, tokenValue, tokenizer, TokenType.String);   
        }
        
        private void AssertFirstToken(Token? token, string text, Tokenizer tokenizer, TokenType type)
        {
            Assert.NotNull(token);
            Assert.AreEqual(text, token.Value);
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(TokenIndex.Zero, token.StartIndex);

            Assert.AreEqual(TokenIndex.To(text.Length), tokenizer.Index);
            Assert.AreEqual(tokenizer.Tokens.Count, 1);
        }
    }
}
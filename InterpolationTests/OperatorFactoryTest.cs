using NUnit.Framework;
using TextBinding.Operators;

namespace InterpolationTests
{
    public class OperatorFactoryTest
    {
        [Test]
        public void Ctor()
        {
            var factory = new OperatorFactory(new[] {typeof(CorrectOverload)});

            Assert.NotNull(factory);
        }

        // [Test]
        // public void Ctor_ShouldNotHad_TwoOverloadClass()
        // {
        //     var types = new[] {typeof(RealOperatorOverload), typeof(RealOperatorOverload)};
        //     var factory = new OperatorFactory(types);
        //
        //     Assert.AreEqual(1, factory.OperatorTypes.Count);
        //     Assert.AreEqual(typeof(RealOperatorOverload), factory.OperatorTypes[0]);
        // }

        [Test]
        public void AddOperatorMethod()
        {
            var method = typeof(SimpleOverload).GetMethod("Add");
            Assert.NotNull(method);

            var factory = new OperatorFactory();
            factory.AddMethod(method!);

            OperatorMethod? opMethod = factory.Find("+", typeof(int), typeof(int));

            Assert.NotNull(opMethod);
            Assert.AreEqual(method, opMethod?.Method);
            Assert.AreEqual("+", opMethod?.Operator);
            Assert.AreEqual(typeof(int), opMethod?.Type);
            Assert.AreEqual(typeof(int), opMethod?.OtherType);
            Assert.AreEqual(typeof(int), opMethod?.ReturnType);
        }

        [Test]
        public void TryAddNonStaticMethod_ShouldThrow()
        {
            var method = typeof(SimpleOverload).GetMethod("Minus");
            Assert.NotNull(method);

            var factory = new OperatorFactory();
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method!));
            
            Assert.AreEqual(OperatorMethodError.NonStaticMethod, ex?.Error);
        }
        
        [Test]
        public void TryAddNonDecoratedMethod_ShouldThrow()
        {
            var method = typeof(SimpleOverload).GetMethod("Div");
            Assert.NotNull(method);

            var factory = new OperatorFactory();
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method!));
            Assert.AreEqual(OperatorMethodError.NonDecoratedMethod, ex?.Error);
        }

        [Test]
        public void TryAddTwoSameOperatorSignature_ShouldThrow()
        {
            var method1 = typeof(IntegerOperatorOverload).GetMethod("Add");
            var method2 = typeof(SimpleOverload).GetMethod("Add");
            
            var factory = new OperatorFactory();
            factory.AddMethod(method1);
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method2));
            
            Assert.AreEqual(OperatorMethodError.DuplicationOperatorSignature, ex.Error);
        }
        
        
        [Test]
        public void TryAddTwoSameOperatorUnarySignature_ShouldThrow()
        {
            var method1 = typeof(IntegerOperatorOverload).GetMethod("Plus");
            var method2 = typeof(SimpleOverload).GetMethod("Plus");
            
            var factory = new OperatorFactory();
            factory.AddMethod(method1);
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method2));
            
            Assert.AreEqual(OperatorMethodError.DuplicationOperatorSignature, ex.Error);
        }
        
        
        [Test]
        public void TryAddVoidMethod_ShouldThrow()
        {
            var method = typeof(SimpleOverload).GetMethod("MulVoid");
            
            var factory = new OperatorFactory();
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method));
            
            Assert.AreEqual( OperatorMethodError.VoidReturn, ex?.Error);
        }
        
        
        [Test]
        public void TryAddUnknownOperatorSignature_ShouldThrow()
        {
            var method = typeof(SimpleOverload).GetMethod("UnaryMul");
            
            var factory = new OperatorFactory();
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method));
            
            Assert.AreEqual( OperatorMethodError.UnknownOperator, ex?.Error);
        }
        
        [Test]
        public void TryAdd_OperatorWithInvalidReturnType_ShouldThrow()
        {
            var method = typeof(SimpleOverload).GetMethod("True");
            
            var factory = new OperatorFactory();
            var ex = Assert.Throws<OperatorMethodException>(() => factory.AddMethod(method));
            
            Assert.AreEqual( OperatorMethodError.InvalidReturnType, ex?.Error);
        }
    }


    public class SimpleOverload
    {
        [OperatorMethod("+")]
        public static int Add(int a, int b) => a + b;
        
        [OperatorMethod("+")]
        public static int Plus(int a) => a;
        
        [OperatorMethod("-")]
        public int Minus(int a, int b) => a - b;
        
        [OperatorMethod("*")]
        private  int Mul(int a, int b) => a * b;
        
        public static  int Div(int a, int b) => a / b;
        
        [OperatorMethod("*")]
        public static int MulZeroParam() => 0;
        
        [OperatorMethod("*")]
        public static void MulVoid(int a, int b) {}

        [OperatorMethod("*")]
        public static int UnaryMul(int a) => a;
        
        [OperatorMethod("true")]
        public static int True(int a) => a;
    }

    public class CorrectOverload
    {
        [OperatorMethod("+")]
        public static int Add(int a, int b) => a + b;
        
        [OperatorMethod("+")]
        public static int Plus(int a) => a;
    }
}
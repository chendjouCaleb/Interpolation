using System;

namespace TextBinding.Utilities
{
    public class Assertions
    {
        public static void IsTrue(bool value)
        {
            if (!value)
            {
                throw new InvalidOperationException("Assertions failed; Value is false.");
            }
        }
    }
}
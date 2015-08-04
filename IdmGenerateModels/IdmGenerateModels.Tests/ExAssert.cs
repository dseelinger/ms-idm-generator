using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdmGenerateModels.Tests
{
    public static class ExAssert
    {
        public static void AreEqual(string expected, string actual) 
        {
            if (expected != actual)
            {
                if (string.IsNullOrEmpty(actual))
                {
                    Assert.AreEqual(expected, actual, "Actual Value was null or empty, Expected was not");
                }
                if (string.IsNullOrEmpty(expected))
                {
                    Assert.AreEqual(expected, actual, "Expected Value was null or empty, Actual value was not");
                }
                int max = Math.Max(expected.Length, actual.Length);
                for (int i = 0; i < max; i++)
                {
                    if (expected[i] != actual[i])
                    {
                        int contextSize = 20;
                        int start = Math.Max(i-contextSize, 0);
                        int endExpected = Math.Min(expected.Length, i + contextSize);
                        string expectedContext = expected.Substring(start, endExpected - start).Replace(Environment.NewLine, "");

                        int endActual = Math.Min(actual.Length, i + contextSize);
                        string actualContext = actual.Substring(start, endActual - start).Replace(Environment.NewLine, "");

                        var spaces = new string(' ', contextSize - (i - start) + contextSize);
                        var msg = string.Format("Text not equal at:{0}Exp:{1}{0}Act:{2}{0}    {3}^{0}{4}{0}{5}",
                            Environment.NewLine,
                            expectedContext,
                            actualContext,
                            spaces,
                            expected,
                            actual);

                        //Assert.AreEqual(expected, actual, msg);
                        Assert.Fail(msg);
                    }
                }
            }
        }
    }
}

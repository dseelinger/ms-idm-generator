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
                        var msg = string.Format("Text not equal:{0}{1}{0}{2}{0}{3}^",
                            Environment.NewLine,
                            expectedContext,
                            actualContext,
                            spaces);

                        Assert.AreEqual(expected, actual, msg);
                    }
                }
            }
        }
    }
}

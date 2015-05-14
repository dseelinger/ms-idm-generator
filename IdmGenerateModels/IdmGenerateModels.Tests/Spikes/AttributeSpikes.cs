using System.Collections.Generic;
using System.Linq;
using IdmNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdmGenerateModels.Tests.Spikes
{
    [TestClass]
    public class AttributeSpikes
    {
        [TestMethod]
        public void ConvertMultiValuedIntegerAttributeToListOfInts()
        {
            // Arrange
            var multiAttr = new IdmAttribute
            {
                Name = "MyInts",
                Values = new List<string>
                {
                    "1",
                    "2",
                    "3"
                }
            };

            // Act
            List<int> converted = multiAttr.Values.Select(int.Parse).ToList();

            // Assert
            Assert.AreEqual(1, converted[0]);
            Assert.AreEqual(2, converted[1]);
            Assert.AreEqual(3, converted[2]);
        }
    }
}

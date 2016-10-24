using System.Collections.Generic;
using System.Linq;
using IdmNet.Models;
using Xunit;
using FluentAssertions;

namespace IdmGenerateModels.Tests.Spikes
{
    public class AttributeSpikes
    {
        [Fact]
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
            converted[0].Should().Be(1);
            converted[1].Should().Be(2);
            converted[2].Should().Be(3);
        }
    }
}

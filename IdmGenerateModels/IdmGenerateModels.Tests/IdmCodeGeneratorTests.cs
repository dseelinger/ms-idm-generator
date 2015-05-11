using System.Collections.Generic;
using IdmNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdmGenerateModels.Tests
{
    [TestClass]
    public class IdmCodeGeneratorTests
    {
        [TestMethod]
        public void It_generates_the_correct_class_for_an_empty_object_type()
        {
            // Arrange
            var it = new IdmCodeGenerator(new ObjectTypeDescription
            {
                Name = "Foo",
                Description = "Bar"
            });

            // Act
            var result = it.Generate();

            // Assert
            Assert.AreEqual(TestData.EmptyClassOutput, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_string_attribute_with_all_string_fields_populated()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "First Choice for Summary Part I",
                Description =
                    "First Choice for Summary Part II",
                Required = true,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName",
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithAllStringFieldsPopulated, result);
        }
    }
}

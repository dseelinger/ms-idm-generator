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
                Description = "Bar",
                BindingDescriptions = new List<BindingDescription>()
            });

            // Act
            var result = it.Generate();

            // Assert
            Assert.AreEqual(TestData.EmptyClassOutput, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_binding_with_different_DisplayName()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Binding Display Name",
                Description =
                    "First Choice for Summary Part II",
                Required = true,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.BindingWithDifferentDisplayName, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_binding_with_different_Description()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "First Choice for Summary Part I",
                Description = "Binding Description",
                Required = true,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.BindingWithDifferentDescription, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_binding_thats_not_required()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "First Choice for Summary Part I",
                Description = "First Choice for Summary Part II",
                Required = false,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.BindingNotRequired, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_string_attribute_with_a_different_property_name()
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
                    Name = "PropertyName2"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithDifferentPropertyName, result);
        }



        [TestMethod]
        public void It_generates_the_correct_property_for_a_string_attribute_without_a_binding_displayName()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                Description =
                    "First Choice for Summary Part II",
                Required = true,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithoutBindingDisplayName, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_string_attribute_without_a_binding_description()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "First Choice for Summary Part I",
                Required = true,
                StringRegex = "*.",
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithoutABindingDescription, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_string_attribute_without_a_regular_expression()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "First Choice for Summary Part I",
                Description =
                    "First Choice for Summary Part II",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithoutRegEx, result);
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
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.StringAttributeWithAllStringFieldsPopulated, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_boolean()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Boolean Attrbute",
                Description =
                    "A boolean attribute",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "Boolean",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.BoolAttribute, result);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_an_integer()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Integer Attrbute",
                Description =
                    "An integer attribute",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            string result = it.GenerateProperty(bindingDescription);

            // Assert
            Assert.AreEqual(TestData.IntegerAttribute, result);
        }

    }
}

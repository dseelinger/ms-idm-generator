using System;
using System.Collections.Generic;
using Fare;
using IdmNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdmGenerateModels.Tests
{
    [TestClass]
    public class IdmCodeGeneratorTests
    {
        [TestMethod]
        public void It_generates_the_correct_classes_for_an_empty_object_type()
        {
            // Arrange
            var it = new IdmCodeGenerator(new Schema
            {
                Name = "Foo-Bar",
                Description = "Bat",
                BindingDescriptions = new List<BindingDescription>()
            });

            // Act
            var result = it.Generate();

            // Assert
            ExAssert.AreEqual(TestData.EmptyClassOutput, result.Item1);
            ExAssert.AreEqual(TestData.TestClassTemplate, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_binding_with_different_DisplayName()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "Binding Display Name",
                Description = "First Choice for Summary Part II",
                Required = true,
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BindingWithDifferentDisplayName, result.Item1);
            ExAssert.AreEqual(TestData.BindingWithDifferentDisplayNameTests, result.Item2);
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
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BindingWithDifferentDescription, result.Item1);
            ExAssert.AreEqual(TestData.BindingWithDifferentDescriptionTests, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_and_tests_for_a_binding_thats_not_required()
        {
            // Arrange
            var regExString = "8";
            var bindingDescription = new BindingDescription
            {
                DisplayName = "First Choice for Summary Part I",
                Description = "First Choice for Summary Part II",
                Required = false,
                StringRegex = regExString,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };
            var xeger = new Xeger(regExString);
            var match = xeger.Generate();
            var nonMatch = @"flkj3332@!!!$

fd333
";
            var expectedTests = string.Format(TestData.BindingNotRequiredTests, match, nonMatch);

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BindingNotRequired, result.Item1);
            ExAssert.AreEqual(expectedTests, result.Item2);
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
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName2"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithDifferentPropertyName, result.Item1);
            ExAssert.AreEqual(TestData.StringAttributeWithDifferentPropertyNameTests, result.Item2);

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
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithoutBindingDisplayName, result.Item1);
            ExAssert.AreEqual(TestData.StringAttributeWithoutBindingDisplayNameTests, result.Item2);
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
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithoutABindingDescription, result.Item1);
            ExAssert.AreEqual(TestData.StringAttributeWithoutABindingDescriptionTests, result.Item2);
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
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithoutRegEx, result.Item1);
            ExAssert.AreEqual(TestData.StringAttributeWithoutRegExTests, result.Item2);
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
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithAllStringFieldsPopulated, result.Item1);
            ExAssert.AreEqual(TestData.StringAttributeWithAllStringFieldsPopulatedTests, result.Item2);
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
                    Multivalued = false,
                    DataType = "Boolean",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BoolAttribute, result.Item1);
            ExAssert.AreEqual(TestData.BoolAttributeTests, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_non_required_boolean()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Boolean Attrbute",
                Description =
                    "A boolean attribute",
                Required = false,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Boolean",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BoolAttributeNotRequired, result.Item1);
            ExAssert.AreEqual(TestData.BoolAttributeNotRequiredTests, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_standard_reference_attribute_that_matches_an_object_type_name()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Reference Attrbute",
                Description =
                    "A standard reference attribute",
                Required = true, // should be ignored - reference attributes can't be required
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Reference",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
                // The generator needs the smarts of the generator or,
                // "How do I know that a "DisplayedOwner" is a "Person" object?
            };

            var it = new IdmCodeGenerator(null, null, @"[ { ""AttrName"": ""PropertyName"", ""ObjType"": ""Person"" } ]");

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StandardReferenceAttributeWhereTypeMatchesName, result.Item1);
            ExAssert.AreEqual(TestData.StandardReferenceAttributeWhereTypeMatchesNameTests, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_standard_reference_attribute_that_matches_an_item_in_the_json_environment_variable()
        {
            // Where's the environment variable
            // Arrange
            var it = new IdmCodeGenerator(null);
            var bindingDescription = new BindingDescription
            {
                DisplayName = "Reference Attrbute",
                Description = "A standard reference attribute",
                Required = true, // should be ignored - reference attributes can't be required
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Reference",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StandardReferenceAttributeWhereTypeFoundInJsonFile, result.Item1);
            ExAssert.AreEqual(TestData.StandardReferenceAttributeWhereTypeFoundInJsonFileTests, result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_reference_attribute_doesnt_match_any_object_type()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Reference Attrbute",
                Description =
                    "A standard reference attribute",
                Required = true, // should be ignored - reference attributes can't be required
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Reference",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName2"
                },
            };

            var it = new IdmCodeGenerator(null);


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.ReferenceAttributeWhereTypeNotFound, result.Item1);
            // TODO 013: Confirm Test generation
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_binary_attr()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Reference Attrbute",
                Description =
                    "A standard reference attribute",
                Required = true, 
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Binary",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BinaryAttribute, result.Item1);
            // TODO 014: ***** Confirm Test generation, including setting it to null - Including Tests!!! *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_class_names_that_have_dash()
        {
            // Arrange
            var it = new IdmCodeGenerator(new Schema
            {
                Name = "Foo-Bar",
                Description = "Bar",
                BindingDescriptions = new List<BindingDescription>()
            });

            // Act
            Tuple<string, string> result = it.Generate();

            // Assert
            ExAssert.AreEqual(TestData.ClassWithDash, result.Item1);
            // TODO 015: Confirm Test generation
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_attributes_with_dashes_in_the_name()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "First Choice for Summary Part I",
                Description =
                    "First Choice for Summary Part II",
                Required = true,
                StringRegex = "abc",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "String",
                    DisplayName = "Second Choice for Summary Part I",
                    Description = "Second Choice for Summary Part II",
                    Name = "Property-Name",
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.StringAttributeWithDashInName, result.Item1);
            // TODO 016: Confirm Test generation
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_booleans_with_dashes()
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
                    Multivalued = false,
                    DataType = "Boolean",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BoolAttributeWithDash, result.Item1);
            // TODO 017: Confirm Test generation
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_dashes_in_DateTime_attrName()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "DateTime",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.DateTimeAttributeWithDash, result.Item1);
            // TODO 018: ***** Add Test Generation for DateTime *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_dashes_in_reference_AttrName()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "Reference Attrbute",
                Description = "A standard reference attribute",
                Required = true, // should be ignored - reference attributes can't be required
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Reference",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null, new List<string> { "Property-Name" });

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.ReferenceAttrWithDashInName, result.Item1);
            // TODO 019: Confirm Test generation
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_binary_attributes_with_dashes_in_the_name()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Reference Attrbute",
                Description =
                    "A standard reference attribute",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Binary",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.BinaryAttributeWithDash, result.Item1);
            // TODO 020: Confirm Test generation
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_multi_valued_strings()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                StringRegex = "[0-9]",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "String",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.MultiValuedString, result.Item1);
            // TODO 021: ***** Generate Tests for Multi-valued strings *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_IntegerMinimum()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                IntegerMinimum = 5,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.IntegerAttributeWithMin, result.Item1);
            // TODO 022: ***** Create Tests for IntegerMinimum *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_IntegerMaximum()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                IntegerMaximum = 5,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.IntegerAttributeWithMax, result.Item1);
            // TODO 023: ***** Create Tests for IntegerMaximum *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_IntegerMaxAndMin()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                IntegerMinimum = 2,
                IntegerMaximum = 5,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.IntegerAttributeWithMinMax, result.Item1);
            // TODO 024: ***** Create Tests for Integer Max & Min *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_can_handle_MultiValuedIntegerMaxAndMin()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                IntegerMinimum = 2,
                IntegerMaximum = 5,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.MultiValuedIntegerAttributeWithMinMax, result.Item1);
            // TODO 025: ***** Create Tests for Multi-valued Integer Max & Min *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_Multivalued_DateTimes()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "DateTime",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.MultiValuedDateTime, result.Item1);
            // TODO 026: ***** Create Tests for Multi-valued DateTime *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_Multivalued_Reference_attributes()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "Reference",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null, new List<string> { "foo" },
                @"[ { ""AttrName"": ""ActionWorkflowInstance"", ""ObjType"": ""WorkflowInstance"" }, { ""AttrName"": ""Property-Name"", ""ObjType"": ""Model-Type"" } ]");


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.MultiValuedReference, result.Item1);
            // TODO 027: ***** Create Tests for Multi-valued Reference *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_handles_Multivalued_Binary_attributes()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "Binary",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "Property-Name"
                },
            };

            var it = new IdmCodeGenerator(null);


            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.MultiValuedBinary, result.Item1);
            // TODO 028: ***** Create Tests for Multi-valued Binary *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_an_integer_thats_required()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.IntegerAttribute, result.Item1);
            // TODO 029: ***** Create Tests for required Int *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_an_integer_thats_not_required()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "Integer",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.IntegerAttributeNotRequired, result.Item1);
            // TODO 030: ***** Create Tests for non-required Int *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_a_required_DateTime()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "DateTime",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.DateTimeAttribute, result.Item1);
            // TODO 031: ***** Create Tests for required DateTime *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_property_for_an_optional_DateTime()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "DateTime",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            Tuple<string, string> result = it.GenerateAPropertyAndItsTests(bindingDescription);

            // Assert
            ExAssert.AreEqual(TestData.DateTimeAttributeOptional, result.Item1);
            // TODO 033: ***** Create Tests for non-required DateTime *****
            //Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        public void It_generates_the_correct_class_for_an_object_type_with_a_multivalued_binary_attribute()
        {
            // Arrange
            var it = new IdmCodeGenerator(new Schema
            {
                Name = "Foo",
                Description = "Bar",
                BindingDescriptions = new List<BindingDescription>
                {
                    new BindingDescription
                    {
                        DisplayName = "My Display Name",
                        Description = "My Description",
                        Required = true,
                        BoundAttributeType = new AttributeTypeDescription
                        {
                            Multivalued = true,
                            DataType = "Binary",
                            DisplayName = "Doesn't matter",
                            Description = "Doesn't matter",
                            Name = "Property-Name"
                        },
                    }
                }
            });

            // Act
            var result = it.Generate();

            // Assert
            ExAssert.AreEqual(TestData.ClassOutputWithMultivaluedBinaryAttribute, result.Item1);
            // TODO 034: ***** Create Tests for class with multi-valued binary *****
            Assert.IsNotNull(result.Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void It_throws_for_an_unrecognized_single_value_type()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = false,
                    DataType = "FooBar",
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            it.GenerateAPropertyAndItsTests(bindingDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void It_throws_for_an_unrecognized_multi_value_type()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                BoundAttributeType = new AttributeTypeDescription
                {
                    Multivalued = true,
                    DataType = "Boolean", // Multi-valued attributes cannot be boolean
                    DisplayName = "Doesn't matter",
                    Description = "Doesn't matter",
                    Name = "PropertyName"
                },
            };

            var it = new IdmCodeGenerator(null);

            // Act
            it.GenerateAPropertyAndItsTests(bindingDescription);
        }
    }
}

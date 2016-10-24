using System;
using System.Collections.Generic;
using Fare;
using IdmNet.Models;
using Xunit;
using FluentAssertions;

namespace IdmGenerateModels.Tests
{
    public class IdmCodeGeneratorTests
    {
        [Fact]
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
            result.Item1.Should().Be(TestData.EmptyClassOutput);
            result.Item2.Should().Be(TestData.TestClassTemplate);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BindingWithDifferentDisplayName);
            result.Item2.Should().Be(TestData.BindingWithDifferentDisplayNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BindingWithDifferentDescription);
            result.Item2.Should().Be(TestData.BindingWithDifferentDescriptionTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BindingNotRequired);
            result.Item2.Should().Be(expectedTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithDifferentPropertyName);
            result.Item2.Should().Be(TestData.StringAttributeWithDifferentPropertyNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithoutBindingDisplayName);
            result.Item2.Should().Be(TestData.StringAttributeWithoutBindingDisplayNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithoutABindingDescription);
            result.Item2.Should().Be(TestData.StringAttributeWithoutABindingDescriptionTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithoutRegEx);
            result.Item2.Should().Be(TestData.StringAttributeWithoutRegExTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithAllStringFieldsPopulated);
            result.Item2.Should().Be(TestData.StringAttributeWithAllStringFieldsPopulatedTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BoolAttribute);
            result.Item2.Should().Be(TestData.BoolAttributeTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BoolAttributeNotRequired);
            result.Item2.Should().Be(TestData.BoolAttributeNotRequiredTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StandardReferenceAttributeWhereTypeMatchesName);
            result.Item2.Should().Be(TestData.StandardReferenceAttributeWhereTypeMatchesNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StandardReferenceAttributeWhereTypeFoundInJsonFile);
            result.Item2.Should().Be(TestData.StandardReferenceAttributeWhereTypeFoundInJsonFileTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.ReferenceAttributeWhereTypeNotFound);
            result.Item2.Should().Be(TestData.ReferenceAttributeWhereTypeNotFoundTests);
        }

        [Fact]
        public void It_generates_the_correct_property_and_tests_for_a_non_required_binary_attr()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName =
                    "Reference Attrbute",
                Description =
                    "A standard reference attribute",
                Required = false,
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
            result.Item1.Should().Be(TestData.BinaryAttribute);
            result.Item2.Should().Be(TestData.BinaryAttributeTests);
        }

        [Fact]
        public void It_generates_the_correct_property_and_tests_for_a_required_binary_attr()
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
            result.Item1.Should().Be(TestData.BinaryAttributeRequired);
            result.Item2.Should().Be(TestData.BinaryAttributeTestsRequired);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.ClassWithDash);
            result.Item2.Should().Be(TestData.ClassWithDashTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.StringAttributeWithDashInName);
            result.Item2.Should().Be(TestData.StringAttributeWithDashInNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BoolAttributeWithDash);
            result.Item2.Should().Be(TestData.BoolAttributeWithDashTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.DateTimeAttributeWithDash);
            result.Item2.Should().Be(TestData.DateTimeAttributeWithDashTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.ReferenceAttrWithDashInName);
            result.Item2.Should().Be(TestData.ReferenceAttrWithDashInNameTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.BinaryAttributeWithDash);
            result.Item2.Should().Be(TestData.BinaryAttributeWithDashTests);
        }

        [Fact]
        public void It_can_handle_required_multi_valued_strings()
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
            result.Item1.Should().Be(TestData.MultiValuedStringRequired);
            result.Item2.Should().Be(TestData.MultiValuedStringTestsRequired);
        }

        [Fact]
        public void It_can_handle_non_required_multi_valued_strings()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
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
            result.Item1.Should().Be(TestData.MultiValuedStringNonRequired);
            result.Item2.Should().Be(TestData.MultiValuedStringTestsNonRequired);
        }


        [Fact]
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
            result.Item1.Should().Be(TestData.IntegerAttributeWithMin);
            result.Item2.Should().Be(TestData.IntegerAttributeWithMinTests);
        }

        [Fact]
        public void It_can_handle_IntegerMaximum()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = true,
                IntegerMaximum = 10,
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
            result.Item1.Should().Be(TestData.IntegerAttributeWithMax);
            result.Item2.Should().Be(TestData.IntegerAttributeWithMaxTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.IntegerAttributeWithMinMax);
            result.Item2.Should().Be(TestData.IntegerAttributeWithMinMaxTests);
        }

        [Fact]
        public void It_can_handle_non_required_IntegerMaxAndMin()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
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
            result.Item1.Should().Be(TestData.IntegerAttributeWithMinMaxNotRequired);
            result.Item2.Should().Be(TestData.IntegerAttributeWithMinMaxNotRequiredTests);

        }

        [Fact]
        public void It_can_handle_non_required_multiValued_Integer_with_MaxAndMin()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
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
            result.Item1.Should().Be(TestData.MultiValuedIntegerAttributeWithMinMax);
            result.Item2.Should().Be(TestData.MultiValuedIntegerAttributeWithMinMaxTests);
        }

        [Fact]
        public void It_handles_Multivalued_DateTimes_not_required()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
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
            result.Item1.Should().Be(TestData.MultiValuedDateTime);
            result.Item2.Should().Be(TestData.MultiValuedDateTimeTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.MultiValuedReference);
            result.Item2.Should().Be(TestData.MultiValuedReferenceTests);
        }

        [Fact]
        public void It_handles_Multivalued_Binary_attributes()
        {
            // Arrange
            var bindingDescription = new BindingDescription
            {
                DisplayName = "My Display Name",
                Description = "My Description",
                Required = false,
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
            result.Item1.Should().Be(TestData.MultiValuedBinary);
            result.Item2.Should().Be(TestData.MultivaluedBinaryTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.IntegerAttribute);
            result.Item2.Should().Be(TestData.IntegerAttributeTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.IntegerAttributeNotRequired);
            result.Item2.Should().Be(TestData.IntegerAttributeNotRequiredTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.DateTimeAttribute);
            result.Item2.Should().Be(TestData.DateTimeAttributeTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.DateTimeAttributeOptional);
            result.Item2.Should().Be(TestData.DateTimeAttributeOptionalTests);
        }

        [Fact]
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
            result.Item1.Should().Be(TestData.ClassOutputWithMultivaluedBinaryAttribute);
            result.Item2.Should().Be(TestData.ClassOutputWithMultivaluedBinaryAttributeTests);
        }

        [Fact]
        //[ExpectedException(typeof(ApplicationException))]
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
            Action a = () => it.GenerateAPropertyAndItsTests(bindingDescription);
            a.ShouldThrow<ApplicationException>();
        }

        [Fact]
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
            Action a = () => it.GenerateAPropertyAndItsTests(bindingDescription);
            a.ShouldThrow<ApplicationException>();
        }
    }
}

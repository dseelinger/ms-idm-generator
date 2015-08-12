using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fare;
using IdmNet.Models;
using Newtonsoft.Json;

namespace IdmGenerateModels
{
    public class IdmCodeGenerator
    {
        private readonly Schema _schemaObject;
        private readonly List<string> _objectTypeNames;
        private readonly List<ReferenceToObjectTypeMap> _referenceMap;
        private readonly string _customSchemaJson;

        public IdmCodeGenerator(Schema schemaObject, List<string> objectTypeNames = null, string json = null)
        {
            _schemaObject = schemaObject;
            _objectTypeNames = objectTypeNames;

            // TODO later 001: eliminate dependency on system environment variables
            _customSchemaJson = Environment.GetEnvironmentVariable("CUSTOM_ATTR_TO_OBJ_MAPPINGS");

            if (!string.IsNullOrWhiteSpace(json))
                _referenceMap = JsonConvert.DeserializeObject<List<ReferenceToObjectTypeMap>>(json);
        }

        public Tuple<string,string> Generate()
        {
            var className = GetValidCSharpIdentifier(_schemaObject.Name);

            Tuple<StringBuilder, StringBuilder> classPropertiesAndTest = GeneratePropertiesAndTests();

            var classContents = string.Format(Templates.ClassTemplate,
                _schemaObject.Name,
                _schemaObject.Description,
                className,
                classPropertiesAndTest.Item1);

            var testsContents = string.Format(Templates.TestClassTemplate,
                className,
                _schemaObject.Name,
                classPropertiesAndTest.Item2);


            return new Tuple<string, string>(classContents, testsContents);
        }

        private static string GetValidCSharpIdentifier(string name)
        {
            return name.Replace('-', '_');
        }

        public Tuple<StringBuilder, StringBuilder> GeneratePropertiesAndTests()
        {
            var propertiesSb = new StringBuilder();
            var testsSb = new StringBuilder();

            var attrsToSkip = new[]
            {
                "ObjectID", "ObjectType", "CreatedTime", "Creator", "DeletedTime", "Description", "DetectedRulesList",
                "DisplayName", "ExpectedRulesList", "ExpirationTime", "Locale", "MVObjectID", "ResourceTime"
            };

            var bindingsToGenerate =
                _schemaObject.BindingDescriptions.Where(b => !(attrsToSkip.Contains(b.BoundAttributeType.Name)));

            foreach (var bindingDescription in bindingsToGenerate)
            {
                var propertyAndTests = GenerateAPropertyAndItsTests(bindingDescription);
                propertiesSb.Append(propertyAndTests.Item1);
                testsSb.Append(propertyAndTests.Item2);
            }

            return new Tuple<StringBuilder, StringBuilder>(propertiesSb, testsSb);
        }

        public Tuple<string, string> GenerateAPropertyAndItsTests(BindingDescription bindingDescription)
        {
            var propertyAndTests = bindingDescription.BoundAttributeType.Multivalued
                ? GenerateMultiValuedPropertyAndTests(bindingDescription)
                : GenerateSingleValuedPropertyAndTests(bindingDescription);

            return propertyAndTests;
        }

        public Tuple<string, string> GenerateSingleValuedPropertyAndTests(BindingDescription bindingDescription)
        {
            string property;
            string tests;
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                case "Text":
                    property = GenerateASingleValuedString(bindingDescription);
                    tests = GenerateSingleValuedStringTests(bindingDescription);
                    break;
                case "Boolean":
                case "Integer":
                    property = GenerateSingleValuedValue(bindingDescription);
                    tests = GenerateSingleValuedValueTests(bindingDescription);
                    break;
                case "Reference":
                    property = GenerateSingleValuedReference(bindingDescription);
                    tests = GenerateSingleValuedReferenceTests(bindingDescription);
                    break;
                case "DateTime":
                    property = GenerateSingleValuedDateTime(bindingDescription);
                    tests = GenerateSingleValuedDateTimeTests(bindingDescription);
                    break;
                case "Binary":
                    property = GenerateSingleValuedBinary(bindingDescription);
                    tests = GenerateSingleValuedBinaryTests(bindingDescription);
                    break;
                default:
                    throw new ApplicationException();
            }
            return new Tuple<string, string>(property, tests);
        }

        private Tuple<string, string> GenerateMultiValuedPropertyAndTests(BindingDescription bindingDescription)
        {
            string property;
            string tests = null;
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                case "Text":
                    property = GenerateMultiValuedString(bindingDescription);
                    tests = GenerateMultiValuedStringTests(bindingDescription);
                    break;
                case "Integer":
                    property = GenerateMultiValuedValue(bindingDescription);
                    tests = GenerateMultiValuedValueTests(bindingDescription);
                    break;
                case "DateTime":
                    property = GenerateMultiValuedDateTime(bindingDescription);
                    tests = GenerateMultiValuedDateTimeTests(bindingDescription);
                    break;
                case "Reference":
                    property = GenerateMultiValuedReference(bindingDescription);
                    tests = GenerateMultiValuedReferenceTests(bindingDescription);
                    break;
                case "Binary":
                    property = GenerateMultiValuedBinary(bindingDescription);
                    tests = GenerateMultivaluedBinaryTests(bindingDescription);
                    break;
                default:
                    throw new ApplicationException();
            }
            return new Tuple<string, string>(property, tests);
        }

        private string GenerateMultiValuedValueTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            var testValueString = GetTestValueString(bindingDescription);
            var nullTest = GetNullTestMultivaluedValue(bindingDescription, validCSharpIdentifier, testValueString);
            var minTest = GetMinTest(bindingDescription, testValueString, validCSharpIdentifier, Templates.MinTestMultivalued);
            var maxTest = GetMaxTest(bindingDescription, testValueString, validCSharpIdentifier, Templates.MaxTestMultivalued);

            return string.Format(Templates.MultiValuedValueFormatTests,
                validCSharpIdentifier,
                testValueString,
                nullTest,
                minTest + maxTest);
        }

        private string GenerateMultiValuedDateTimeTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            var nullTest = GetNullTestMultivaluedDateTime(bindingDescription, validCSharpIdentifier);

            return string.Format(Templates.MultiValuedDateTimeFormatTests,
                validCSharpIdentifier,
                nullTest);
        }

        private string GenerateMultiValuedReferenceTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);

            var tests = string.Format(Templates.MultivaluedReferenceFormatTests,
                validCSharpIdentifier,
                GetObjTypeName(bindingDescription));
            return tests;
        }




        private string GenerateMultiValuedBinary(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedBinaryFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private string GenerateMultiValuedReference(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedReferenceFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetObjTypeName(bindingDescription),
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }


        private string GenerateMultiValuedDateTime(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedDateTimeFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private string GenerateMultiValuedValue(BindingDescription bindingDescription)
        {
            var minMax = GetMinMax(bindingDescription);
            var propertyCode = string.Format(Templates.MultiValuedIntegerFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                minMax);
            return propertyCode;
        }

        private string GenerateMultiValuedString(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedStringFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                GetRegEx(bindingDescription));
        }

        private string GenerateMultiValuedStringTests(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedStringFormatTests,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private static string GenerateSingleValuedBinaryTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.SingleValuedBinaryNullTestFormat,
                    validCSharpIdentifier);
            var tests = string.Format(Templates.SingleValuedBinaryFormatTests,
                validCSharpIdentifier,
                nullTest);
            return tests;
        }

        private static string GenerateMultivaluedBinaryTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.MultialuedBinaryNullTestFormat,
                    validCSharpIdentifier);
            var tests = string.Format(Templates.MultivaluedBinaryFormatTests,
                validCSharpIdentifier,
                nullTest);
            return tests;
        }

        private static string GenerateSingleValuedBinary(BindingDescription bindingDescription)
        {
            var property = string.Format(Templates.SingleValuedBinaryFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
            return property;
        }

        private string GenerateSingleValuedReferenceTests(BindingDescription bindingDescription)
        {
            var tests = string.Format(Templates.SingleValuedReferenceFormatTests,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                GetObjTypeName(bindingDescription));
            return tests;
        }

        private string GenerateSingleValuedReference(BindingDescription bindingDescription)
        {
            var property = string.Format(Templates.SingleValuedReferenceFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetObjTypeName(bindingDescription),
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
            return property;
        }

        private string GetObjTypeName(BindingDescription bindingDescription)
        {
            string returnVal = "IdmResource";
            if (IsObjectTypeSameAsAttributeName(bindingDescription))
            {
                returnVal = bindingDescription.BoundAttributeType.Name;
            }
            else if (IsObjectTypeListedInJsonFile(bindingDescription))
            {
                returnVal = GetObjectTypeFromJson(bindingDescription, returnVal);
            }
            else if (!string.IsNullOrWhiteSpace(_customSchemaJson))
            {
                returnVal = GetObjectTypeFromCustomSchemaJson(bindingDescription, returnVal);
            }
            return GetValidCSharpIdentifier(returnVal);
        }

        private string GetObjectTypeFromCustomSchemaJson(BindingDescription bindingDescription, string returnVal)
        {
            List<ReferenceToObjectTypeMap> otherMappings =
                JsonConvert.DeserializeObject<List<ReferenceToObjectTypeMap>>(_customSchemaJson);
            if (otherMappings.Any(r => r.AttrName == bindingDescription.BoundAttributeType.Name))
            {
                returnVal = (from r in otherMappings
                    where r.AttrName == bindingDescription.BoundAttributeType.Name
                    select r.ObjType).First();
            }
            return returnVal;
        }

        private string GetObjectTypeFromJson(BindingDescription bindingDescription, string returnVal)
        {
            if (returnVal == null) throw new ArgumentNullException(nameof(returnVal));
            returnVal = (from r in _referenceMap
                where r.AttrName == bindingDescription.BoundAttributeType.Name
                select r.ObjType).First();
            return returnVal;
        }

        private bool IsObjectTypeListedInJsonFile(BindingDescription bindingDescription)
        {
            return _referenceMap != null && _referenceMap.Any(r => r.AttrName == bindingDescription.BoundAttributeType.Name);
        }

        private bool IsObjectTypeSameAsAttributeName(BindingDescription bindingDescription)
        {
            return _objectTypeNames != null && _objectTypeNames.Contains(bindingDescription.BoundAttributeType.Name);
        }

        private static string GenerateSingleValuedDateTimeTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.SingleValuedDateTimeNullTestFormat,
                    validCSharpIdentifier);
            var tests = string.Format(
                Templates.SingleValuedDateTimeFormatTests,
                validCSharpIdentifier,
                nullTest);
            return tests;
        }

        private static string GenerateSingleValuedDateTime(BindingDescription bindingDescription)
        {
            var property = string.Format(Templates.SingleValuedDateTimeFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                bindingDescription.Required == true ? "" : "?",
                bindingDescription.Required == true ? "" : "Nullable"
                );
            return property;
        }

        private static string GenerateSingleValuedValueTests(BindingDescription bindingDescription)
        {
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            var testValueString = GetTestValueString(bindingDescription);
            var nullTest = GetNullTest(bindingDescription, validCSharpIdentifier, testValueString);
            var minTest = GetMinTest(bindingDescription, testValueString, validCSharpIdentifier, Templates.MinTest);
            var maxTest = GetMaxTest(bindingDescription, testValueString, validCSharpIdentifier, Templates.MaxTest);

            return string.Format(Templates.SingleValuedValueFormatTests,
                validCSharpIdentifier,
                testValueString,
                nullTest,
                minTest + maxTest);
        }

        private static string GetNullTest(BindingDescription bindingDescription, string validCSharpIdentifier,
            string testValueString)
        {
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.SingleValuedValueNullTestFormat,
                    validCSharpIdentifier, testValueString);
            return nullTest;
        }

        private static string GetNullTestMultivaluedValue(BindingDescription bindingDescription, string validCSharpIdentifier,
            string testValueString)
        {
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.MultiValuedValueValueNullTestFormat,
                    validCSharpIdentifier, testValueString);
            return nullTest;
        }

        private static string GetNullTestMultivaluedDateTime(BindingDescription bindingDescription, string validCSharpIdentifier)
        {
            string nullTest = bindingDescription.Required == true
                ? ""
                : string.Format(Templates.MultiValuedDateTimeNullTestFormat, validCSharpIdentifier);
            return nullTest;
        }

        private static string GetMaxTest(BindingDescription bindingDescription, string testValueString,
            string validCSharpIdentifier, string format)
        {
            string maxTest = "";
            if (testValueString != "true" && bindingDescription.IntegerMaximum != null)
            {
                maxTest = string.Format(format, validCSharpIdentifier, bindingDescription.IntegerMaximum + 1);
            }
            return maxTest;
        }

        private static string GetMinTest(BindingDescription bindingDescription, string testValueString,
            string validCSharpIdentifier, string format)
        {
            string minTest = "";
            if (testValueString != "true" && bindingDescription.IntegerMinimum != null)
            {
                minTest = string.Format(format,
                    validCSharpIdentifier,
                    bindingDescription.IntegerMinimum - 1);
            }
            return minTest;
        }

        private static string GetTestValueString(BindingDescription bindingDescription)
        {
            string testValueString = bindingDescription.BoundAttributeType.DataType == "Boolean" ? "true" : "123";
            if (testValueString != "true" && bindingDescription.IntegerMinimum != null)
                testValueString = bindingDescription.IntegerMinimum.ToString();
            if (testValueString != "true" && bindingDescription.IntegerMaximum != null)
                testValueString = bindingDescription.IntegerMaximum.ToString();
            return testValueString;
        }

        private static string GenerateSingleValuedValue(BindingDescription bindingDescription)
        {
            string conversionMethodString;
            string typeString = bindingDescription.BoundAttributeType.DataType == "Boolean" ? "bool" : "int";
            if (bindingDescription.Required == false)
            {
                typeString += '?';
                conversionMethodString = bindingDescription.BoundAttributeType.DataType == "Boolean"
                    ? "AttrToNullableBool"
                    : "AttrToNullableInteger";
            }
            else
            {
                conversionMethodString = bindingDescription.BoundAttributeType.DataType == "Boolean"
                    ? "AttrToBool"
                    : "AttrToInteger";
            }

            var minMax = GetMinMax(bindingDescription);
            var validCSharpIdentifier = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            var propertyCode = string.Format(Templates.SingleValuedValueFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                typeString,
                validCSharpIdentifier,
                conversionMethodString,
                minMax);
            return propertyCode;
        }

        private static string GetMinMax(BindingDescription bindingDescription)
        {
            var minMax = "";
            var min = bindingDescription.IntegerMinimum;
            var max = bindingDescription.IntegerMaximum;
            if (min != null)
            {
                if (bindingDescription.BoundAttributeType.Multivalued)
                {
                    var format = @"if (value.Any( v => v < {0}))
                    throw new ArgumentException(""One or more invalid values for {1}.  Minimum value for each is {0}"");
                ";
                    minMax += string.Format(format, min, bindingDescription.BoundAttributeType.Name);
                }
                else
                {
                    var format = @"if (value < {0})
                    throw new ArgumentException(""Invalid value for {1}.  Minimum value is {0}"");
                ";
                    minMax += string.Format(format, min, bindingDescription.BoundAttributeType.Name);
                }
            }
            if (max != null)
            {
                if (bindingDescription.BoundAttributeType.Multivalued)
                {
                    var format = @"if (value.Any( v => v > {0}))
                    throw new ArgumentException(""One or more invalid values for {1}.  Maximum value for each is {0}"");
                ";
                    minMax += string.Format(format, max, bindingDescription.BoundAttributeType.Name);

                }
                else
                {
                    var format = @"if (value > {0})
                    throw new ArgumentException(""Invalid value for {1}.  Maximum value is {0}"");
                ";
                    minMax += string.Format(format, max, bindingDescription.BoundAttributeType.Name);
                }
            }
            return minMax;
        }

        private static string GenerateSingleValuedStringTests(BindingDescription bindingDescription)
        {
            var nonMatchTest = "";
            var val = "A string";
            if (!string.IsNullOrEmpty(bindingDescription.StringRegex))
            {
                var xeger = new Xeger(bindingDescription.StringRegex);
                val = xeger.Generate();

                var nonMatch = @"flkj3332@!!!$

fd333
";
                nonMatchTest = string.Format(Templates.NonMatchTest,
                    GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                    nonMatch);
            }

            string tests = string.Format(Templates.SingleValuedStringFormatTests,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                val,
                nonMatchTest);
            return tests;
        }

        private static string GenerateASingleValuedString(BindingDescription bindingDescription)
        {
            string property = string.Format(Templates.SingleValuedStringFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                GetRegEx(bindingDescription));
            return property;
        }

        private static string GetDescription(BindingDescription bindingDescription)
        {
            return bindingDescription.Description ?? bindingDescription.BoundAttributeType.Description;
        }

        private static string GetDisplayName(BindingDescription bindingDescription)
        {
            return bindingDescription.DisplayName ?? bindingDescription.BoundAttributeType.DisplayName;
        }

        private static string GetRegEx(BindingDescription bindingDescription)
        {
            var regEx = "";
            if (!string.IsNullOrEmpty(bindingDescription.StringRegex))
            {
                if (bindingDescription.BoundAttributeType.Multivalued)
                {
                    var regExFormat = @"var regEx = new RegEx(""{0}"");
                if (value.Any(x => !regEx.IsMatch(x))
                    throw new ArgumentException(""One or more invalid values for {1}.  Each value must match regular expression '{0}'"");
                ";
                    regEx = string.Format(regExFormat, bindingDescription.StringRegex, bindingDescription.BoundAttributeType.Name);
                    
                }
                else
                {
                    var regExFormat = @"var regEx = new RegEx(""{0}"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for {1}.  Must match regular expression '{0}'"");
                ";
                    regEx = string.Format(regExFormat, bindingDescription.StringRegex, bindingDescription.BoundAttributeType.Name);
                }
            }
            return regEx;
        }

        private static string GetRequired(BindingDescription bindingDescription)
        {
            var required = "";
            if (bindingDescription.Required == true)
                required = $"[Required]{Environment.NewLine}        ";
            return required;
        }
    }
}

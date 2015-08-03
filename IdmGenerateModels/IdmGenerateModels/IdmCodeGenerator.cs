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
            // Bad - tight coupling to system environment variables
            _customSchemaJson = Environment.GetEnvironmentVariable("CUSTOM_ATTR_TO_OBJ_MAPPINGS");

            if (!string.IsNullOrWhiteSpace(json))
            {
                _referenceMap = JsonConvert.DeserializeObject<List<ReferenceToObjectTypeMap>>(json);
            }
        }

        public Tuple<string,string> Generate()
        {
            var className = GetValidCSharpIdentifier(_schemaObject.Name);

            Tuple<StringBuilder, StringBuilder> classPropertiesAndTest = GeneratePropertiesAndTests();

            var classContents = String.Format(Templates.ClassTemplate,
                _schemaObject.Name,
                _schemaObject.Description,
                className,
                classPropertiesAndTest.Item1);

            var testsContents = String.Format(Templates.TestClassTemplate,
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
                var propertyAndTests = GeneratePropertyAndTests(bindingDescription);
                propertiesSb.Append(propertyAndTests.Item1);
                testsSb.Append(propertyAndTests.Item2);
            }

            return new Tuple<StringBuilder, StringBuilder>(propertiesSb, testsSb);
        }

        public Tuple<string, string> GeneratePropertyAndTests(BindingDescription bindingDescription)
        {
            var propertyAndTests = bindingDescription.BoundAttributeType.Multivalued
                ? GenerateMultiValuedPropertyAndTests(bindingDescription)
                : GenerateSingleValuedPropertyAndTests(bindingDescription);

            return propertyAndTests;
        }

        public Tuple<string, string> GenerateSingleValuedPropertyAndTests(BindingDescription bindingDescription)
        {
            Tuple<string, string> propertyAndTests;
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                case "Text":
                    propertyAndTests = GenerateSingleValuedStringProperty(bindingDescription);
                    break;
                case "Boolean":
                case "Integer":
                    propertyAndTests = new Tuple<string, string>(GenerateSingleValuedValueProperty(bindingDescription), null);
                    break;
                case "DateTime":
                    propertyAndTests = new Tuple<string, string>(GenerateSingleValuedDateTimeProperty(bindingDescription), null);
                    break;
                case "Reference":
                    propertyAndTests = new Tuple<string, string>(GenerateSingleValuedReferenceProperty(bindingDescription), null);
                    break;
                case "Binary":
                    propertyAndTests = new Tuple<string, string>(GenerateSingleValuedBinaryProperty(bindingDescription), null);
                    break;
                default:
                    throw new ApplicationException();
            }
            return propertyAndTests;
        }

        private Tuple<string, string> GenerateMultiValuedPropertyAndTests(BindingDescription bindingDescription)
        {
            string prop;
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                case "Text":
                    prop = GenerateMultiValuedStringProperty(bindingDescription);
                    break;
                case "Integer":
                    prop = GenerateMultiValuedValueProperty(bindingDescription);
                    break;
                case "DateTime":
                    prop = GenerateMultiValuedDateTimeProperty(bindingDescription);
                    break;
                case "Reference":
                    prop = GenerateMultiValuedReferenceProperty(bindingDescription);
                    break;
                case "Binary":
                    prop = GenerateMultiValuedBinaryProperty(bindingDescription);
                    break;
                default:
                    throw new ApplicationException();
            }
            return new Tuple<string, string>(prop, null);
        }


        private string GenerateMultiValuedBinaryProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedBinaryFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private string GenerateMultiValuedReferenceProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedReferenceFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetObjTypeName(bindingDescription),
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }


        private string GenerateMultiValuedDateTimeProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedDateTimeFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private string GenerateMultiValuedValueProperty(BindingDescription bindingDescription)
        {
            var minMax = GetMinMax(bindingDescription);
            var propertyCode = String.Format(Templates.MultiValuedIntegerFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                minMax);
            return propertyCode;
        }

        private string GenerateMultiValuedStringProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.MultiValuedStringFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                GetRegEx(bindingDescription));
        }

        private string GenerateSingleValuedBinaryProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.SingleValuedBinaryFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
        }

        private string GenerateSingleValuedReferenceProperty(BindingDescription bindingDescription)
        {
            return string.Format(Templates.SingleValuedReferenceFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetObjTypeName(bindingDescription),
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name));
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

        private static string GenerateSingleValuedDateTimeProperty(BindingDescription bindingDescription)
        {
            return String.Format(Templates.SingleValuedDateTimeFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
                bindingDescription.Required == true ? "" : "?",
                bindingDescription.Required == true ? "" : "Nullable"
                );
        }

        public static string GenerateSingleValuedValueProperty(BindingDescription bindingDescription)
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
            var propertyCode = String.Format(Templates.SingleValuedValueFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                typeString,
                GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name),
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
                    minMax += String.Format(format, min, bindingDescription.BoundAttributeType.Name);

                }
                else
                {
                    var format = @"if (value < {0})
                    throw new ArgumentException(""Invalid value for {1}.  Minimum value is {0}"");
                ";
                    minMax += String.Format(format, min, bindingDescription.BoundAttributeType.Name);
                }
            }
            if (max != null)
            {
                if (bindingDescription.BoundAttributeType.Multivalued)
                {
                    var format = @"if (value.Any( v => v > {0}))
                    throw new ArgumentException(""One or more invalid values for {1}.  Maximum value for each is {0}"");
                ";
                    minMax += String.Format(format, max, bindingDescription.BoundAttributeType.Name);

                }
                else
                {
                    var format = @"if (value > {0})
                    throw new ArgumentException(""Invalid value for {1}.  Maximum value is {0}"");
                ";
                    minMax += String.Format(format, max, bindingDescription.BoundAttributeType.Name);
                }
            }
            return minMax;
        }

        public static Tuple<string, string> GenerateSingleValuedStringProperty(BindingDescription bindingDescription)
        {
            var propName = GetValidCSharpIdentifier(bindingDescription.BoundAttributeType.Name);
            string property = String.Format(Templates.SingleValuedStringFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                propName,
                GetRegEx(bindingDescription));

            var nonMatchTest = "";
            var val = "A string";
            if (!string.IsNullOrEmpty(bindingDescription.StringRegex))
            {
                var xeger = new Xeger(bindingDescription.StringRegex);
                val = xeger.Generate();

                var nonMatch = @"flkj3332@!!!$

fd333
";
                nonMatchTest = String.Format(Templates.NonMatchTest,
                    propName,
                    nonMatch);
            }

            string tests = String.Format(Templates.SingleValuedStringTestsFormat,
                propName,
                val,
                nonMatchTest);



            return new Tuple<string, string>(property, tests);

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
            if (!String.IsNullOrEmpty(bindingDescription.StringRegex))
            {
                if (bindingDescription.BoundAttributeType.Multivalued)
                {
                    var regExFormat = @"var regEx = new RegEx(""{0}"");
                if (value.Any(x => !regEx.IsMatch(x))
                    throw new ArgumentException(""One or more invalid values for {1}.  Each value must match regular expression '{0}'"");
                ";
                    regEx = String.Format(regExFormat, bindingDescription.StringRegex, bindingDescription.BoundAttributeType.Name);
                    
                }
                else
                {
                    var regExFormat = @"var regEx = new RegEx(""{0}"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for {1}.  Must match regular expression '{0}'"");
                ";
                    regEx = String.Format(regExFormat, bindingDescription.StringRegex, bindingDescription.BoundAttributeType.Name);
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

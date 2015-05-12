using System;
using System.Linq;
using System.Text;
using IdmNet.Models;

namespace IdmGenerateModels
{
    public class IdmCodeGenerator
    {
        private readonly ObjectTypeDescription _objectTypeDescription;

        public IdmCodeGenerator(ObjectTypeDescription objectTypeDescription)
        {
            _objectTypeDescription = objectTypeDescription;
        }

        public string Generate()
        {
            var attrSb = GenerateAttributes();

            return String.Format(Templates.ClassTemplate, _objectTypeDescription.Name,
                _objectTypeDescription.Description, null, null, attrSb);
        }

        public StringBuilder GenerateAttributes()
        {
            var attrSb = new StringBuilder();

            var attrsToSkip = new[]
            {
                "ObjectID", "ObjectType", "CreatedTime", "Creator", "DeletedTime", "Description", "DetectedRulesList",
                "DisplayName", "ExpectedRulesList", "ExpirationTime", "Locale", "MVObjectID", "ResourceTime"
            };

            var bindingsToGenerate =
                _objectTypeDescription.BindingDescriptions.Where(b => !(attrsToSkip.Contains(b.BoundAttributeType.Name)));

            foreach (var bindingDescription in bindingsToGenerate)
            {
                attrSb.Append(GenerateProperty(bindingDescription));
            }
            return attrSb;
        }

        public string GenerateProperty(BindingDescription bindingDescription)
        {
            string prop = "";
            if (bindingDescription.BoundAttributeType.Multivalued)
            {
                
            }
            else
            {
                prop = GenerateSingleValuedProperty(bindingDescription, prop);
            }
            return prop;
        }

        public static string GenerateSingleValuedProperty(BindingDescription bindingDescription, string prop)
        {
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                    prop = GenerateSingleValuedStringProperty(bindingDescription);
                    break;
                case "Boolean":
                case "Integer":
                    prop = GenerateSingleValuedValueProperty(bindingDescription);
                    break;
            }
            return prop;
        }

        public static string GenerateSingleValuedValueProperty(BindingDescription bindingDescription)
        {
            string typeString = bindingDescription.BoundAttributeType.DataType == "Boolean" ? "bool?" : "int?";
            return String.Format(Templates.SingleValuedValueFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                typeString);
        }

        public static string GenerateSingleValuedStringProperty(BindingDescription bindingDescription)
        {
            return String.Format(Templates.SingleValuedStringFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetRegEx(bindingDescription));
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
                var regExFormat = @"var regEx = new RegEx(""{0}"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for {1}.  Must match regular expression '{0}'"");
                ";
                regEx = String.Format(regExFormat, bindingDescription.StringRegex, bindingDescription.BoundAttributeType.Name);
            }
            return regEx;
        }

        private static string GetRequired(BindingDescription bindingDescription)
        {
            var required = "";
            if (bindingDescription.Required == true)
                required = string.Format("[Required]{0}        ", Environment.NewLine);
            return required;
        }
    }
}

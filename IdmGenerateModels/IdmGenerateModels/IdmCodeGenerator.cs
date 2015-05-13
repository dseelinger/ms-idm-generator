using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdmNet.Models;
using Newtonsoft.Json;

namespace IdmGenerateModels
{
    public class IdmCodeGenerator
    {
        private readonly ObjectTypeDescription _objectTypeDescription;
        private readonly IEnumerable<string> _objectTypeNames;
        private readonly List<ReferenceToObjectTypeMap> _referenceMap;
 
        public IdmCodeGenerator(ObjectTypeDescription objectTypeDescription, IEnumerable<string> objectTypeNames = null, string json = null)
        {
            _objectTypeDescription = objectTypeDescription;
            _objectTypeNames = objectTypeNames;

            if (!string.IsNullOrWhiteSpace(json))
            {
                _referenceMap = JsonConvert.DeserializeObject<List<ReferenceToObjectTypeMap>>(json);
            }
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
                prop = GenerateSingleValuedProperty(bindingDescription);
            }
            return prop;
        }

        public string GenerateSingleValuedProperty(BindingDescription bindingDescription)
        {
            string prop;
            switch (bindingDescription.BoundAttributeType.DataType)
            {
                case "String":
                case "Text":
                    prop = GenerateSingleValuedStringProperty(bindingDescription);
                    break;
                case "Boolean":
                case "Integer":
                    prop = GenerateSingleValuedValueProperty(bindingDescription);
                    break;
                case "DateTime":
                    prop = GenerateSingleValuedDateTimeProperty(bindingDescription);
                    break;
                case "Reference":
                    prop = GenerateSingleValuedReferenceProperty(bindingDescription);
                    break;
                case "Binary":
                    prop = GenerateSingleValuedBinaryProperty(bindingDescription);
                    break;
                default:
                    // TODO: Binary is next
                    throw new ApplicationException();
            }
            return prop;
        }

        private string GenerateSingleValuedBinaryProperty(BindingDescription bindingDescription)
        {
            return String.Format(Templates.SingleValuedBinaryFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name);
        }

        private string GenerateSingleValuedReferenceProperty(BindingDescription bindingDescription)
        {
            return String.Format(Templates.SingleValuedReferenceFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                bindingDescription.BoundAttributeType.Name,
                GetObjTypeName(bindingDescription));
        }

        private string GetObjTypeName(BindingDescription bindingDescription)
        {
            if (_objectTypeNames != null && _objectTypeNames.Contains(bindingDescription.BoundAttributeType.Name))
            {
                return bindingDescription.BoundAttributeType.Name;
            }
            if (_referenceMap != null && _referenceMap.Any(r => r.AttrName == bindingDescription.BoundAttributeType.Name))
            {
                return (from r in _referenceMap
                    where r.AttrName == bindingDescription.BoundAttributeType.Name
                    select r.ObjType).First();
            }
            var otherJson = Environment.GetEnvironmentVariable("CUSTOM_ATTR_TO_OBJ_MAPPINGS");
            if (!string.IsNullOrWhiteSpace(otherJson))
            {
                List<ReferenceToObjectTypeMap> otherMappings = JsonConvert.DeserializeObject<List<ReferenceToObjectTypeMap>>(otherJson);
                if (otherMappings.Any(r => r.AttrName == bindingDescription.BoundAttributeType.Name))
                {
                    return (from r in otherMappings
                            where r.AttrName == bindingDescription.BoundAttributeType.Name
                            select r.ObjType).First();
                }
            }
            return "IdmResource";
        }

        private static string GenerateSingleValuedDateTimeProperty(BindingDescription bindingDescription)
        {
            return String.Format(Templates.SingleValuedDateTimeFormat,
                GetDisplayName(bindingDescription),
                GetDescription(bindingDescription),
                GetRequired(bindingDescription),
                bindingDescription.BoundAttributeType.Name);
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

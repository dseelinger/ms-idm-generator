using System;
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
            return String.Format(Templates.ClassTemplate, _objectTypeDescription.Name,
                _objectTypeDescription.Description, null, null, null);
        }

        public string GenerateProperty(BindingDescription bindingDescription)
        {
            var required = GetRequired(bindingDescription);

            var regEx = GetRegEx(bindingDescription);

            var propertyFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public string {3}
        {{
            get {{ return GetAttrValue(""{3}""); }}
            set {{
                {4}SetAttrValue(""{3}"", value); 
            }}
        }}

";
            return String.Format(propertyFormat,
                bindingDescription.DisplayName ?? bindingDescription.BoundAttributeType.DisplayName,
                bindingDescription.Description ?? bindingDescription.BoundAttributeType.Description, 
                required, 
                bindingDescription.BoundAttributeType.Name,
                regEx);
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

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
            throw new NotImplementedException();
        }
    }
}

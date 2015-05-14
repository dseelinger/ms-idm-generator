namespace IdmGenerateModels
{
    public static class Templates
    {

        public const string SingleValuedStringFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public string {4}
        {{
            get {{ return GetAttrValue(""{3}""); }}
            set {{
                {5}SetAttrValue(""{3}"", value); 
            }}
        }}

";

        public const string MultiValuedStringFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<string> {4}
        {{
            get {{ return GetAttrValues(""{3}""); }}
            set {{
                {5}SetAttrValues(""{3}"", value); 
            }}
        }}

";

        public const string MultiValuedIntegerFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<int> {4}
        {{
            get {{ return GetAttr(""{3}"").Values.Select(int.Parse).ToList(); }}
            set {{
                {5}SetAttrValues(""{3}"", value.Select(v => v.ToString());
            }}
        }}

";

        public const string SingleValuedValueFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public {4} {5}
        {{
            get {{ return {6}(""{3}""); }}
            set {{ 
                {7}SetAttrValue(""{3}"", value.ToString());
            }}
        }}

";

        public const string SingleValuedDateTimeFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public DateTime? {4}
        {{
            get {{ return GetAttr(""{3}"") == null ? null : GetAttr(""{3}"").ToDateTime(); }}
            set {{ SetAttrValue(""{3}"", value.ToString()); }}
        }}

";

        public const string SingleValuedBinaryFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public byte[] {4}
        {{
            get {{ return GetAttr(""{3}"") == null ? null : GetAttr(""{3}"").ToBinary(); }}
            set {{ SetAttrValue(""{3}"", Convert.ToBase64String(value)); }}
        }}

";

        public const string SingleValuedReferenceFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        public {3} {4}
        {{
            get {{ return GetAttr(""{2}"", _the{4}); }}
            set 
            {{ 
                _the{4} = value;
                SetAttrValue(""{2}"", ObjectIdOrNull(value)); 
            }}
        }}
        private {3} _the{4};

";

        // ObjectType name
        // ObjectType Description
        // privates (if any)
        // Complex Attributes (if any)
        // Attributes (if any)
        public const string ClassTemplate = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{{
    /// <summary>
    /// {0} - {1}
    /// </summary>
    public class {2} : IdmResource
    {{
        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public {2}()
        {{
            ObjectType = ForcedObjType = ""{0}"";
        }}

        /// <summary>
        /// Build a {2} object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public {2}(IdmResource resource)
        {{
            ObjectType = ForcedObjType = ""{0}"";
            Attributes = resource.Attributes;
            if (resource.Creator == null)
                return;
            Creator = resource.Creator;
        }}

        readonly string ForcedObjType;

        /// <summary>
        /// Object Type (can only be {0})
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {{
            get {{ return GetAttrValue(""ObjectType""); }}
            set
            {{
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of {2} can only be '{0}'"");
                SetAttrValue(""ObjectType"", value);
            }}
        }}
{3}
    }}
}}
";

    }
}

namespace IdmGenerateModels
{
    public static class Templates
    {

        public const string SingleValuedStringFormat = @"
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

        public const string SingleValuedValueFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public {4} {3}
        {{
            get {{ return AttrToBool(""{3}""); }}
            set {{ SetAttrValue(""{3}"", value.ToString()); }}
        }}

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
    public class {0} : IdmResource
    {{
        {2}/// <summary>
        /// Parameterless CTOR
        /// </summary>
        public {0}()
        {{
            ObjectType = ForcedObjType = ""{0}"";
        }}

        /// <summary>
        /// Build a {0} object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public {0}(IdmResource resource)
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
                    throw new InvalidOperationException(""Object Type of {0} can only be '{0}'"");
                SetAttrValue(""ObjectType"", value);
            }}
        }}
{3}{4}
    }}
}}
";

    }
}

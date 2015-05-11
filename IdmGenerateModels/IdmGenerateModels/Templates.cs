namespace IdmGenerateModels
{
    public static class Templates
    {
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
            : base(resource)
        {{
            ObjectType = ForcedObjType = ""{0}"";
            Clone(resource);
        }}

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

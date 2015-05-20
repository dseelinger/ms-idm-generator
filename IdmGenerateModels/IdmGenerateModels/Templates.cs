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

        public const string SingleValuedStringTestsFormat = @"
        [TestMethod]
        public void It_can_get_and_set_{0}()
        {{
            // Act
            _it.{0} = ""{1}"";

            // Assert
            Assert.AreEqual(""{1}"", _it.{0});
        }}

{2}";

        public const string NonMatchTest = @"        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_{0}_set_to_invalid_value()
        {{
            // Act
            _it.{0} = ""{1}"";
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

        public const string MultiValuedDateTimeFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<DateTime> {4}
        {{
            get {{ return GetAttr(""{3}"").Values.Select(DateTime.Parse).ToList(); }}
            set {{ SetAttrValues(""{3}"", value.Select(v => v.ToString()); }}
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
        {2}public DateTime{5} {4}
        {{
            get {{ return AttrTo{6}DateTime(""{3}""); }}
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

        public const string MultiValuedBinaryFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<byte[]> {4}
        {{
            get {{ return GetAttr(""{3}"").ToBinaries(); }}
            set {{ SetAttrValues(""{3}"", value.Select(Convert.ToBase64String).ToList()); }}
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

        public const string MultiValuedReferenceFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        public List<{3}> {4}
        {{
            get {{ return GetMultiValuedAttr(""{2}"", _the{4}); }}
            set {{ SetMultiValuedAttr(""{2}"", out _the{4}, value); }}
        }}
        private List<{3}> _the{4};

";


        public const string ClassTemplate = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            Attributes = resource.Attributes;
            ObjectType = ForcedObjType = ""{0}"";
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




        public const string TestClassTemplate = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{{
    [TestClass]
    public class {0}Tests
    {{
        private {0} _it;

        public {0}Tests()
        {{
            _it = new {0}();
        }}

        [TestMethod]
        public void It_has_a_paremeterless_constructor()
        {{
            Assert.AreEqual(""{1}"", _it.ObjectType);
        }}

        [TestMethod]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {{
            var resource = new IdmResource
            {{
                DisplayName = ""My Display Name"",
                Creator = new Person {{ DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""}},
            }};
            var it = new {0}(resource);

            Assert.AreEqual(""{1}"", it.ObjectType);
            Assert.AreEqual(""My Display Name"", it.DisplayName);
            Assert.AreEqual(""Creator Display Name"", it.Creator.DisplayName);
        }}

        [TestMethod]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {{
            var resource = new IdmResource
            {{
                DisplayName = ""My Display Name"",
            }};
            var it = new {0}(resource);

            Assert.AreEqual(""My Display Name"", it.DisplayName);
            Assert.IsNull(it.Creator);
        }}

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {{
            _it.ObjectType = ""Invalid Object Type"";
        }}
{2}
    }}
}}
";


    }
}

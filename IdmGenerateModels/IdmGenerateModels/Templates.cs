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
            _it.{0} = @""{1}"";
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

        public const string SingleValuedValueTestsFormat = @"{2}
        [TestMethod]
        public void It_can_get_and_set_{0}()
        {{
            // Act
            _it.{0} = {1};

            // Assert
            Assert.AreEqual({1}, _it.{0});
        }}

";

        public const string SingleValuedValueNullTestFormat = @"
        [TestMethod]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            Assert.IsNull(_it.{0});
        }}

        [TestMethod]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            _it.{0} = {1};

            // Act
            _it.{0} = null;

            // Assert
            Assert.IsNull(_it.{0});
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
            set {{ SetAttrValue(""{3}"", value == null ? null : Convert.ToBase64String(value)); }}
        }}

";

        public const string SingleValuedBinaryTestsFormat = @"
        [TestMethod]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            Assert.IsNull(_it.{0});
        }}

        [TestMethod]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            _it.{0} = byteArray; 

            // Act
            _it.{0} = null;

            // Assert
            Assert.IsNull(_it.{0});
        }}

        [TestMethod]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);

            // Act
            _it.{0} = byteArray; 

            // Assert
            Assert.AreEqual(byteArray[0], _it.{0}[0]);
            Assert.AreEqual(byteArray[1], _it.{0}[1]);
            Assert.AreEqual(byteArray[2], _it.{0}[2]);
            Assert.AreEqual(byteArray[byteArray.Length - 1], _it.{0}[_it.{0}.Length - 1]);
        }}

";

        public const string MultiValuedBinaryFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<byte[]> {4}
        {{
            get {{ return GetAttr(""{3}"") == null ? new List<byte[]>() : GetAttr(""{3}"").ToBinaries(); }}
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

        public const string SingleValuedReferenceTestsFormat = @"
        [TestMethod]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            Assert.IsNull(_it.{0});
        }}

        [TestMethod]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            var test{1} = new {1} {{ DisplayName = ""Test {1}"" }};			
            _it.{0} = test{1}; 

            // Act
            _it.{0} = null;

            // Assert
            Assert.IsNull(_it.{0});
        }}

        [TestMethod]
        public void It_can_get_and_set_{0}()
        {{
            // Act
			var test{1} = new {1} {{ DisplayName = ""Test {1}"" }};			
            _it.{0} = test{1}; 

            // Assert
            Assert.AreEqual(test{1}.DisplayName, _it.{0}.DisplayName);
        }}

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

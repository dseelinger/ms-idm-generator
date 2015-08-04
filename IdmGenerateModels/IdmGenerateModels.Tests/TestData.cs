namespace IdmGenerateModels.Tests
{
    public static class TestData
    {
        public const string EmptyClassOutput = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{
    /// <summary>
    /// Foo-Bar - Bat
    /// </summary>
    public class Foo_Bar : IdmResource
    {
        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public Foo_Bar()
        {
            ObjectType = ForcedObjType = ""Foo-Bar"";
        }

        /// <summary>
        /// Build a Foo_Bar object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public Foo_Bar(IdmResource resource)
        {
            Attributes = resource.Attributes;
            ObjectType = ForcedObjType = ""Foo-Bar"";
            if (resource.Creator == null)
                return;
            Creator = resource.Creator;
        }

        readonly string ForcedObjType;

        /// <summary>
        /// Object Type (can only be Foo-Bar)
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {
            get { return GetAttrValue(""ObjectType""); }
            set
            {
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of Foo_Bar can only be 'Foo-Bar'"");
                SetAttrValue(""ObjectType"", value);
            }
        }

    }
}
";


        public const string ClassWithDash = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{
    /// <summary>
    /// Foo-Bar - Bar
    /// </summary>
    public class Foo_Bar : IdmResource
    {
        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public Foo_Bar()
        {
            ObjectType = ForcedObjType = ""Foo-Bar"";
        }

        /// <summary>
        /// Build a Foo_Bar object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public Foo_Bar(IdmResource resource)
        {
            Attributes = resource.Attributes;
            ObjectType = ForcedObjType = ""Foo-Bar"";
            if (resource.Creator == null)
                return;
            Creator = resource.Creator;
        }

        readonly string ForcedObjType;

        /// <summary>
        /// Object Type (can only be Foo-Bar)
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {
            get { return GetAttrValue(""ObjectType""); }
            set
            {
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of Foo_Bar can only be 'Foo-Bar'"");
                SetAttrValue(""ObjectType"", value);
            }
        }

    }
}
";


        public const string GroupExpectedOutput = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{
    /// <summary>
    /// Group class - covers both Security Groups and Distribution Lists
    /// </summary>
    public class Group : SecurityIdentifierResource
    {
        private Person _displayedOwner;
        private List<Person> _owners;
        private List<SecurityIdentifierResource> _computedMembers;
        private List<SecurityIdentifierResource> _explicitMembers;

        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public Group()
        {
            ObjectType = ForcedObjType = ""Group"";
        }

        /// <summary>
        /// Build a Group object from a SecurityIdentifierResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public Group(SecurityIdentifierResource resource)
            : base(resource)
        {
            ObjectType = ForcedObjType = ""Group"";
            Clone(resource);
        }

        /// <summary>
        /// Construct a Group from an IdMResource
        /// </summary>
        /// <param name=""resource"">base class</param>
        public Group(IdmResource resource)
            : base(resource)
        {
            ObjectType = ForcedObjType = ""Group"";
            var identifierResource = resource as SecurityIdentifierResource;
            if (identifierResource != null)
                Clone(identifierResource);
        }


        /// <summary>
        /// Object Type (can only be Group)
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {
            get { return GetAttrValue(""ObjectType""); }
            set
            {
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of Person can only be 'Person'"");
                SetAttrValue(""ObjectType"", value);
            }
        }

        /// <summary>
        /// List of owners of the Group
        /// </summary>
        public List<Person> Owner
        {
            get { return GetMultiValuedAttr(""Owner"", _owners); }
            set { SetMultiValuedAttr(""Owner"", out _owners, value); }
        }

        /// <summary>
        /// Resources in the set that are computed from the membership filter.  List of resources that are currently 
        /// computed as being members of the group. Should only be retrieved from Identity Manger.  Use ExplicitMember
        /// to manually add members to the group.
        /// </summary>
        public List<SecurityIdentifierResource> ComputedMember
        {
            get { return GetMultiValuedAttr(""ComputedMember"", _computedMembers); }
            set { SetMultiValuedAttr(""ComputedMember"", out _computedMembers, value); }
        }

        /// <summary>
        /// Determines when evaluation of the group happens with respect to request processing - real-time or deferred.
        /// </summary>
        public bool? msidmDeferredEvaluation
        {
            get { return AttrToBool(""msidmDeferredEvaluation""); }
            set { SetAttrValue(""msidmDeferredEvaluation"", value.ToString()); }
        }

        /// <summary>
        /// Single Person who is listed as the owner of the group
        /// </summary>
        public Person DisplayedOwner
        {
            get { return GetAttr(""DisplayedOwner"", _displayedOwner); }
            set
            {
                _displayedOwner = value;
                SetAttrValue(""DisplayedOwner"", ObjectIdOrNull(value));
            }
        }

        /// <summary>
        /// A predicate defining a subset of the resources.
        /// </summary>
        public string Filter
        {
            get { return GetAttrValue(""Filter""); }
            set { SetAttrValue(""Filter"", value); }
        }

        /// <summary>
        /// (aka Manually-managed Membership) Members in the group that are manually managed.
        /// </summary>
        public List<SecurityIdentifierResource> ExplicitMember
        {
            get { return GetMultiValuedAttr(""ExplicitMember"", _explicitMembers); }
            set { SetMultiValuedAttr(""ExplicitMember"", out _explicitMembers, value); }
        }

        /// <summary>
        /// Membership Add Workflow - Workflow that fires when someone is manually added to the group. Note that this
        /// is a string and not a reference object in the Identity Manager Schema.
        /// </summary>
        [Required]
        public string MembershipAddWorkflow
        {
            get { return GetAttrValue(""MembershipAddWorkflow""); }
            set { SetAttrValue(""MembershipAddWorkflow"", value); }
        }

        /// <summary>
        /// If true, no one may be manually added to the group.
        /// </summary>
        [Required]
        public bool? MembershipLocked
        {
            get { return AttrToBool(""MembershipLocked""); }
            set { SetAttrValue(""MembershipLocked"", value.ToString()); }
        }

        /// <summary>
        /// Group scope.  Must be one of (DomainLocal|Global|Universal)
        /// </summary>
        [RegularExpression(""^(DomainLocal|Global|Universal)$"")]
        [Required]
        public string Scope
        {
            get { return GetAttrValue(""Scope""); }
            set { SetAttrValue(""Scope"", value); }
        }

        /// <summary>
        /// Defined by a filter that matches resources based on date and time attributes
        /// </summary>
        public bool? Temporal
        {
            get { return AttrToBool(""Temporal""); }
            set { SetAttrValue(""Temporal"", value.ToString()); }
        }
    }
}
";

        public const string BindingWithDifferentDisplayName = @"
        /// <summary>
        /// Binding Display Name - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string BindingWithDifferentDisplayNameTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            _it.PropertyName = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string BindingWithDifferentDescription = @"
        /// <summary>
        /// First Choice for Summary Part I - Binding Description
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string BindingWithDifferentDescriptionTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            _it.PropertyName = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string BindingNotRequired = @"
        /// <summary>
        /// First Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""8"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '8'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string StringAttributeWithDifferentPropertyName = @"
        /// <summary>
        /// First Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName2
        {
            get { return GetAttrValue(""PropertyName2""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName2.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName2"", value); 
            }
        }

";

        public const string StringAttributeWithDifferentPropertyNameTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName2()
        {
            // Act
            _it.PropertyName2 = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName2_set_to_invalid_value()
        {
            // Act
            _it.PropertyName2 = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string StringAttributeWithoutBindingDisplayName = @"
        /// <summary>
        /// Second Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string StringAttributeWithoutBindingDisplayNameTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            _it.PropertyName = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string StringAttributeWithoutABindingDescription = @"
        /// <summary>
        /// First Choice for Summary Part I - Second Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string StringAttributeWithoutABindingDescriptionTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            _it.PropertyName = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string StringAttributeWithoutRegEx= @"
        /// <summary>
        /// First Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string StringAttributeWithoutRegExTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""A string"";

            // Assert
            Assert.AreEqual(""A string"", _it.PropertyName);
        }

";

        public const string StringAttributeWithAllStringFieldsPopulated = @"
        /// <summary>
        /// First Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string PropertyName
        {
            get { return GetAttrValue(""PropertyName""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression 'abc'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

        public const string StringAttributeWithAllStringFieldsPopulatedTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            Assert.AreEqual(""abc"", _it.PropertyName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            _it.PropertyName = @""flkj3332@!!!$

fd333
"";
        }

";

        public const string BoolAttribute = @"
        /// <summary>
        /// Boolean Attrbute - A boolean attribute
        /// </summary>
        [Required]
        public bool PropertyName
        {
            get { return AttrToBool(""PropertyName""); }
            set { 
                SetAttrValue(""PropertyName"", value.ToString());
            }
        }

";

        public const string BoolAttributeTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = true;

            // Assert
            Assert.AreEqual(true, _it.PropertyName);
        }

";

        public const string IntegerAttributeWithMin = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public int Property_Name
        {
            get { return AttrToInteger(""Property-Name""); }
            set { 
                if (value < 5)
                    throw new ArgumentException(""Invalid value for Property-Name.  Minimum value is 5"");
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";

        public const string IntegerAttributeWithMax = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public int Property_Name
        {
            get { return AttrToInteger(""Property-Name""); }
            set { 
                if (value > 5)
                    throw new ArgumentException(""Invalid value for Property-Name.  Maximum value is 5"");
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";

        public const string IntegerAttributeWithMinMax = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public int Property_Name
        {
            get { return AttrToInteger(""Property-Name""); }
            set { 
                if (value < 2)
                    throw new ArgumentException(""Invalid value for Property-Name.  Minimum value is 2"");
                if (value > 5)
                    throw new ArgumentException(""Invalid value for Property-Name.  Maximum value is 5"");
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";

        public const string StandardReferenceAttributeWhereTypeMatchesName = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public PropertyName PropertyName
        {
            get { return GetAttr(""PropertyName"", _thePropertyName); }
            set 
            { 
                _thePropertyName = value;
                SetAttrValue(""PropertyName"", ObjectIdOrNull(value)); 
            }
        }
        private PropertyName _thePropertyName;

";

        public const string StandardReferenceAttributeWhereTypeFoundInJsonFile = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public ModelType PropertyName
        {
            get { return GetAttr(""PropertyName"", _thePropertyName); }
            set 
            { 
                _thePropertyName = value;
                SetAttrValue(""PropertyName"", ObjectIdOrNull(value)); 
            }
        }
        private ModelType _thePropertyName;

";

        public const string ReferenceAttributeWhereTypeNotFound = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public IdmResource PropertyName2
        {
            get { return GetAttr(""PropertyName2"", _thePropertyName2); }
            set 
            { 
                _thePropertyName2 = value;
                SetAttrValue(""PropertyName2"", ObjectIdOrNull(value)); 
            }
        }
        private IdmResource _thePropertyName2;

";

        public const string BinaryAttribute = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        [Required]
        public byte[] PropertyName
        {
            get { return GetAttr(""PropertyName"") == null ? null : GetAttr(""PropertyName"").ToBinary(); }
            set { SetAttrValue(""PropertyName"", Convert.ToBase64String(value)); }
        }

";

        public const string StringAttributeWithDashInName = @"
        /// <summary>
        /// First Choice for Summary Part I - First Choice for Summary Part II
        /// </summary>
        [Required]
        public string Property_Name
        {
            get { return GetAttrValue(""Property-Name""); }
            set {
                var regEx = new RegEx(""abc"");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for Property-Name.  Must match regular expression 'abc'"");
                SetAttrValue(""Property-Name"", value); 
            }
        }

";

        public const string BoolAttributeWithDash = @"
        /// <summary>
        /// Boolean Attrbute - A boolean attribute
        /// </summary>
        [Required]
        public bool Property_Name
        {
            get { return AttrToBool(""Property-Name""); }
            set { 
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";

        public const string ReferenceAttrWithDashInName = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public Property_Name Property_Name
        {
            get { return GetAttr(""Property-Name"", _theProperty_Name); }
            set 
            { 
                _theProperty_Name = value;
                SetAttrValue(""Property-Name"", ObjectIdOrNull(value)); 
            }
        }
        private Property_Name _theProperty_Name;

";

        public const string BinaryAttributeWithDash = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        [Required]
        public byte[] Property_Name
        {
            get { return GetAttr(""Property-Name"") == null ? null : GetAttr(""Property-Name"").ToBinary(); }
            set { SetAttrValue(""Property-Name"", Convert.ToBase64String(value)); }
        }

";

        public const string MultiValuedString = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public List<string> Property_Name
        {
            get { return GetAttrValues(""Property-Name""); }
            set {
                var regEx = new RegEx(""[0-9]"");
                if (value.Any(x => !regEx.IsMatch(x))
                    throw new ArgumentException(""One or more invalid values for Property-Name.  Each value must match regular expression '[0-9]'"");
                SetAttrValues(""Property-Name"", value); 
            }
        }

";

        public const string MultiValuedIntegerAttributeWithMinMax = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public List<int> Property_Name
        {
            get { return GetAttr(""Property-Name"").Values.Select(int.Parse).ToList(); }
            set {
                if (value.Any( v => v < 2))
                    throw new ArgumentException(""One or more invalid values for Property-Name.  Minimum value for each is 2"");
                if (value.Any( v => v > 5))
                    throw new ArgumentException(""One or more invalid values for Property-Name.  Maximum value for each is 5"");
                SetAttrValues(""Property-Name"", value.Select(v => v.ToString());
            }
        }

";

        public const string MultiValuedDateTime = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public List<DateTime> Property_Name
        {
            get { return GetAttr(""Property-Name"").Values.Select(DateTime.Parse).ToList(); }
            set { SetAttrValues(""Property-Name"", value.Select(v => v.ToString()); }
        }

";

        public const string MultiValuedReference = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        public List<Model_Type> Property_Name
        {
            get { return GetMultiValuedAttr(""Property-Name"", _theProperty_Name); }
            set { SetMultiValuedAttr(""Property-Name"", out _theProperty_Name, value); }
        }
        private List<Model_Type> _theProperty_Name;

";

        public const string MultiValuedBinary = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public List<byte[]> Property_Name
        {
            get { return GetAttr(""Property-Name"") == null ? new List<byte[]>() : GetAttr(""Property-Name"").ToBinaries(); }
            set { SetAttrValues(""Property-Name"", value.Select(Convert.ToBase64String).ToList()); }
        }

";

        public const string IntegerAttribute = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public int PropertyName
        {
            get { return AttrToInteger(""PropertyName""); }
            set { 
                SetAttrValue(""PropertyName"", value.ToString());
            }
        }

";

        public const string IntegerAttributeNotRequired = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        public int? PropertyName
        {
            get { return AttrToNullableInteger(""PropertyName""); }
            set { 
                SetAttrValue(""PropertyName"", value.ToString());
            }
        }

";

        public const string DateTimeAttributeWithDash = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public DateTime Property_Name
        {
            get { return AttrToDateTime(""Property-Name""); }
            set { SetAttrValue(""Property-Name"", value.ToString()); }
        }

";

        public const string DateTimeAttribute = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public DateTime PropertyName
        {
            get { return AttrToDateTime(""PropertyName""); }
            set { SetAttrValue(""PropertyName"", value.ToString()); }
        }

";

        public const string DateTimeAttributeOptional = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        public DateTime? PropertyName
        {
            get { return AttrToNullableDateTime(""PropertyName""); }
            set { SetAttrValue(""PropertyName"", value.ToString()); }
        }

";




        public const string TestClassTemplate = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{
    [TestClass]
    public class Foo_BarTests
    {
        private Foo_Bar _it;

        public Foo_BarTests()
        {
            _it = new Foo_Bar();
        }

        [TestMethod]
        public void It_has_a_paremeterless_constructor()
        {
            Assert.AreEqual(""Foo-Bar"", _it.ObjectType);
        }

        [TestMethod]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
                Creator = new Person { DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""},
            };
            var it = new Foo_Bar(resource);

            Assert.AreEqual(""Foo-Bar"", it.ObjectType);
            Assert.AreEqual(""My Display Name"", it.DisplayName);
            Assert.AreEqual(""Creator Display Name"", it.Creator.DisplayName);
        }

        [TestMethod]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
            };
            var it = new Foo_Bar(resource);

            Assert.AreEqual(""My Display Name"", it.DisplayName);
            Assert.IsNull(it.Creator);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {
            _it.ObjectType = ""Invalid Object Type"";
        }

    }
}
";

        public const string ClassOutputWithMultivaluedBinaryAttribute = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{
    /// <summary>
    /// Foo - Bar
    /// </summary>
    public class Foo : IdmResource
    {
        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public Foo()
        {
            ObjectType = ForcedObjType = ""Foo"";
        }

        /// <summary>
        /// Build a Foo object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public Foo(IdmResource resource)
        {
            Attributes = resource.Attributes;
            ObjectType = ForcedObjType = ""Foo"";
            if (resource.Creator == null)
                return;
            Creator = resource.Creator;
        }

        readonly string ForcedObjType;

        /// <summary>
        /// Object Type (can only be Foo)
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {
            get { return GetAttrValue(""ObjectType""); }
            set
            {
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of Foo can only be 'Foo'"");
                SetAttrValue(""ObjectType"", value);
            }
        }

        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        [Required]
        public List<byte[]> Property_Name
        {
            get { return GetAttr(""Property-Name"") == null ? new List<byte[]>() : GetAttr(""Property-Name"").ToBinaries(); }
            set { SetAttrValues(""Property-Name"", value.Select(Convert.ToBase64String).ToList()); }
        }


    }
}
";


        public const string BindingNotRequiredTests = @"
        [TestMethod]
        public void It_can_get_and_set_PropertyName()
        {{
            // Act
            _it.PropertyName = ""{0}"";

            // Assert
            Assert.AreEqual(""{0}"", _it.PropertyName);
        }}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {{
            // Act
            _it.PropertyName = @""{1}"";
        }}

";

    }
}










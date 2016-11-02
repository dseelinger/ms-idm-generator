﻿namespace IdmGenerateModels.Tests
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

        public const string ClassWithDashTests = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Xunit;
using FluentAssertions;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{
    public class Foo_BarTests
    {
        private Foo_Bar _it;

        public Foo_BarTests()
        {
            _it = new Foo_Bar();
        }

        [Fact]
        public void It_has_a_paremeterless_constructor()
        {
            _it.ObjectType.Should().Be(""Foo-Bar"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
                Creator = new Person { DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""},
            };
            var it = new Foo_Bar(resource);

            it.ObjectType.Should().Be(""Foo-Bar"");
            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.DisplayName.Should().Be(""Creator Display Name"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
            };
            var it = new Foo_Bar(resource);

            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.Should().BeNull();
        }

        [Fact]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {
            Action action = () => _it.ObjectType = ""Invalid Object Type"";
            action.ShouldThrow<InvalidOperationException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            _it.PropertyName.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            _it.PropertyName.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName2()
        {
            // Act
            _it.PropertyName2 = ""abc"";

            // Assert
            _it.PropertyName2.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName2_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName2 = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            _it.PropertyName.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            _it.PropertyName.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""A string"";

            // Assert
            _it.PropertyName.Should().Be(""A string"");
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = ""abc"";

            // Assert
            _it.PropertyName.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.PropertyName = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = true;

            // Assert
            _it.PropertyName.Should().Be(true);
        }

";

        public const string BoolAttributeNotRequired = @"
        /// <summary>
        /// Boolean Attrbute - A boolean attribute
        /// </summary>
        public bool? PropertyName
        {
            get { return AttrToNullableBool(""PropertyName""); }
            set { 
                SetAttrValue(""PropertyName"", value.ToString());
            }
        }

";

        public const string BoolAttributeNotRequiredTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            _it.PropertyName = true;

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = true;

            // Assert
            _it.PropertyName.Should().Be(true);
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
        public const string IntegerAttributeWithMinTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = 5;

            // Assert
            _it.Property_Name.Should().Be(5);
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_small()
        {
            // Act
            Action action = () => _it.Property_Name = 4;
            action.ShouldThrow<ArgumentException>();
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
                if (value > 10)
                    throw new ArgumentException(""Invalid value for Property-Name.  Maximum value is 10"");
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";

        public const string IntegerAttributeWithMaxTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = 10;

            // Assert
            _it.Property_Name.Should().Be(10);
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_big()
        {
            // Act
            Action action = () => _it.Property_Name = 11;
            action.ShouldThrow<ArgumentException>();
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

        public const string IntegerAttributeWithMinMaxNotRequired = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        public int? Property_Name
        {
            get { return AttrToNullableInteger(""Property-Name""); }
            set { 
                if (value < 2)
                    throw new ArgumentException(""Invalid value for Property-Name.  Minimum value is 2"");
                if (value > 5)
                    throw new ArgumentException(""Invalid value for Property-Name.  Maximum value is 5"");
                SetAttrValue(""Property-Name"", value.ToString());
            }
        }

";
        public const string IntegerAttributeWithMinMaxTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = 5;

            // Assert
            _it.Property_Name.Should().Be(5);
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_small()
        {
            // Act
            Action action = () => _it.Property_Name = 1;
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_big()
        {
            // Act
            Action action = () => _it.Property_Name = 6;
            action.ShouldThrow<ArgumentException>();
        }

";

        public const string IntegerAttributeWithMinMaxNotRequiredTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            _it.Property_Name = 5;

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = 5;

            // Assert
            _it.Property_Name.Should().Be(5);
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_small()
        {
            // Act
            Action action = () => _it.Property_Name = 1;
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_big()
        {
            // Act
            Action action = () => _it.Property_Name = 6;
            action.ShouldThrow<ArgumentException>();
        }

";

        public const string StandardReferenceAttributeWhereTypeMatchesName = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public Person PropertyName
        {
            get { return GetAttr(""PropertyName"", _thePropertyName); }
            set 
            { 
                _thePropertyName = value;
                SetAttrValue(""PropertyName"", ObjectIdOrNull(value)); 
            }
        }
        private Person _thePropertyName;

";

        public const string StandardReferenceAttributeWhereTypeMatchesNameTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            var testPerson = new Person { DisplayName = ""Test Person"" };			
            _it.PropertyName = testPerson; 

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
			var testPerson = new Person { DisplayName = ""Test Person"" };			
            _it.PropertyName = testPerson; 

            // Assert
            _it.PropertyName.DisplayName.Should().Be(testPerson.DisplayName);
        }

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

        public const string StandardReferenceAttributeWhereTypeFoundInJsonFileTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            var testModelType = new ModelType { DisplayName = ""Test ModelType"" };			
            _it.PropertyName = testModelType; 

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
			var testModelType = new ModelType { DisplayName = ""Test ModelType"" };			
            _it.PropertyName = testModelType; 

            // Assert
            _it.PropertyName.DisplayName.Should().Be(testModelType.DisplayName);
        }

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

        public const string ReferenceAttributeWhereTypeNotFoundTests = @"
        [Fact]
        public void It_has_PropertyName2_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName2.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName2_which_can_be_set_back_to_null()
        {
            // Arrange
            var testIdmResource = new IdmResource { DisplayName = ""Test IdmResource"" };			
            _it.PropertyName2 = testIdmResource; 

            // Act
            _it.PropertyName2 = null;

            // Assert
            _it.PropertyName2.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName2()
        {
            // Act
			var testIdmResource = new IdmResource { DisplayName = ""Test IdmResource"" };			
            _it.PropertyName2 = testIdmResource; 

            // Assert
            _it.PropertyName2.DisplayName.Should().Be(testIdmResource.DisplayName);
        }

";

        public const string BinaryAttribute = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        public byte[] PropertyName
        {
            get { return GetAttr(""PropertyName"") == null ? null : GetAttr(""PropertyName"").ToBinary(); }
            set { SetAttrValue(""PropertyName"", value == null ? null : Convert.ToBase64String(value)); }
        }

";

        public const string BinaryAttributeRequired = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        [Required]
        public byte[] PropertyName
        {
            get { return GetAttr(""PropertyName"") == null ? null : GetAttr(""PropertyName"").ToBinary(); }
            set { SetAttrValue(""PropertyName"", value == null ? null : Convert.ToBase64String(value)); }
        }

";

        public const string BinaryAttributeTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            _it.PropertyName = byteArray; 

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);

            // Act
            _it.PropertyName = byteArray; 

            // Assert
            _it.PropertyName[0].Should().Be(byteArray[0]);
            _it.PropertyName[1].Should().Be(byteArray[1]);
            _it.PropertyName[2].Should().Be(byteArray[2]);
            _it.PropertyName[_it.PropertyName.Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }

";

        public const string BinaryAttributeTestsRequired = @"
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);

            // Act
            _it.PropertyName = byteArray; 

            // Assert
            _it.PropertyName[0].Should().Be(byteArray[0]);
            _it.PropertyName[1].Should().Be(byteArray[1]);
            _it.PropertyName[2].Should().Be(byteArray[2]);
            _it.PropertyName[_it.PropertyName.Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }

";

        public const string MultivaluedBinaryTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            var list = new List<byte[]> {
                byteArray,
                byteArray
            };
            _it.Property_Name = list; 

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            var list = new List<byte[]> {
                byteArray,
                byteArray
            };

            // Act
            _it.Property_Name = list; 

            // Assert
            _it.Property_Name[0][0].Should().Be(byteArray[0]);
            _it.Property_Name[0][1].Should().Be(byteArray[1]);
            _it.Property_Name[0][2].Should().Be(byteArray[2]);
            _it.Property_Name[1][0].Should().Be(byteArray[0]);
            _it.Property_Name[1][1].Should().Be(byteArray[1]);
            _it.Property_Name[1][2].Should().Be(byteArray[2]);
            _it.Property_Name[0][_it.Property_Name[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
            _it.Property_Name[1][_it.Property_Name[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
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

        public const string StringAttributeWithDashInNameTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = ""abc"";

            // Assert
            _it.Property_Name.Should().Be(""abc"");
        }

        [Fact]
        public void It_throws_when_Property_Name_set_to_invalid_value()
        {
            // Act
            Action action = () => _it.Property_Name = @""flkj3332@!!!$

fd333
"";
            action.ShouldThrow<ArgumentException>();
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

        public const string BoolAttributeWithDashTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            _it.Property_Name = true;

            // Assert
            _it.Property_Name.Should().Be(true);
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

        public const string ReferenceAttrWithDashInNameTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var testProperty_Name = new Property_Name { DisplayName = ""Test Property_Name"" };			
            _it.Property_Name = testProperty_Name; 

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
			var testProperty_Name = new Property_Name { DisplayName = ""Test Property_Name"" };			
            _it.Property_Name = testProperty_Name; 

            // Assert
            _it.Property_Name.DisplayName.Should().Be(testProperty_Name.DisplayName);
        }

";

        public const string BinaryAttributeWithDash = @"
        /// <summary>
        /// Reference Attrbute - A standard reference attribute
        /// </summary>
        [Required]
        public byte[] Property_Name
        {
            get { return GetAttr(""Property-Name"") == null ? null : GetAttr(""Property-Name"").ToBinary(); }
            set { SetAttrValue(""Property-Name"", value == null ? null : Convert.ToBase64String(value)); }
        }

";

        public const string BinaryAttributeWithDashTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);

            // Act
            _it.Property_Name = byteArray; 

            // Assert
            _it.Property_Name[0].Should().Be(byteArray[0]);
            _it.Property_Name[1].Should().Be(byteArray[1]);
            _it.Property_Name[2].Should().Be(byteArray[2]);
            _it.Property_Name[_it.Property_Name.Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }

";

        public const string MultiValuedStringRequired = @"
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

        public const string MultiValuedStringNonRequired = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
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

        public const string MultiValuedStringTestsRequired = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            var subObject1 = ""foo1"";
            var subObject2 = ""foo2"";
            var list = new List<string> { subObject1, subObject2 };

            // Act
            _it.Property_Name = list; 

            // Assert
            _it.Property_Name[0].Should().Be(""foo1"");
            _it.Property_Name[1].Should().Be(""foo2"");
        }

";

        public const string MultiValuedStringTestsNonRequired = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var subObject1 = ""foo1"";
            var subObject2 = ""foo2"";
            var list = new List<string> { subObject1, subObject2 };
            _it.Property_Name = list; 

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            var subObject1 = ""foo1"";
            var subObject2 = ""foo2"";
            var list = new List<string> { subObject1, subObject2 };

            // Act
            _it.Property_Name = list; 

            // Assert
            _it.Property_Name[0].Should().Be(""foo1"");
            _it.Property_Name[1].Should().Be(""foo2"");
        }

";

        public const string MultiValuedIntegerAttributeWithMinMax = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
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

        public const string MultiValuedIntegerAttributeWithMinMaxTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var subObject1 = 5;
            var subObject2 = 5;
            var list = new List<int> { subObject1, subObject2 };
            _it.Property_Name = list; 

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var subObject1 = 5;
            var subObject2 = 5;
            var list = new List<int> { subObject1, subObject2 };

            // Act
            _it.Property_Name = list;

            // Assert
            _it.Property_Name[0].Should().Be(""foo1"");
            _it.Property_Name[1].Should().Be(""foo2"");
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_small()
        {
            // Arrange
            var subObject1 = 1;
            var subObject2 = 1;
            var list = new List<int> { subObject1, subObject2 };

            // Act
            Action action = () => _it.Property_Name = list;
            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void It_throws_when_Property_Name_is_too_big()
        {
            // Arrange
            var subObject1 = 6;
            var subObject2 = 6;
            var list = new List<int> { subObject1, subObject2 };

            // Act
            Action action = () => _it.Property_Name = list;
            action.ShouldThrow<ArgumentException>();
        }

";

        public const string MultiValuedDateTime = @"
        /// <summary>
        /// My Display Name - My Description
        /// </summary>
        public List<DateTime> Property_Name
        {
            get { return GetAttr(""Property-Name"").Values.Select(DateTime.Parse).ToList(); }
            set { SetAttrValues(""Property-Name"", value.Select(v => v.ToString()); }
        }

";

        public const string MultiValuedDateTimeTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var now = DateTime.Now;
            var testTime1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var later = DateTime.Now.AddHours(1);
            var testTime2 = new DateTime(later.Year, later.Month, later.Day, later.Hour, later.Minute, later.Second);
            _it.Property_Name[0] = testTime1;
            _it.Property_Name[1] = testTime2;

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var now = DateTime.Now;
            var testTime1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var later = DateTime.Now.AddHours(1);
            var testTime2 = new DateTime(later.Year, later.Month, later.Day, later.Hour, later.Minute, later.Second);

            // Act
            _it.Property_Name[0] = testTime1;
            _it.Property_Name[1] = testTime2;

            // Assert
            _it.Property_Name[0].Should().Be(testTime1);
            _it.Property_Name[1].Should().Be(testTime2);
        }

";

        public const string MultiValuedReferenceTests = @"
        [Fact]
        public void It_has_Property_Name_which_is_null_by_default()
        {
            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_has_Property_Name_which_can_be_set_back_to_null()
        {
            // Arrange
            var list = new List<Model_Type>
            {
                new Model_Type { DisplayName = ""Test Model_Type1"", ObjectID = ""guid1"" },
                new Model_Type { DisplayName = ""Test Model_Type2"", ObjectID = ""guid2"" }
            };
            _it.Property_Name = list;

            // Act
            _it.Property_Name = null;

            // Assert
            _it.Property_Name.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var list = new List<Model_Type>
            {
                new Model_Type { DisplayName = ""Test Model_Type1"", ObjectID = ""guid1"" },
                new Model_Type { DisplayName = ""Test Model_Type2"", ObjectID = ""guid2"" }
            };

            // Act
            _it.Property_Name = list;

            // Assert
            _it.Property_Name[0].DisplayName.Should().Be(list[0].DisplayName);
            _it.Property_Name[1].DisplayName.Should().Be(list[1].DisplayName);
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
        public List<byte[]> Property_Name
        {
            get { return GetAttr(""Property-Name"")?.ToBinaries(); }
            set { SetAttrValues(""Property-Name"", value?.Select(Convert.ToBase64String).ToList()); }
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

        public const string IntegerAttributeTests = @"
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = 123;

            // Assert
            _it.PropertyName.Should().Be(123);
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

        public const string IntegerAttributeNotRequiredTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            _it.PropertyName = 123;

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            _it.PropertyName = 123;

            // Assert
            _it.PropertyName.Should().Be(123);
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
        public const string DateTimeAttributeWithDashTests = @"
        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Act
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.Property_Name = testTime;

            // Assert
            _it.Property_Name.Should().Be(testTime);
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

        public const string DateTimeAttributeTests = @"
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.PropertyName = testTime;

            // Assert
            _it.PropertyName.Should().Be(testTime);
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


        public const string DateTimeAttributeOptionalTests = @"
        [Fact]
        public void It_has_PropertyName_which_is_null_by_default()
        {
            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_has_PropertyName_which_can_be_set_back_to_null()
        {
            // Arrange
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.PropertyName = testTime;

            // Act
            _it.PropertyName = null;

            // Assert
            _it.PropertyName.Should().BeNull();
        }

        [Fact]
        public void It_can_get_and_set_PropertyName()
        {
            // Act
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.PropertyName = testTime;

            // Assert
            _it.PropertyName.Should().Be(testTime);
        }

";


        public const string TestClassTemplate = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Xunit;
using FluentAssertions;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{
    public class Foo_BarTests
    {
        private Foo_Bar _it;

        public Foo_BarTests()
        {
            _it = new Foo_Bar();
        }

        [Fact]
        public void It_has_a_paremeterless_constructor()
        {
            _it.ObjectType.Should().Be(""Foo-Bar"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
                Creator = new Person { DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""},
            };
            var it = new Foo_Bar(resource);

            it.ObjectType.Should().Be(""Foo-Bar"");
            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.DisplayName.Should().Be(""Creator Display Name"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
            };
            var it = new Foo_Bar(resource);

            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.Should().BeNull();
        }

        [Fact]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {
            Action action = () => _it.ObjectType = ""Invalid Object Type"";
            action.ShouldThrow<InvalidOperationException>();
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
            get { return GetAttr(""Property-Name"")?.ToBinaries(); }
            set { SetAttrValues(""Property-Name"", value?.Select(Convert.ToBase64String).ToList()); }
        }


    }
}
";

        public const string ClassOutputWithMultivaluedBinaryAttributeTests = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Xunit;
using FluentAssertions;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{
    public class FooTests
    {
        private Foo _it;

        public FooTests()
        {
            _it = new Foo();
        }

        [Fact]
        public void It_has_a_paremeterless_constructor()
        {
            _it.ObjectType.Should().Be(""Foo"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
                Creator = new Person { DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""},
            };
            var it = new Foo(resource);

            it.ObjectType.Should().Be(""Foo"");
            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.DisplayName.Should().Be(""Creator Display Name"");
        }

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {
            var resource = new IdmResource
            {
                DisplayName = ""My Display Name"",
            };
            var it = new Foo(resource);

            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.Should().BeNull();
        }

        [Fact]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {
            Action action = () => _it.ObjectType = ""Invalid Object Type"";
            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void It_can_get_and_set_Property_Name()
        {
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            var list = new List<byte[]> {
                byteArray,
                byteArray
            };

            // Act
            _it.Property_Name = list; 

            // Assert
            _it.Property_Name[0][0].Should().Be(byteArray[0]);
            _it.Property_Name[0][1].Should().Be(byteArray[1]);
            _it.Property_Name[0][2].Should().Be(byteArray[2]);
            _it.Property_Name[1][0].Should().Be(byteArray[0]);
            _it.Property_Name[1][1].Should().Be(byteArray[1]);
            _it.Property_Name[1][2].Should().Be(byteArray[2]);
            _it.Property_Name[0][_it.Property_Name[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
            _it.Property_Name[1][_it.Property_Name[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }


    }
}
";


        public const string BindingNotRequiredTests = @"
        [Fact]
        public void It_can_get_and_set_PropertyName()
        {{
            // Act
            _it.PropertyName = ""{0}"";

            // Assert
            _it.PropertyName.Should().Be(""{0}"");
        }}

        [Fact]
        public void It_throws_when_PropertyName_set_to_invalid_value()
        {{
            // Act
            Action action = () => _it.PropertyName = @""{1}"";
            action.ShouldThrow<ArgumentException>();
        }}

";

    }
}










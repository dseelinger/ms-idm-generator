namespace IdmGenerateModels.Tests
{
    public static class TestData
    {
        public const string EmptyClassOutput = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            : base(resource)
        {
            ObjectType = ForcedObjType = ""Foo"";
            Clone(resource);
        }

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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName"", value); 
            }
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName"", value); 
            }
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName2.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName2"", value); 
            }
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName"", value); 
            }
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName"", value); 
            }
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
                var regEx = new RegEx(""*."");
                if (!regEx.IsMatch(value))
                    throw new ArgumentException(""Invalid value for PropertyName.  Must match regular expression '*.'"");
                SetAttrValue(""PropertyName"", value); 
            }
        }

";

    }


}



using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a user action
    /// </summary>
    public partial class UserAction : BaseEntity
    {
        private ICollection<ActionCondition> _condition;
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets active action
        /// </summary>
        public bool Active { get; set; }


        /// <summary>
        /// Gets or sets the action Type
        /// </summary>
        public string ActionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the action conditions
        /// </summary>
        public int ConditionId { get; set; }

        public UserActionConditionEnum Condition
        {
            get { return (UserActionConditionEnum)ConditionId; }
            set { this.ConditionId = (int)value; }
        }


        /// <summary>
        /// Gets or sets the action conditions
        /// </summary>
        public int ReactionTypeId { get; set; }

        public UserReactionTypeEnum ReactionType
        {
            get { return (UserReactionTypeEnum)ReactionTypeId; }
            set { this.ReactionTypeId = (int)value; }
        }

        public string BannerId { get; set; }
        public string InteractiveFormId { get; set; }

        public string MessageTemplateId { get; set; }

        public string UserRoleId { get; set; }

        public string UserTagId { get; set; }
        /// <summary>
        /// Gets or sets the start date 
        /// </summary>
        public DateTime StartDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        public DateTime EndDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the user roles
        /// </summary>
        public virtual ICollection<ActionCondition> Conditions
        {
            get { return _condition ?? (_condition = new List<ActionCondition>()); }
            protected set { _condition = value; }
        }

        public partial class ActionCondition: SubBaseEntity
        {
            private ICollection<string> _notes;
            private ICollection<string> _notebooks;
            private ICollection<string> _userRoles;
            private ICollection<string> _userTags;
            private ICollection<UserRegister> _userRegister;
            private ICollection<UserRegister> _customUserAttributes;
            private ICollection<Url> _urlReferrer;
            private ICollection<Url> _urlCurrent;

            public string Name { get; set; }

            public int UserActionConditionTypeId { get; set; }

            public UserActionConditionTypeEnum UserActionConditionType
            {
                get { return (UserActionConditionTypeEnum)UserActionConditionTypeId; }
                set { this.UserActionConditionTypeId = (int)value; }
            }            

            public int ConditionId { get; set; }

            public UserActionConditionEnum Condition
            {
                get { return (UserActionConditionEnum)ConditionId; }
                set { this.ConditionId = (int)value; }
            }


            public virtual ICollection<string> Notes
            {
                get { return _notes ?? (_notes = new List<string>()); }
                protected set { _notes = value; }
            }

            public virtual ICollection<string> Notebooks
            {
                get { return _notebooks ?? (_notebooks = new List<string>()); }
                protected set { _notebooks = value; }
            }

            public virtual ICollection<string> UserRoles
            {
                get { return _userRoles ?? (_userRoles = new List<string>()); }
                protected set { _userRoles = value; }
            }
            public virtual ICollection<string> UserTags
            {
                get { return _userTags ?? (_userTags = new List<string>()); }
                protected set { _userTags = value; }
            }

            public virtual ICollection<UserRegister> UserRegistration
            {
                get { return _userRegister ?? (_userRegister = new List<UserRegister>()); }
                protected set { _userRegister = value; }
            }

            public virtual ICollection<UserRegister> CustomUserAttributes
            {
                get { return _customUserAttributes ?? (_customUserAttributes = new List<UserRegister>()); }
                protected set { _customUserAttributes = value; }
            }

            public virtual ICollection<Url> UrlReferrer
            {
                get { return _urlReferrer ?? (_urlReferrer = new List<Url>()); }
                protected set { _urlReferrer = value; }
            }

            public virtual ICollection<Url> UrlCurrent
            {
                get { return _urlCurrent ?? (_urlCurrent = new List<Url>()); }
                protected set { _urlCurrent = value; }
            }

            public partial class Url : SubBaseEntity
            {
                public string Name { get; set; }
            }

            public partial class UserRegister : SubBaseEntity
            {
                public string RegisterField { get; set; }
                public string RegisterValue { get; set; }
            }

        }

    }
}

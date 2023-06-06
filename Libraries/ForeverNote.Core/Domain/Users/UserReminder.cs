using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a user reminder 
    /// </summary>
    public partial class UserReminder : BaseEntity
    {
        private ICollection<ReminderCondition> _condition;

        private ICollection<ReminderLevel> _level;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public bool AllowRenew { get; set; }
        public int RenewedDay { get; set; }
        /// <summary>
        /// Gets or sets display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets active action
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the action conditions
        /// </summary>
        public int ConditionId { get; set; }

        public UserReminderConditionEnum Condition
        {
            get { return (UserReminderConditionEnum)ConditionId; }
            set { this.ConditionId = (int)value; }
        }

        /// <summary>
        /// Gets or sets the reminder rule
        /// </summary>
        public int ReminderRuleId { get; set; }

        public UserReminderRuleEnum ReminderRule
        {
            get { return (UserReminderRuleEnum)ReminderRuleId; }
            set { this.ReminderRuleId = (int)value; }
        }

        /// <summary>
        /// Gets or sets the user condition
        /// </summary>
        public virtual ICollection<ReminderCondition> Conditions
        {
            get { return _condition ?? (_condition = new List<ReminderCondition>()); }
            protected set { _condition = value; }
        }

        /// <summary>
        /// Gets or sets the reminder level
        /// </summary>
        public virtual ICollection<ReminderLevel> Levels
        {
            get { return _level ?? (_level = new List<ReminderLevel>()); }
            protected set { _level = value; }
        }


        public partial class ReminderCondition: SubBaseEntity
        {
            private ICollection<string> _notes;
            private ICollection<string> _notebooks;
            private ICollection<string> _userTags;
            private ICollection<UserRegister> _userRegister;
            private ICollection<UserRegister> _customUserAttributes;

            public string Name { get; set; }

            public int ConditionTypeId { get; set; }

            public UserReminderConditionTypeEnum ConditionType
            {
                get { return (UserReminderConditionTypeEnum)ConditionTypeId; }
                set { this.ConditionTypeId = (int)value; }
            }

            public int ConditionId { get; set; }

            public UserReminderConditionEnum Condition
            {
                get { return (UserReminderConditionEnum)ConditionId; }
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

            public partial class UserRegister: SubBaseEntity
            {
                public string RegisterField { get; set; }
                public string RegisterValue { get; set; }
            }

        }

        public partial class ReminderLevel: SubBaseEntity
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public int Day { get; set; }
            public int Hour { get; set; }
            public int Minutes { get; set; }
            public string EmailAccountId { get; set; }
            public string BccEmailAddresses { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
       
    }
}

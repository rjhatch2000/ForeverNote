using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a user reminder history
    /// </summary>
    public partial class UserReminderHistory : BaseEntity
    {
        private ICollection<HistoryLevel> _level;

        public string UserReminderId { get; set; }
        public string UserId { get; set; }
        public int ReminderRuleId { get; set; }

        public UserReminderRuleEnum ReminderRule
        {
            get { return (UserReminderRuleEnum)ReminderRuleId; }
            set { this.ReminderRuleId = (int)value; }
        }

        public int Status { get; set; }

        public UserReminderHistoryStatusEnum HistoryStatus
        {
            get { return (UserReminderHistoryStatusEnum)Status; }
            set { this.Status = (int)value; }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Gets or sets the reminder level history
        /// </summary>
        public virtual ICollection<HistoryLevel> Levels
        {
            get { return _level ?? (_level = new List<HistoryLevel>()); }
            protected set { _level = value; }
        }


        public partial class HistoryLevel : SubBaseEntity
        {
            public string ReminderLevelId { get; set; }
            public int Level { get; set; }
            public DateTime SendDate { get; set; }

        }

    }
}

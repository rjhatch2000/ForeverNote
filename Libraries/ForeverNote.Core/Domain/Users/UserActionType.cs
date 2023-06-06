using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a User ActionType
    /// </summary>
    public partial class UserActionType : BaseEntity
    {
        private ICollection<int> _conditionType;

        public string SystemKeyword { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<int> ConditionType
        {
            get { return _conditionType ?? (_conditionType = new List<int>()); }
            protected set { _conditionType = value; }
        }


    }


}

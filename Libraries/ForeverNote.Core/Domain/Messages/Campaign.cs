using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Messages
{
    //TODO: Is this needed? Or can I chop it out?
    /// <summary>
    /// Represents a campaign
    /// </summary>
    public partial class Campaign : BaseEntity
    {
        private ICollection<string> _userTags;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the lang identifier
        /// </summary>
        public string LanguageId { get; set; }

        public DateTime? UserCreatedDateFrom { get; set; }
        public DateTime? UserCreatedDateTo { get; set; }
        public DateTime? UserLastActivityDateFrom { get; set; }
        public DateTime? UserLastActivityDateTo { get; set; }

        /// <summary>
        /// Gets or sets the used email account identifier
        /// </summary>
        public string EmailAccountId { get; set; }
        /// <summary>
        /// Gets or sets the user tags
        /// </summary>
        public virtual ICollection<string> UserTags
        {
            get { return _userTags ?? (_userTags = new List<string>()); }
            protected set { _userTags = value; }
        }
    }
}

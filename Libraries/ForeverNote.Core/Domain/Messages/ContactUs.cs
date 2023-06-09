﻿namespace ForeverNote.Core.Domain.Messages
{
    /// <summary>
    /// Search term record (for statistics)
    /// </summary>
    public partial class ContactUs : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets enquiry
        /// </summary>
        public string Enquiry { get; set; }

        /// <summary>
        /// Gets or sets the email account identifier
        /// </summary>
        public string EmailAccountId { get; set; }

        /// <summary>
        /// Gets or sets the contact attribute description
        /// </summary>
        public string ContactAttributeDescription { get; set; }

        /// <summary>
        /// Gets or sets the contact attributes in XML format
        /// </summary>
        public string ContactAttributesXml { get; set; }

    }
}

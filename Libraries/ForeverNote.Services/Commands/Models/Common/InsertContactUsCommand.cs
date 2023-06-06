using MediatR;

namespace ForeverNote.Services.Commands.Models.Common
{
    public class InsertContactUsCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string Enquiry { get; set; }
        public string ContactAttributeDescription { get; set; }
        public string ContactAttributesXml { get; set; }
        public string EmailAccountId { get; set; }
    }

}

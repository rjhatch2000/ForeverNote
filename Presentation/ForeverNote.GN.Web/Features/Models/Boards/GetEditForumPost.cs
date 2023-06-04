using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetEditForumPost : IRequest<EditForumPostModel>
    {
        public Customer Customer { get; set; }
        public Forum Forum { get; set; }
        public ForumTopic ForumTopic { get; set; }
        public string Quote { get; set; }
    }
}

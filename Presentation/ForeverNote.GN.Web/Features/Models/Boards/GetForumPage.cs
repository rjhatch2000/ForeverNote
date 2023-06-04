using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetForumPage : IRequest<ForumPageModel>
    {
        public Customer Customer { get; set; }
        public Forum Forum { get; set; }
        public int PageNumber { get; set; }
    }
}

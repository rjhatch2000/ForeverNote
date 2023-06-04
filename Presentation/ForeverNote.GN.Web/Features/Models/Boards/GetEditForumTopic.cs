using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetEditForumTopic : IRequest<EditForumTopicModel>
    {
        public Customer Customer { get; set; }
        public Forum Forum { get; set; }
    }
}

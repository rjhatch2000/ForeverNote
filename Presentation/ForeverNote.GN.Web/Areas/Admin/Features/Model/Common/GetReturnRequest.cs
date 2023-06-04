using MediatR;
namespace ForeverNote.Web.Areas.Admin.Features.Model.Common
{
    public class GetReturnRequest : IRequest<int>
    {
        public int RequestStatusId { get; set; }
        public string StoreId { get; set; }
    }
}

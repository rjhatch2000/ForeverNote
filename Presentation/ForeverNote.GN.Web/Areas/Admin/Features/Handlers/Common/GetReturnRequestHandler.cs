using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Areas.Admin.Features.Model.Common;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Features.Handlers.Common
{
    public class GetReturnRequestHandler : IRequestHandler<GetReturnRequest, int>
    {
        private readonly IRepository<ReturnRequest> _returnRequestRepository;

        public GetReturnRequestHandler(IRepository<ReturnRequest> returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }

        public async Task<int> Handle(GetReturnRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                _returnRequestRepository.Table.Where(x => x.ReturnRequestStatusId == request.RequestStatusId &&
                (string.IsNullOrEmpty(request.StoreId) || x.StoreId == request.StoreId)).Count());
        }
    }
}

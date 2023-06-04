using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetReturnRequests : IRequest<CustomerReturnRequestsModel>
    { 
    }
}

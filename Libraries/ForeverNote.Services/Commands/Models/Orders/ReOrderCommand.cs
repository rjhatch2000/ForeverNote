using ForeverNote.Core.Domain.Orders;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class ReOrderCommand : IRequest<IList<string>>
    {
        public Order Order { get; set; }
    }
}

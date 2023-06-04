using ForeverNote.Core.Domain.Orders;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class CancelRecurringPaymentCommand : IRequest<IList<string>>
    {
        public RecurringPayment RecurringPayment { get; set; }
    }
}

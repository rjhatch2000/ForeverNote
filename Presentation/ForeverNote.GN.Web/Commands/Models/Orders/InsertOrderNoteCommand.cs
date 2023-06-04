using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Orders
{
    public class InsertOrderNoteCommand : IRequest<OrderNote>
    {
        public Order Order { get; set; } 
        public Language Language { get; set; }
        public AddOrderNoteModel OrderNote { get; set; }
    }
}

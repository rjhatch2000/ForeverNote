using System;
using System.Collections.Generic;
using System.Text;
using ForeverNote.Core.Domain.Orders;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class CheckOrderStatusCommand : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}

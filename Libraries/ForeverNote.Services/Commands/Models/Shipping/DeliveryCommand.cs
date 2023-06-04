﻿using ForeverNote.Core.Domain.Shipping;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Shipping
{
    public class DeliveryCommand : IRequest<bool>
    {
        public Shipment Shipment { get; set; }
        public bool NotifyCustomer { get; set; }
    }
}
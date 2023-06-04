﻿using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Shipping;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Commands.Models.Orders;
using ForeverNote.Services.Commands.Models.Shipping;
using ForeverNote.Services.Messages;
using ForeverNote.Services.Orders;
using ForeverNote.Services.Shipping;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Shipping
{
    public class DeliveryCommandHandler : IRequestHandler<DeliveryCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IOrderService _orderService;
        private readonly IShipmentService _shipmentService;
        private readonly IProductService _productService;
        private readonly IWorkflowMessageService _workflowMessageService;

        public DeliveryCommandHandler(
            IMediator mediator,
            IOrderService orderService,
            IShipmentService shipmentService,
            IProductService productService,
            IWorkflowMessageService workflowMessageService)
        {
            _mediator = mediator;
            _orderService = orderService;
            _shipmentService = shipmentService;
            _productService = productService;
            _workflowMessageService = workflowMessageService;
        }

        public async Task<bool> Handle(DeliveryCommand request, CancellationToken cancellationToken)
        {
            if (request.Shipment == null)
                throw new ArgumentNullException("shipment");

            var order = await _orderService.GetOrderById(request.Shipment.OrderId);
            if (order == null)
                throw new Exception("Order cannot be loaded");

            if (!request.Shipment.ShippedDateUtc.HasValue)
                throw new Exception("This shipment is not shipped yet");

            if (request.Shipment.DeliveryDateUtc.HasValue)
                throw new Exception("This shipment is already delivered");

            request.Shipment.DeliveryDateUtc = DateTime.UtcNow;
            await _shipmentService.UpdateShipment(request.Shipment);

            if (!await order.HasItemsToAddToShipment(_orderService, _shipmentService, _productService)
                && !await order.HasItemsToShip(_orderService, _shipmentService, _productService)
                && !await order.HasItemsToDeliver(_shipmentService, _productService))
                order.ShippingStatusId = (int)ShippingStatus.Delivered;

            await _orderService.UpdateOrder(order);

            //add a note
            await _orderService.InsertOrderNote(new OrderNote {
                Note = $"Shipment #{request.Shipment.ShipmentNumber} has been delivered",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow,
                OrderId = order.Id,
            });
            if (request.NotifyCustomer)
            {
                //send email notification
                int queuedEmailId = await _workflowMessageService.SendShipmentDeliveredCustomerNotification(request.Shipment, order);
                if (queuedEmailId > 0)
                {
                    await _orderService.InsertOrderNote(new OrderNote {
                        Note = "\"Delivered\" email (to customer) has been queued.",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow,
                        OrderId = order.Id,
                    });
                }
            }
            //event
            await _mediator.PublishShipmentDelivered(request.Shipment);

            //check order status
            await _mediator.Send(new CheckOrderStatusCommand() { Order = order });

            return true;
        }
    }
}

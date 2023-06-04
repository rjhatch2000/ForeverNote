#! "netcoreapp3.1"
#r "ForeverNote.Core"
#r "ForeverNote.Services"

using System;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Services.Events;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

/* Sample code to add new token message (message email) to the order */

public class OrderTokenTest : INotificationHandler<EntityTokensAddedEvent<Order>>
{
    public Task Handle(EntityTokensAddedEvent<Order> eventMessage, CancellationToken cancellationToken)
    {
        //in message templates you can put new token {{AdditionalTokens["NewOrderNumber"]}}
        eventMessage.LiquidObject.AdditionalTokens.Add("NewOrderNumber", $"{eventMessage.Entity.CreatedOnUtc.Year}/{eventMessage.Entity.OrderNumber}");
        return Task.CompletedTask;
    }

}




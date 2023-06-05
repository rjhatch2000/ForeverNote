﻿using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Commands.Models.Customers;
using MediatR;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Customers
{
    /// <summary>
    /// Update Customer Reminder History
    /// </summary>
    public class UpdateCustomerReminderHistoryCommandHandler : IRequestHandler<UpdateCustomerReminderHistoryCommand, bool>
    {
        private readonly IRepository<CustomerReminderHistory> _customerReminderHistory;

        public UpdateCustomerReminderHistoryCommandHandler(IRepository<CustomerReminderHistory> customerReminderHistory)
        {
            _customerReminderHistory = customerReminderHistory;
        }

        public async Task<bool> Handle(UpdateCustomerReminderHistoryCommand request, CancellationToken cancellationToken)
        {
            var builder = Builders<CustomerReminderHistory>.Filter;
            var filter = builder.Eq(x => x.CustomerId, request.CustomerId);

            //update started reminders
            filter &= builder.Eq(x => x.Status, (int)CustomerReminderHistoryStatusEnum.Started);
            var update = Builders<CustomerReminderHistory>.Update
                .Set(x => x.EndDate, DateTime.UtcNow)
            ;
            await _customerReminderHistory.Collection.UpdateManyAsync(filter, update);

            //update Ended reminders
            filter = builder.Eq(x => x.CustomerId, request.CustomerId);
            filter &= builder.Eq(x => x.Status, (int)CustomerReminderHistoryStatusEnum.CompletedReminder);
            filter &= builder.Gt(x => x.EndDate, DateTime.UtcNow.AddHours(-36));

            await _customerReminderHistory.Collection.UpdateManyAsync(filter, update);

            return true;
        }
    }
}

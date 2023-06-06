using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Commands.Models.Users;
using MediatR;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Users
{
    /// <summary>
    /// Update User Reminder History
    /// </summary>
    public class UpdateUserReminderHistoryCommandHandler : IRequestHandler<UpdateUserReminderHistoryCommand, bool>
    {
        private readonly IRepository<UserReminderHistory> _userReminderHistory;

        public UpdateUserReminderHistoryCommandHandler(IRepository<UserReminderHistory> userReminderHistory)
        {
            _userReminderHistory = userReminderHistory;
        }

        public async Task<bool> Handle(UpdateUserReminderHistoryCommand request, CancellationToken cancellationToken)
        {
            var builder = Builders<UserReminderHistory>.Filter;
            var filter = builder.Eq(x => x.UserId, request.UserId);

            //update started reminders
            filter &= builder.Eq(x => x.Status, (int)UserReminderHistoryStatusEnum.Started);
            var update = Builders<UserReminderHistory>.Update
                .Set(x => x.EndDate, DateTime.UtcNow)
            ;
            await _userReminderHistory.Collection.UpdateManyAsync(filter, update);

            //update Ended reminders
            filter = builder.Eq(x => x.UserId, request.UserId);
            filter &= builder.Eq(x => x.Status, (int)UserReminderHistoryStatusEnum.CompletedReminder);
            filter &= builder.Gt(x => x.EndDate, DateTime.UtcNow.AddHours(-36));

            await _userReminderHistory.Collection.UpdateManyAsync(filter, update);

            return true;
        }
    }
}

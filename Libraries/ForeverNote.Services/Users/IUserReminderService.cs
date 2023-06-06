using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public partial interface IUserReminderService
    {


        /// <summary>
        /// Gets user reminder
        /// </summary>
        /// <param name="id">User reminder identifier</param>
        /// <returns>User reminder</returns>
        Task<UserReminder> GetUserReminderById(string id);


        /// <summary>
        /// Gets all user reminders
        /// </summary>
        /// <returns>User reminders</returns>
        Task<IList<UserReminder>> GetUserReminders();

        /// <summary>
        /// Inserts a user reminder
        /// </summary>
        /// <param name="UserReminder">User reminder</param>
        Task InsertUserReminder(UserReminder userReminder);

        /// <summary>
        /// Delete a user reminder
        /// </summary>
        /// <param name="userReminder">User reminder</param>
        Task DeleteUserReminder(UserReminder userReminder);

        /// <summary>
        /// Updates the user reminder
        /// </summary>
        /// <param name="UserReminder">User reminder</param>
        Task UpdateUserReminder(UserReminder userReminder);

        /// <summary>
        /// Gets user reminders history for reminder
        /// </summary>
        /// <returns>SerializeUserReminderHistory</returns>
        Task<IPagedList<SerializeUserReminderHistory>> GetAllUserReminderHistory(string userReminderId, int pageIndex = 0, int pageSize = 2147483647);

        Task Task_RegisteredUser(string id = "");
        Task Task_LastActivity(string id = "");
    }
}

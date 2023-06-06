using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public partial interface IUserActionService
    {

        /// <summary>
        /// Gets user action
        /// </summary>
        /// <param name="id">User action identifier</param>
        /// <returns>User Action</returns>
        Task<UserAction> GetUserActionById(string id);

        /// <summary>
        /// Gets all user actions
        /// </summary>
        /// <returns>User actions</returns>
        Task<IList<UserAction>> GetUserActions();

        /// <summary>
        /// Inserts a user action
        /// </summary>
        /// <param name="UserAction">User action</param>
        Task InsertUserAction(UserAction userAction);

        /// <summary>
        /// Delete a user action
        /// </summary>
        /// <param name="userAction">User action</param>
        Task DeleteUserAction(UserAction userAction);

        /// <summary>
        /// Updates the user action
        /// </summary>
        /// <param name="userTag">User tag</param>
        Task UpdateUserAction(UserAction userAction);

        Task<IList<UserActionType>> GetUserActionType();
        Task<UserActionType> GetUserActionTypeById(string id);
        Task<IPagedList<UserActionHistory>> GetAllUserActionHistory(string userActionId, int pageIndex = 0, int pageSize = 2147483647);
        Task UpdateUserActionType(UserActionType userActionType);
    }
}

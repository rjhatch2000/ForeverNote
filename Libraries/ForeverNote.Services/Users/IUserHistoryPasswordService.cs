using ForeverNote.Core.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public interface IUserHistoryPasswordService
    {
        #region Password history

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        Task<IList<UserHistoryPassword>> GetPasswords(string userId, int passwordsToReturn);

        /// <summary>
        /// Insert a user history password
        /// </summary>
        /// <param name="user">User</param>
        Task InsertUserPassword(User user);


        #endregion
    }
}

using System;

namespace ForeverNote.Core.Domain.Users
{
    public static class UserExtensions
    {
        #region User role

        /// <summary>
        /// Gets a value indicating whether the user is a built-in record for background tasks
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.IsSystemAccount || String.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(SystemUserNames.BackgroundTask, StringComparison.OrdinalIgnoreCase);
            return result;
        }

        #endregion
    }
}

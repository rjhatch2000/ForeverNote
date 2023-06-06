using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Logging
{
    /// <summary>
    /// User activity service interface
    /// </summary>
    public partial interface IUserActivityService
    {
        /// <summary>
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        Task InsertActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        Task UpdateActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="activityLogType">Activity log type</param>
        Task DeleteActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type items</returns>
        Task<IList<ActivityLogType>> GetAllActivityTypes();

        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        Task<ActivityLogType> GetActivityTypeById(string activityLogTypeId);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        Task InsertActivity(string systemKeyword, string entityKeyId, string comment, params object[] commentParams);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="user">The user</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        Task<ActivityLog> InsertActivity(string systemKeyword, string entityKeyId,
            string comment, User user, params object[] commentParams);


        /// <summary>
        /// Inserts an activity log item async
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="entityKeyId">Entity Key</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="userId">The user</param>
        /// <param name="addressIp">IP Address</param>
        /// <returns>Activity log item</returns>
        Task InsertActivityAsync(string systemKeyword, string entityKeyId,
            string comment, string userId, string addressIp);

        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="activityLog">Activity log</param>
        Task DeleteActivity(ActivityLog activityLog);

        /// <summary>
        /// Gets all activity log items
        /// </summary>
        /// <param name="comment">Log item message text or text part; null or empty string to load all</param>
        /// <param name="createdOnFrom">Log item creation from; null to load all users</param>
        /// <param name="createdOnTo">Log item creation to; null to load all users</param>
        /// <param name="userId">User identifier; null to load all users</param>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetAllActivities(string comment = "",
            DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string userId = "", string activityLogTypeId = "",
            string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets stats activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Stats Activity log items</returns>
        Task<IPagedList<ActivityStats>> GetStatsActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string activityLogTypeId = "",
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets notebook activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="notebookId">Notebook identifier</param>        
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetNotebookActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string notebookId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets knowledgebase notebook activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="notebookId">Notebook identifier</param>        
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetKnowledgebaseNotebookActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string notebookId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets knowledgebase article activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="notebookId">Notebook identifier</param>        
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetKnowledgebaseArticleActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string notebookId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets manufacturer activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>        
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetManufacturerActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string manufacturerId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets note activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all records</param>
        /// <param name="createdOnTo">Log item creation to; null to load all records</param>
        /// <param name="noteId">Manufacturer identifier</param>        
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Activity log items</returns>
        Task<IPagedList<ActivityLog>> GetNoteActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, string noteId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        Task<ActivityLog> GetActivityById(string activityLogId);

        /// <summary>
        /// Clears activity log
        /// </summary>
        Task ClearAllActivities();
    }
}

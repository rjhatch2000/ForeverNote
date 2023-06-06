using ForeverNote.Core;
using ForeverNote.Core.Domain.PushNotifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.PushNotifications
{
    public interface IPushNotificationsService
    {
        /// <summary>
        /// Inserts push receiver
        /// </summary>
        /// <param name="registration"></param>
        Task InsertPushReceiver(PushRegistration registration);

        /// <summary>
        /// Deletes push receiver
        /// </summary>
        /// <param name="registration"></param>
        Task DeletePushReceiver(PushRegistration registration);

        /// <summary>
        /// Gets push receiver
        /// </summary>
        /// <param name="UserId"></param>
        Task<PushRegistration> GetPushReceiverByUserId(string UserId);

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        Task<List<PushRegistration>> GetPushReceivers();

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        /// <param name="Id"></param>
        Task<PushRegistration> GetPushReceiver(string Id);

        /// <summary>
        /// Gets number of users that accepted push notifications permission popup
        /// </summary>
        Task<int> GetAllowedReceivers();

        /// <summary>
        /// Gets number of users that denied push notifications permission popup
        /// </summary>
        Task<int> GetDeniedReceivers();

        /// <summary>
        /// Updates push receiver
        /// </summary>
        /// <param name="registration"></param>
        Task UpdatePushReceiver(PushRegistration registration);

        /// <summary>
        /// Inserts push message
        /// </summary>
        /// <param name="message"></param>
        Task InsertPushMessage(PushMessage message);

        /// <summary>
        /// Gets all push messages
        /// </summary>
        Task<IPagedList<PushMessage>> GetPushMessages(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        Task<IPagedList<PushRegistration>> GetPushReceivers(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Sends push notification to all receivers
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="registrationIds"></param>
        /// <param name="clickUrl"></param>
        /// <returns>Bool indicating whether message was sent successfully and string result to display</returns>
        Task<(bool, string)> SendPushNotification(string title, string text, string pictureUrl, string clickUrl, List<string> registrationIds = null);

        /// <summary>
        /// Sends push notification to all receivers
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="userId"></param>
        /// <param name="clickUrl"></param>
        /// <returns>Bool indicating whether message was sent successfully and string result to display</returns>
        Task<(bool, string)> SendPushNotification(string title, string text, string pictureUrl, string userId, string clickUrl);
    }
}

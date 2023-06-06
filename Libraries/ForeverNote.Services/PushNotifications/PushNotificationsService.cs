using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.PushNotifications;
using ForeverNote.Services.Events;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MediatR;
using System.Net.Http;

namespace ForeverNote.Services.PushNotifications
{
    public class PushNotificationsService : IPushNotificationsService
    {
        private readonly IRepository<PushRegistration> _pushRegistratiosnRepository;
        private readonly IRepository<PushMessage> _pushMessagesRepository;
        private readonly IMediator _mediator;
        private readonly PushNotificationsSettings _pushNotificationsSettings;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        public PushNotificationsService(IRepository<PushRegistration> pushRegistratiosnRepository, IRepository<PushMessage> pushMessagesRepository,
            IMediator mediator, PushNotificationsSettings pushNotificationsSettings, ILocalizationService localizationService, ILogger logger)
        {
            _pushRegistratiosnRepository = pushRegistratiosnRepository;
            _pushMessagesRepository = pushMessagesRepository;
            _mediator = mediator;
            _pushNotificationsSettings = pushNotificationsSettings;
            _localizationService = localizationService;
            _logger = logger;
        }

        /// <summary>
        /// Inserts push receiver
        /// </summary>
        /// <param name="model"></param>
        public virtual async Task InsertPushReceiver(PushRegistration registration)
        {
            await _pushRegistratiosnRepository.InsertAsync(registration);
            await _mediator.EntityInserted(registration);
        }

        /// <summary>
        /// Deletes push receiver
        /// </summary>
        /// <param name="model"></param>
        public virtual async Task DeletePushReceiver(PushRegistration registration)
        {
            await _pushRegistratiosnRepository.DeleteAsync(registration);
            await _mediator.EntityDeleted(registration);
        }

        /// <summary>
        /// Gets push receiver
        /// </summary>
        /// <param name="UserId"></param>
        public virtual async Task<PushRegistration> GetPushReceiverByUserId(string UserId)
        {
            return await _pushRegistratiosnRepository.Table.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates push receiver
        /// </summary>
        /// <param name="registration"></param>
        public virtual async Task UpdatePushReceiver(PushRegistration registration)
        {
            await _pushRegistratiosnRepository.UpdateAsync(registration);
            await _mediator.EntityUpdated(registration);
        }

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        public virtual async Task<List<PushRegistration>> GetPushReceivers()
        {
            return await _pushRegistratiosnRepository.Table.Where(x => x.Allowed).ToListAsync();
        }

        /// <summary>
        /// Gets number of users that accepted push notifications permission popup
        /// </summary>
        public virtual Task<int> GetAllowedReceivers()
        {
            return _pushRegistratiosnRepository.Table.Where(x => x.Allowed).CountAsync();
        }

        /// <summary>
        /// Gets number of users that denied push notifications permission popup
        /// </summary>
        public virtual Task<int> GetDeniedReceivers()
        {
            return _pushRegistratiosnRepository.Table.Where(x => !x.Allowed).CountAsync();
        }

        /// <summary>
        /// Inserts push message
        /// </summary>
        /// <param name="registration"></param>
        public virtual async Task InsertPushMessage(PushMessage message)
        {
            await _pushMessagesRepository.InsertAsync(message);
            await _mediator.EntityInserted(message);
        }

        /// <summary>
        /// Gets all push messages
        /// </summary>
        public virtual async Task<IPagedList<PushMessage>> GetPushMessages(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var allMessages = await _pushMessagesRepository.Table.OrderByDescending(x => x.SentOn).ToListAsync();
            return new PagedList<PushMessage>(allMessages.Skip(pageIndex * pageSize).Take(pageSize).ToList(), pageIndex, pageSize, allMessages.Count);
        }

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        public virtual async Task<IPagedList<PushRegistration>> GetPushReceivers(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var allReceivers = await _pushRegistratiosnRepository.Table.OrderByDescending(x => x.RegisteredOn).ToListAsync();
            return new PagedList<PushRegistration>(allReceivers.Skip(pageIndex * pageSize).Take(pageSize).ToList(), pageIndex, pageSize, allReceivers.Count);
        }

        /// <summary>
        /// Sends push notification to all receivers
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="registrationIds"></param>
        /// <param name="clickUrl"></param>
        /// <returns>Bool indicating whether message was sent successfully and string result to display</returns>
        public virtual async Task<(bool, string)> SendPushNotification(string title, string text, string pictureUrl, string clickUrl, List<string> registrationIds = null)
        {
            ////WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            using (HttpClient client = new HttpClient())
            {
                ////tRequest.Method = "post";
                ////tRequest.ContentType = "application/json";

                var ids = new List<string>();

                if (registrationIds != null && registrationIds.Any())
                {
                    ids = registrationIds;
                }
                else
                {
                    var receivers = await GetPushReceivers();
                    if (!receivers.Any())
                    {
                        return (false, _localizationService.GetResource("Admin.PushNotifications.Error.NoReceivers"));
                    }

                    int batchsize = 1000;
                    for (int batch = 0; batch <= Math.Round((decimal)(receivers.Count / batchsize), 0, MidpointRounding.ToEven); batch++)
                    {
                        var t = receivers.Skip(batch * batchsize).Take(batchsize);
                        foreach (var receiver in receivers)
                        {
                            if (!ids.Contains(receiver.Token))
                                ids.Add(receiver.Token);
                        }
                    }
                }

                var data = new
                {
                    registration_ids = ids,
                    notification = new
                    {
                        body = text,
                        title = title,
                        icon = pictureUrl,
                        click_action = clickUrl
                    }
                };

                var json = JsonConvert.SerializeObject(data);
                ////Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                //TODO: Is this how the new headers need to be formatted?
                ////tRequest.Headers.Add(string.Format("Authorization: key={0}", _pushNotificationsSettings.PrivateApiKey));
                client.DefaultRequestHeaders.Add("Authorization", string.Format("key={0}", _pushNotificationsSettings.PrivateApiKey));
                ////tRequest.Headers.Add(string.Format("Sender: id={0}", _pushNotificationsSettings.SenderId));
                client.DefaultRequestHeaders.Add("Sender", string.Format("id={0}", _pushNotificationsSettings.SenderId));
                ////tRequest.ContentLength = byteArray.Length; //TODO: Needed? There's no property for this
                try
                {
                    //client.PostAsync("https://fcm.googleapis.com/fcm/send",)
                    ////using (Stream dataStream = tRequest.GetRequestStream())
                    using (var response = 
                        await client.PostAsync("https://fcm.googleapis.com/fcm/send", content))
                    {
                        ////dataStream.Write(byteArray, 0, byteArray.Length);
                        ////using (WebResponse tResponse = tRequest.GetResponse())
                        ////{
                        response.EnsureSuccessStatusCode();

                            ////using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            ////{
                            ////    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            ////    {
                                    ////String sResponseFromServer = tReader.ReadToEnd();
                                    string sResponseFromServer = await response.Content.ReadAsStringAsync();
                                    
                                    var jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(sResponseFromServer);

                                    if (jsonResponse.failure > 0)
                                    {
                                        await _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Error occured while sending push notification.", sResponseFromServer);
                                    }

                                    await InsertPushMessage(new PushMessage
                                    {
                                        NumberOfReceivers = jsonResponse.success,
                                        SentOn = DateTime.UtcNow,
                                        Text = text,
                                        Title = title
                                    });

                                    return (true, string.Format(_localizationService.GetResource("Admin.PushNotifications.MessageSent"), jsonResponse.success, jsonResponse.failure));
                            ////    }
                            ////}
                        ////}
                    }
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }

        }

        /// <summary>
        /// Sends push notification to specified user
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="userId"></param>
        /// <param name="clickUrl"></param>
        /// <returns>Bool indicating whether message was sent successfully and string result to display</returns>
        public virtual async Task<(bool, string)> SendPushNotification(string title, string text, string pictureUrl, string userId, string clickUrl)
        {
            return await SendPushNotification(title, text, pictureUrl, clickUrl, new List<string> { GetPushReceiverByUserId(userId).Id.ToString() });
        }

        /// <summary>
        /// Gets all push receivers
        /// </summary>
        /// <param name="Id"></param>
        public virtual Task<PushRegistration> GetPushReceiver(string Id)
        {
            return _pushRegistratiosnRepository.Table.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}

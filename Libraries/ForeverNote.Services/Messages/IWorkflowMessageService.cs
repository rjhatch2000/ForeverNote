﻿using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial interface IWorkflowMessageService
    {
        #region User workflow

        /// <summary>
        /// Sends 'New user' notification message to a store owner
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserRegisteredNotificationMessage(User user, string languageId);

        /// <summary>
        /// Sends a welcome message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserWelcomeMessage(User user, string languageId);

        /// <summary>
        /// Sends an email validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserEmailValidationMessage(User user, string languageId);

        /// <summary>
        /// Sends password recovery message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserPasswordRecoveryMessage(User user, string languageId);

        /// <summary>
        /// Send an email token validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="token">Token</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserEmailTokenValidationMessage(User user, string token, string languageId);

        #endregion

        #region Send a message to a friend, ask question

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="note">Note instance</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNoteEmailAFriendMessage(User user, string languageId,
            Note note, string userEmail, string friendsEmail, string personalMessage);

        /// <summary>
        /// Sends wishlist "email a friend" message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendWishlistEmailAFriendMessage(User user, string languageId,
             string userEmail, string friendsEmail, string personalMessage);


        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="note">Note instance</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNoteQuestionMessage(User user, string languageId,
            Note note, string userEmail, string fullName, string phone, string message);

        #endregion

        #region Misc

        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <param name="attrInfo">Attr info</param>
        /// <param name="attrXml">Attr xml</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendContactUsMessage(User user, string languageId, string senderEmail, string senderName, string subject, string body, string attrInfo, string attrXml);

        /// <summary>
        /// Sends a user action event 
        /// </summary>
        /// <param name="UserAction">User action</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendUserActionEvent_Notification(UserAction action, string languageId, User user);

        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="liquidObject">LiquidObject</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="attachedDownloads">Attached downloads ident</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, string languageId, LiquidObject liquidObject,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            IEnumerable<string> attachedDownloads = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null);

        /// <summary>
        /// Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<int> SendTestEmail(string messageTemplateId, string sendToEmail, LiquidObject liquidObject, string languageId);

        #endregion
    }
}
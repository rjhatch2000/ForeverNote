using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        private readonly CommonSettings _commonSettings;
        #region Fields

        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public MessageTokenProvider(
            CommonSettings commonSettings,
            IMediator mediator
        )
        {
            _commonSettings = commonSettings;
            _mediator = mediator;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>
        public virtual string[] GetListOfCampaignAllowedTokens()
        {
            var allowedTokens = LiquidExtensions.GetTokens(
                typeof(LiquidUser)
            );

            return allowedTokens.ToArray();
        }

        public virtual string[] GetListOfAllowedTokens()
        {
            var allowedTokens = LiquidExtensions.GetTokens(
                typeof(LiquidAskQuestion),
                typeof(LiquidBackInStockSubscription),
                typeof(LiquidContactUs),
                typeof(LiquidUser),
                typeof(LiquidEmailAFriend),
                typeof(LiquidNote)
            );

            return allowedTokens.ToArray();
        }

        public virtual string[] GetListOfUserReminderAllowedTokens(UserReminderRuleEnum rule)
        {
            var allowedTokens = new List<string>();

            allowedTokens.AddRange(LiquidExtensions.GetTokens(typeof(LiquidUser)));

            return allowedTokens.ToArray();
        }

        public async Task AddUserTokens(LiquidObject liquidObject, User user, Language language)
        {
            var liquidUser = new LiquidUser(_commonSettings, user, language);
            liquidObject.User = liquidUser;

            await _mediator.EntityTokensAdded(user, liquidUser, liquidObject);
        }

        public async Task AddNoteTokens(LiquidObject liquidObject, Note note, Language language)
        {
            var liquidNote = new LiquidNote(_commonSettings, note, language);
            liquidObject.Note = liquidNote;
            await _mediator.EntityTokensAdded(note, liquidNote, liquidObject);
        }

        #endregion
    }
}
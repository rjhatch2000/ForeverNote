using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
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
                typeof(LiquidNewsLetterSubscription),
                typeof(LiquidCustomer)
            );

            return allowedTokens.ToArray();
        }

        public virtual string[] GetListOfAllowedTokens()
        {
            var allowedTokens = LiquidExtensions.GetTokens(
                typeof(LiquidAskQuestion),
                typeof(LiquidBackInStockSubscription),
                typeof(LiquidContactUs),
                typeof(LiquidCustomer),
                typeof(LiquidEmailAFriend),
                typeof(LiquidNewsLetterSubscription),
                typeof(LiquidProduct)
            );

            return allowedTokens.ToArray();
        }

        public virtual string[] GetListOfCustomerReminderAllowedTokens(CustomerReminderRuleEnum rule)
        {
            var allowedTokens = new List<string>();

            allowedTokens.AddRange(LiquidExtensions.GetTokens(typeof(LiquidCustomer)));

            return allowedTokens.ToArray();
        }

        public async Task AddCustomerTokens(LiquidObject liquidObject, Customer customer, Language language, CustomerNote customerNote = null)
        {
            var liquidCustomer = new LiquidCustomer(_commonSettings, customer, language, customerNote);
            liquidObject.Customer = liquidCustomer;

            await _mediator.EntityTokensAdded(customer, liquidCustomer, liquidObject);
            await _mediator.EntityTokensAdded(customerNote, liquidCustomer, liquidObject);
        }

        public async Task AddProductTokens(LiquidObject liquidObject, Product product, Language language)
        {
            var liquidProduct = new LiquidProduct(_commonSettings, product, language);
            liquidObject.Product = liquidProduct;
            await _mediator.EntityTokensAdded(product, liquidProduct, liquidObject);
        }

        #endregion
    }
}
using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Commands.Models.Catalog;
using ForeverNote.Services.Common;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Messages;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Catalog
{
    public class SendNotificationsToSubscribersCommandHandler : IRequestHandler<SendNotificationsToSubscribersCommand, IList<BackInStockSubscription>>
    {
        private readonly ICustomerService _customerService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IRepository<BackInStockSubscription> _backInStockSubscriptionRepository;

        public SendNotificationsToSubscribersCommandHandler(
            ICustomerService customerService,
            IWorkflowMessageService workflowMessageService,
            IRepository<BackInStockSubscription> backInStockSubscriptionRepository)
        {
            _customerService = customerService;
            _workflowMessageService = workflowMessageService;
            _backInStockSubscriptionRepository = backInStockSubscriptionRepository;
        }

        public async Task<IList<BackInStockSubscription>> Handle(SendNotificationsToSubscribersCommand request, CancellationToken cancellationToken)
        {
            if (request.Product == null)
                throw new ArgumentNullException("product");

            int result = 0;
            var subscriptions = await GetAllSubscriptionsByProductId(request.Product.Id, request.AttributeXml);
            foreach (var subscription in subscriptions)
            {
                var customer = await _customerService.GetCustomerById(subscription.CustomerId);
                //ensure that customer is registered (simple and fast way)
                if (customer != null && CommonHelper.IsValidEmail(customer.Email))
                {
                    var customerLanguageId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LanguageId);
                    await _workflowMessageService.SendBackInStockNotification(customer, request.Product, subscription, customerLanguageId);
                    result++;
                }
            }

            return subscriptions;

        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Subscriptions</returns>
        private async Task<IList<BackInStockSubscription>> GetAllSubscriptionsByProductId(string productId, string attributeXml)
        {
            var query = _backInStockSubscriptionRepository.Table;
            //product
            query = query.Where(biss => biss.ProductId == productId);

            //attributes
            if (!string.IsNullOrEmpty(attributeXml))
                query = query.Where(biss => biss.AttributeXml == attributeXml);

            query = query.OrderByDescending(biss => biss.CreatedOnUtc);
            return await query.ToListAsync();
        }
    }
}

using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Services.Commands.Models.Catalog;
using ForeverNote.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Back in stock subscription service
    /// </summary>
    public partial class BackInStockSubscriptionService : IBackInStockSubscriptionService
    {
        #region Fields

        private readonly IRepository<BackInStockSubscription> _backInStockSubscriptionRepository;
        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="backInStockSubscriptionRepository">Back in stock subscription repository</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="eventPublisher">Event publisher</param>
        public BackInStockSubscriptionService(IRepository<BackInStockSubscription> backInStockSubscriptionRepository,
            IMediator mediator)
        {
            _backInStockSubscriptionRepository = backInStockSubscriptionRepository;
            _mediator = mediator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a back in stock subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual async Task DeleteSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            await _backInStockSubscriptionRepository.DeleteAsync(subscription);

            //event notification
            await _mediator.EntityDeleted(subscription);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Subscriptions</returns>
        public virtual async Task<IPagedList<BackInStockSubscription>> GetAllSubscriptionsByCustomerId(
            string customerId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _backInStockSubscriptionRepository.Table;

            //customer
            query = query.Where(biss => biss.CustomerId == customerId);

            query = query.OrderByDescending(biss => biss.CreatedOnUtc);

            return await PagedList<BackInStockSubscription>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="customerId">Customer id</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="attributeXml">Attribute xml</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <returns>Subscriptions</returns>
        public virtual async Task<BackInStockSubscription> FindSubscription(string customerId, string productId, string attributeXml)
        {
            var query = from biss in _backInStockSubscriptionRepository.Table
                        orderby biss.CreatedOnUtc descending
                        where biss.CustomerId == customerId &&
                              biss.ProductId == productId
                        select biss;

            if (!string.IsNullOrEmpty(attributeXml))
            {
                query = query.Where(x => x.AttributeXml == attributeXml);
            }
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a subscription
        /// </summary>
        /// <param name="subscriptionId">Subscription identifier</param>
        /// <returns>Subscription</returns>
        public virtual async Task<BackInStockSubscription> GetSubscriptionById(string subscriptionId)
        {
            var subscription = await _backInStockSubscriptionRepository.GetByIdAsync(subscriptionId);
            return subscription;
        }

        /// <summary>
        /// Inserts subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual async Task InsertSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            await _backInStockSubscriptionRepository.InsertAsync(subscription);

            //event notification
            await _mediator.EntityInserted(subscription);
        }

        /// <summary>
        /// Updates subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual async Task UpdateSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            await _backInStockSubscriptionRepository.UpdateAsync(subscription);

            //event notification
            await _mediator.EntityUpdated(subscription);
        }

        /// <summary>
        /// Send notification to subscribers
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Number of sent email</returns>
        public virtual async Task SendNotificationsToSubscribers(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var subscriptions = await _mediator.Send(new SendNotificationsToSubscribersCommand() {
                Product = product,
            });

            for (var i = 0; i <= subscriptions.Count - 1; i++)
                await DeleteSubscription(subscriptions[i]);
        }

        /// <summary>
        /// Send notification to subscribers
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributeXml">Attribute xml</param>
        /// <returns>Number of sent email</returns>
        public virtual async Task SendNotificationsToSubscribers(Product product, string attributeXml)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var subscriptions = await _mediator.Send(new SendNotificationsToSubscribersCommand() {
                Product = product,
                AttributeXml = attributeXml
            });

            for (var i = 0; i <= subscriptions.Count - 1; i++)
                await DeleteSubscription(subscriptions[i]);

        }

        #endregion
    }
}

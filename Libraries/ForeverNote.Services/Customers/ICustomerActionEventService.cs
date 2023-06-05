using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Orders;
using System.Threading.Tasks;

namespace ForeverNote.Services.Customers
{
    public partial interface ICustomerActionEventService
    {
        /// <summary>
        /// Viewed
        /// </summary>
        Task Viewed(Customer customer, string currentUrl, string previousUrl);

        /// <summary>
        /// Run action url
        /// </summary>
        Task Url(Customer customer, string currentUrl, string previousUrl);

        /// <summary>
        /// Run action url
        /// </summary>
        Task Registration(Customer customer);
    }
}

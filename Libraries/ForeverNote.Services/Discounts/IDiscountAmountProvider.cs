using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Discounts;
using ForeverNote.Core.Plugins;
using System.Threading.Tasks;

namespace ForeverNote.Services.Discounts
{
    public partial interface IDiscountAmountProvider : IPlugin
    {
        Task<decimal> DiscountAmount(Discount discount, Customer customer, Product product, decimal amount);
    }
}

using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Models.ShoppingCart;
using MediatR;

namespace ForeverNote.Web.Features.Models.ShoppingCart
{
    public class GetMiniShoppingCart : IRequest<MiniShoppingCartModel>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Currency Currency { get; set; }
        public Store Store { get; set; }
        public TaxDisplayType TaxDisplayType { get; set; }
    }
}

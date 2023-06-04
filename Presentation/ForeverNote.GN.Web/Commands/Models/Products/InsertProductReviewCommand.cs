using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Products
{
    public class InsertProductReviewCommand : IRequest<ProductReview>
    {
        public Store Store { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public ProductReviewsModel Model { get; set; }
    }
}

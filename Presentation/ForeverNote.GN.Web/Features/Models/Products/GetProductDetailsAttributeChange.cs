using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductDetailsAttributeChange : IRequest<ProductDetailsAttributeChangeModel>
    {
        public Customer Customer { get; set; }
        public Currency Currency { get; set; }
        public Store Store { get; set; }
        public Product Product { get; set; }
        public bool ValidateAttributeConditions { get; set; }
        public bool LoadPicture { get; set; }
        public IFormCollection Form { get; set; }
    }
}

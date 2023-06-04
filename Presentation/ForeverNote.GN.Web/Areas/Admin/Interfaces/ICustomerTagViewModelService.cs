using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Interfaces
{
    public interface ICustomerTagViewModelService
    {
        CustomerModel PrepareCustomerModelForList(Customer customer);
        CustomerTagModel PrepareCustomerTagModel();
        Task<CustomerTag> InsertCustomerTagModel(CustomerTagModel model);
        Task<CustomerTag> UpdateCustomerTagModel(CustomerTag customerTag, CustomerTagModel model);
        Task DeleteCustomerTag(CustomerTag customerTag);
        Task<CustomerTagProductModel.AddProductModel> PrepareProductModel(string customerTagId);
        Task<(IList<ProductModel> products, int totalCount)> PrepareProductModel(CustomerTagProductModel.AddProductModel model, int pageIndex, int pageSize);
        Task InsertProductModel(CustomerTagProductModel.AddProductModel model);
    }
}

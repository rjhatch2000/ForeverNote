using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Areas.Admin.Models.Customers;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class CustomerActionTypeMappingExtensions
    {
        public static CustomerActionTypeModel ToModel(this CustomerActionType entity)
        {
            return entity.MapTo<CustomerActionType, CustomerActionTypeModel>();
        }
    }
}
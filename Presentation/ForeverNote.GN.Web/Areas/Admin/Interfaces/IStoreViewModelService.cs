using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Areas.Admin.Models.Stores;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Interfaces
{
    public interface IStoreViewModelService
    {
        Task PrepareLanguagesModel(StoreModel model);
        Task PrepareWarehouseModel(StoreModel model);
        Task PrepareCountryModel(StoreModel model);
        StoreModel PrepareStoreModel();
        Task<Store> InsertStoreModel(StoreModel model);
        Task<Store> UpdateStoreModel(Store store, StoreModel model);
        Task DeleteStore(Store store);

    }
}

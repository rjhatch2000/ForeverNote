using ForeverNote.Core.Domain.Directory;
using ForeverNote.Web.Areas.Admin.Models.Directory;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Interfaces
{
    public interface ICurrencyViewModelService
    {
        CurrencyModel PrepareCurrencyModel();
        Task<Currency> InsertCurrencyModel(CurrencyModel model);
        Task<Currency> UpdateCurrencyModel(Currency currency, CurrencyModel model);
    }
}

using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseForeverNoteModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public string CurrentCurrencyId { get; set; }
    }
}
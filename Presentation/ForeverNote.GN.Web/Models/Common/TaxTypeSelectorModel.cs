using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public partial class TaxTypeSelectorModel : BaseForeverNoteModel
    {
        public TaxDisplayType CurrentTaxType { get; set; }
    }
}
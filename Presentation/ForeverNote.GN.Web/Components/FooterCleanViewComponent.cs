using ForeverNote.Core;
using ForeverNote.Core.Domain;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.Components
{
    public class FooterCleanViewComponent : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        public FooterCleanViewComponent(
            IWorkContext workContext,
            IStoreContext storeContext,
            StoreInformationSettings storeInformationSettings)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _storeInformationSettings = storeInformationSettings;
        }

        public IViewComponentResult Invoke()
        {
            var model = PrepareFooter();
            return View(model);
        }

        private FooterCleanModel PrepareFooter()
        {
            var currentstore = _storeContext.CurrentStore;

            var model = new FooterCleanModel {
                StoreName = currentstore.GetLocalized(x => x.Name, _workContext.WorkingLanguage.Id),
                CompanyName = currentstore.CompanyName,
                CompanyEmail = currentstore.CompanyEmail,
                CompanyAddress = currentstore.CompanyAddress,
                CompanyPhone = currentstore.CompanyPhoneNumber,
                CompanyHours = currentstore.CompanyHours,
                HidePoweredByForeverNote = _storeInformationSettings.HidePoweredByForeverNote,
            };

            return model;
        }
    }
}

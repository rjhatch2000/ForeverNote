﻿using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Interfaces
{
    public interface IContactFormViewModelService
    {
        Task<ContactFormListModel> PrepareContactFormListModel();
        Task<ContactFormModel> PrepareContactFormModel(ContactUs contactUs);
        Task<(IEnumerable<ContactFormModel> contactFormModel, int totalCount)> PrepareContactFormListModel(ContactFormListModel model, int pageIndex, int pageSize);
    }
}

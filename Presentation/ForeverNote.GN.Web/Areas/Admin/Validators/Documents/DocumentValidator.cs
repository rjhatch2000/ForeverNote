using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Documents;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Documents
{
    public class DocumentValidator : BaseForeverNoteValidator<DocumentModel>
    {
        public DocumentValidator(
            IEnumerable<IValidatorConsumer<DocumentModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Documents.Document.Fields.Name.Required"));

            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Documents.Document.Fields.Number.Required"));

        }
    }
}

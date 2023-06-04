using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Documents;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Documents
{
    public class DocumentTypeValidator : BaseForeverNoteValidator<DocumentTypeModel>
    {
        public DocumentTypeValidator(
            IEnumerable<IValidatorConsumer<DocumentTypeModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Documents.Type.Fields.Name.Required"));
        }
    }
}

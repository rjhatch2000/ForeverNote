using FluentValidation;
using ForeverNote.Api.DTOs.Common;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Common
{
    public class PictureValidator : BaseForeverNoteValidator<PictureDto>
    {
        public PictureValidator(
            IEnumerable<IValidatorConsumer<PictureDto>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.PictureBinary).NotEmpty().WithMessage(localizationService.GetResource("Api.Common.Picture.Fields.PictureBinary.Required"));
            RuleFor(x => x.MimeType).NotEmpty().WithMessage(localizationService.GetResource("Api.Common.Picture.Fields.MimeType.Required"));
            RuleFor(x => x.Id).Empty().WithMessage(localizationService.GetResource("Api.Common.Picture.Fields.Id.NotRequired"));
        }
    }
}

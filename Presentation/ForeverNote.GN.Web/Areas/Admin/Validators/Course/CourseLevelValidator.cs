using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Courses;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Courses
{
    public class CourseLevelValidator : BaseForeverNoteValidator<CourseLevelModel>
    {
        public CourseLevelValidator(
            IEnumerable<IValidatorConsumer<CourseLevelModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Level.Fields.Name.Required"));
        }
    }
}
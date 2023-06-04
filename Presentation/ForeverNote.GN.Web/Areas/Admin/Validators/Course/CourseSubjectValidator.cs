using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Courses;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Courses
{
    public class CourseSubjectValidator : BaseForeverNoteValidator<CourseSubjectModel>
    {
        public CourseSubjectValidator(
            IEnumerable<IValidatorConsumer<CourseSubjectModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Subject.Fields.Name.Required"));
            RuleFor(x => x.CourseId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Subject.Fields.CourseId.Required"));
        }
    }
}
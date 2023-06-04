using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Courses;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Courses
{
    public class CourseLessonValidator : BaseForeverNoteValidator<CourseLessonModel>
    {
        public CourseLessonValidator(
            IEnumerable<IValidatorConsumer<CourseLessonModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.Name.Required"));
            RuleFor(x => x.CourseId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.CourseId.Required"));
            RuleFor(x => x.SubjectId).NotEmpty().WithMessage(localizationService.GetResource("Admin.Courses.Course.Lesson.Fields.SubjectId.Required"));
        }
    }
}
using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Tasks;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Tasks
{
    public class ScheduleTaskValidator : BaseForeverNoteValidator<ScheduleTaskModel>
    {
        public ScheduleTaskValidator(
            IEnumerable<IValidatorConsumer<ScheduleTaskModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.TimeInterval).GreaterThan(0).WithMessage("Time interval must be greater than zero");
        }
    }
}
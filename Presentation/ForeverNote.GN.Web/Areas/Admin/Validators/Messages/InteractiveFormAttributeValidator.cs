﻿using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Messages
{
    public class InteractiveFormAttributeValidator : BaseForeverNoteValidator<InteractiveFormAttributeModel>
    {
        public InteractiveFormAttributeValidator(
            IEnumerable<IValidatorConsumer<InteractiveFormAttributeModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.InteractiveForms.Attribute.Fields.Name.Required"));
            RuleFor(x => x.SystemName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.InteractiveForms.Attribute.Fields.SystemName.Required"));
        }
    }
}
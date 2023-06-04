﻿using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Common
{
    public class GenericAttributeValidator : BaseForeverNoteValidator<GenericAttributeModel>
    {
        public GenericAttributeValidator(
            IEnumerable<IValidatorConsumer<GenericAttributeModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Common.GenericAttributes.Fields.Id.Required"));
            RuleFor(x => x.ObjectType)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Common.GenericAttributes.Fields.ObjectType.Required"));
            RuleFor(x => x.Key)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Common.GenericAttributes.Fields.Key.Required"));
            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Common.GenericAttributes.Fields.Value.Required"));
        }
    }
}
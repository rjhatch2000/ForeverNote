﻿using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Customers
{
    public class CustomerActionConditionValidator : BaseForeverNoteValidator<CustomerActionConditionModel>
    {
        public CustomerActionConditionValidator(
            IEnumerable<IValidatorConsumer<CustomerActionConditionModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerActionCondition.Fields.Name.Required"));
        }
    }
}
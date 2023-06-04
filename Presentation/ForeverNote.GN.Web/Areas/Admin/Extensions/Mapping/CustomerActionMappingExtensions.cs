﻿using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Helpers;
using ForeverNote.Web.Areas.Admin.Models.Customers;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class CustomerActionMappingExtensions
    {
        public static CustomerActionModel ToModel(this CustomerAction entity, IDateTimeHelper dateTimeHelper)
        {
            var action = entity.MapTo<CustomerAction, CustomerActionModel>();
            action.StartDateTime = entity.StartDateTimeUtc.ConvertToUserTime(dateTimeHelper);
            action.EndDateTime = entity.EndDateTimeUtc.ConvertToUserTime(dateTimeHelper);
            return action;
        }

        public static CustomerAction ToEntity(this CustomerActionModel model, IDateTimeHelper dateTimeHelper)
        {
            var action = model.MapTo<CustomerActionModel, CustomerAction>();
            action.StartDateTimeUtc = model.StartDateTime.ConvertToUtcTime(dateTimeHelper);
            action.EndDateTimeUtc = model.EndDateTime.ConvertToUtcTime(dateTimeHelper);
            return action;
        }

        public static CustomerAction ToEntity(this CustomerActionModel model, CustomerAction destination, IDateTimeHelper dateTimeHelper)
        {
            var action = model.MapTo(destination);
            action.StartDateTimeUtc = model.StartDateTime.ConvertToUtcTime(dateTimeHelper);
            action.EndDateTimeUtc = model.EndDateTime.ConvertToUtcTime(dateTimeHelper);
            return action;
        }

        public static CustomerActionConditionModel ToModel(this CustomerAction.ActionCondition entity)
        {
            return entity.MapTo<CustomerAction.ActionCondition, CustomerActionConditionModel>();
        }

        public static CustomerAction.ActionCondition ToEntity(this CustomerActionConditionModel model)
        {
            return model.MapTo<CustomerActionConditionModel, CustomerAction.ActionCondition>();
        }

        public static CustomerAction.ActionCondition ToEntity(this CustomerActionConditionModel model, CustomerAction.ActionCondition destination)
        {
            return model.MapTo(destination);
        }
    }
}
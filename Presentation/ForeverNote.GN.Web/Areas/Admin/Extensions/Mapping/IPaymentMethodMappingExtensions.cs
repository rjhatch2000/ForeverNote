﻿using ForeverNote.Services.Payments;
using ForeverNote.Web.Areas.Admin.Models.Payments;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class IPaymentMethodMappingExtensions
    {
        public static async Task<PaymentMethodModel> ToModel(this IPaymentMethod entity)
        {
            var paymentmethod = entity.MapTo<IPaymentMethod, PaymentMethodModel>();

            paymentmethod.SupportCapture = await entity.SupportCapture();
            paymentmethod.SupportPartiallyRefund = await entity.SupportPartiallyRefund();
            paymentmethod.SupportRefund = await entity.SupportRefund();
            paymentmethod.SupportVoid = await entity.SupportVoid();

            return paymentmethod;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ForeverNote.Core;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Payments;
using ForeverNote.Core.Plugins;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Common;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Orders;
using ForeverNote.Services.Payments;
using ForeverNote.Services.Tax;
using ForeverNote.Web.Features.Models.Checkout;
using ForeverNote.Web.Models.Checkout;
using MediatR;

namespace ForeverNote.Web.Features.Handlers.Checkout
{
    public class GetPaymentMethodHandler : IRequestHandler<GetPaymentMethod, CheckoutPaymentMethodModel>
    {
        private readonly IRewardPointsService _rewardPointsService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IPaymentService _paymentService;
        private readonly ILocalizationService _localizationService;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;

        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly PaymentSettings _paymentSettings;

        public GetPaymentMethodHandler(IRewardPointsService rewardPointsService,
            IOrderTotalCalculationService orderTotalCalculationService,
            ICurrencyService currencyService,
            IPriceFormatter priceFormatter,
            IPaymentService paymentService,
            ILocalizationService localizationService,
            ITaxService taxService,
            IWebHelper webHelper,
            RewardPointsSettings rewardPointsSettings,
            PaymentSettings paymentSettings)
        {
            _rewardPointsService = rewardPointsService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _currencyService = currencyService;
            _priceFormatter = priceFormatter;
            _paymentService = paymentService;
            _localizationService = localizationService;
            _taxService = taxService;
            _webHelper = webHelper;
            _rewardPointsSettings = rewardPointsSettings;
            _paymentSettings = paymentSettings;
        }

        public async Task<CheckoutPaymentMethodModel> Handle(GetPaymentMethod request, CancellationToken cancellationToken)
        {
            var model = new CheckoutPaymentMethodModel();

            //reward points
            if (_rewardPointsSettings.Enabled && !request.Cart.IsRecurring())
            {
                int rewardPointsBalance = await _rewardPointsService.GetRewardPointsBalance(request.Customer.Id, request.Store.Id);
                decimal rewardPointsAmountBase = await _orderTotalCalculationService.ConvertRewardPointsToAmount(rewardPointsBalance);
                decimal rewardPointsAmount = await _currencyService.ConvertFromPrimaryStoreCurrency(rewardPointsAmountBase, request.Currency);
                if (rewardPointsAmount > decimal.Zero &&
                    _orderTotalCalculationService.CheckMinimumRewardPointsToUseRequirement(rewardPointsBalance))
                {
                    model.DisplayRewardPoints = true;
                    model.RewardPointsAmount = _priceFormatter.FormatPrice(rewardPointsAmount, true, false);
                    model.RewardPointsBalance = rewardPointsBalance;
                    var shoppingCartTotalBase = (await _orderTotalCalculationService.GetShoppingCartTotal(request.Cart, useRewardPoints: true)).shoppingCartTotal;
                    model.RewardPointsEnoughToPayForOrder = (shoppingCartTotalBase.HasValue && shoppingCartTotalBase.Value == decimal.Zero);
                }
            }

            //filter by country
            var paymentMethods = (await _paymentService
                .LoadActivePaymentMethods(request.Customer, request.Store.Id, request.FilterByCountryId))
                .Where(pm => pm.PaymentMethodType == PaymentMethodType.Standard || pm.PaymentMethodType == PaymentMethodType.Redirection).ToList();
            var availablepaymentMethods = new List<IPaymentMethod>();
            foreach (var pm in paymentMethods)
            {
                if (!await pm.HidePaymentMethod(request.Cart))
                    availablepaymentMethods.Add(pm);
            }

            foreach (var pm in availablepaymentMethods)
            {
                if (request.Cart.IsRecurring() && pm.RecurringPaymentType == RecurringPaymentType.NotSupported)
                    continue;

                var pmModel = new CheckoutPaymentMethodModel.PaymentMethodModel {
                    Name = pm.GetLocalizedFriendlyName(_localizationService, request.Language.Id),
                    Description = _paymentSettings.ShowPaymentMethodDescriptions ? await pm.PaymentMethodDescription() : string.Empty,
                    PaymentMethodSystemName = pm.PluginDescriptor.SystemName,
                    LogoUrl = pm.PluginDescriptor.GetLogoUrl(_webHelper)
                };
                //payment method additional fee
                decimal paymentMethodAdditionalFee = await _paymentService.GetAdditionalHandlingFee(request.Cart, pm.PluginDescriptor.SystemName);
                decimal rateBase = (await _taxService.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, request.Customer)).paymentPrice;
                decimal rate = await _currencyService.ConvertFromPrimaryStoreCurrency(rateBase, request.Currency);
                if (rate > decimal.Zero)
                    pmModel.Fee = _priceFormatter.FormatPaymentMethodAdditionalFee(rate, true);

                model.PaymentMethods.Add(pmModel);
            }

            //find a selected (previously) payment method
            var selectedPaymentMethodSystemName = request.Customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.SelectedPaymentMethod, request.Store.Id);
            if (!string.IsNullOrEmpty(selectedPaymentMethodSystemName))
            {
                var paymentMethodToSelect = model.PaymentMethods.ToList()
                    .Find(pm => pm.PaymentMethodSystemName.Equals(selectedPaymentMethodSystemName, StringComparison.OrdinalIgnoreCase));
                if (paymentMethodToSelect != null)
                    paymentMethodToSelect.Selected = true;
            }
            //if no option has been selected, let's do it for the first one
            if (model.PaymentMethods.FirstOrDefault(so => so.Selected) == null)
            {
                var paymentMethodToSelect = model.PaymentMethods.FirstOrDefault();
                if (paymentMethodToSelect != null)
                    paymentMethodToSelect.Selected = true;
            }

            return model;

        }
    }
}

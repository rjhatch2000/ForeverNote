﻿using Google.Authenticator;
using ForeverNote.Core;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Common;
using ForeverNote.Services.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
    {
        private readonly IStoreContext _storeContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IServiceProvider _serviceProvider;
        private TwoFactorAuthenticator _twoFactorAuthentication;

        public TwoFactorAuthenticationService(
            IStoreContext storeContext,
            IWorkflowMessageService workflowMessageService,
            IGenericAttributeService genericAttributeService,
            IServiceProvider serviceProvider)
        {
            _storeContext = storeContext;
            _workflowMessageService = workflowMessageService;
            _genericAttributeService = genericAttributeService;
            _serviceProvider = serviceProvider;
            _twoFactorAuthentication = new TwoFactorAuthenticator();
        }

        public virtual async Task<bool> AuthenticateTwoFactor(string secretKey, string token, Customer customer, TwoFactorAuthenticationType twoFactorAuthenticationType)
        {
            switch (twoFactorAuthenticationType)
            {
                case TwoFactorAuthenticationType.AppVerification:
                    return _twoFactorAuthentication.ValidateTwoFactorPIN(secretKey, token.Trim());

                case TwoFactorAuthenticationType.EmailVerification:
                    var customertoken = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.TwoFactorValidCode);
                    if (customertoken != token.Trim())
                        return false;
                    var validuntil = customer.GetAttributeFromEntity<DateTime>(SystemCustomerAttributeNames.TwoFactorCodeValidUntil);
                    if (validuntil < DateTime.UtcNow)
                        return false;

                    return true;
                case TwoFactorAuthenticationType.SMSVerification:
                    var smsVerificationService = _serviceProvider.GetRequiredService<ISMSVerificationService>();
                    return await smsVerificationService.Authenticate(secretKey, token.Trim(), customer);
                default:
                    return false;
            }
        }

        public virtual async Task<TwoFactorCodeSetup> GenerateCodeSetup(string secretKey, Customer customer, Language language, TwoFactorAuthenticationType twoFactorAuthenticationType)
        {
            var model = new TwoFactorCodeSetup();

            switch (twoFactorAuthenticationType)
            {
                case TwoFactorAuthenticationType.AppVerification:
                    var setupInfo = _twoFactorAuthentication.GenerateSetupCode(_storeContext.CurrentStore.CompanyName, customer.Email, secretKey, false, 3);
                    model.CustomValues.Add("QrCodeImageUrl", setupInfo.QrCodeSetupImageUrl);
                    model.CustomValues.Add("ManualEntryQrCode", setupInfo.ManualEntryKey);
                    break;

                case TwoFactorAuthenticationType.EmailVerification:
                    var token = PrepareRandomCode();
                    await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TwoFactorValidCode, token);
                    await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TwoFactorCodeValidUntil, DateTime.UtcNow.AddMinutes(30));
                    model.CustomValues.Add("Token", token);
                    await _workflowMessageService.SendCustomerEmailTokenValidationMessage(customer, _storeContext.CurrentStore, token, language.Id);
                    break;

                case TwoFactorAuthenticationType.SMSVerification:
                    var smsVerificationService = _serviceProvider.GetRequiredService<ISMSVerificationService>();
                    model = await smsVerificationService.GenerateCode(secretKey, customer, language);
                    break;

                default:
                    break;
            }

            return model;
        }

        private string PrepareRandomCode()
        {
            Random generator = new Random();
            return generator.Next(0, 999999).ToString("D6");
        }
    }
}

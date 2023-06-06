using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Common;
using ForeverNote.Services.Messages;
using Google.Authenticator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
    {
        private readonly CommonSettings _commonSettings;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IServiceProvider _serviceProvider;
        private TwoFactorAuthenticator _twoFactorAuthentication;

        public TwoFactorAuthenticationService(
            CommonSettings commonSettings,
            IWorkflowMessageService workflowMessageService,
            IGenericAttributeService genericAttributeService,
            IServiceProvider serviceProvider
        )
        {
            _commonSettings = commonSettings;
            _workflowMessageService = workflowMessageService;
            _genericAttributeService = genericAttributeService;
            _serviceProvider = serviceProvider;
            _twoFactorAuthentication = new TwoFactorAuthenticator();
        }

        public virtual async Task<bool> AuthenticateTwoFactor(string secretKey, string token, User user, TwoFactorAuthenticationType twoFactorAuthenticationType)
        {
            switch (twoFactorAuthenticationType)
            {
                case TwoFactorAuthenticationType.AppVerification:
                    return _twoFactorAuthentication.ValidateTwoFactorPIN(secretKey, token.Trim());

                case TwoFactorAuthenticationType.EmailVerification:
                    var usertoken = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.TwoFactorValidCode);
                    if (usertoken != token.Trim())
                        return false;
                    var validuntil = user.GetAttributeFromEntity<DateTime>(SystemUserAttributeNames.TwoFactorCodeValidUntil);
                    if (validuntil < DateTime.UtcNow)
                        return false;

                    return true;
                case TwoFactorAuthenticationType.SMSVerification:
                    var smsVerificationService = _serviceProvider.GetRequiredService<ISMSVerificationService>();
                    return await smsVerificationService.Authenticate(secretKey, token.Trim(), user);
                default:
                    return false;
            }
        }

        public virtual async Task<TwoFactorCodeSetup> GenerateCodeSetup(string secretKey, User user, Language language, TwoFactorAuthenticationType twoFactorAuthenticationType)
        {
            var model = new TwoFactorCodeSetup();

            switch (twoFactorAuthenticationType)
            {
                case TwoFactorAuthenticationType.AppVerification:
                    var setupInfo = _twoFactorAuthentication.GenerateSetupCode(_commonSettings.Sitename, user.Email, secretKey, false, 3);
                    model.CustomValues.Add("QrCodeImageUrl", setupInfo.QrCodeSetupImageUrl);
                    model.CustomValues.Add("ManualEntryQrCode", setupInfo.ManualEntryKey);
                    break;

                case TwoFactorAuthenticationType.EmailVerification:
                    var token = PrepareRandomCode();
                    await _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.TwoFactorValidCode, token);
                    await _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.TwoFactorCodeValidUntil, DateTime.UtcNow.AddMinutes(30));
                    model.CustomValues.Add("Token", token);
                    await _workflowMessageService.SendUserEmailTokenValidationMessage(user, token, language.Id);
                    break;

                case TwoFactorAuthenticationType.SMSVerification:
                    var smsVerificationService = _serviceProvider.GetRequiredService<ISMSVerificationService>();
                    model = await smsVerificationService.GenerateCode(secretKey, user, language);
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

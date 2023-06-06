using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public interface ITwoFactorAuthenticationService
    {
        Task<bool> AuthenticateTwoFactor(string secretKey, string token, User user, TwoFactorAuthenticationType twoFactorAuthenticationType);

        Task<TwoFactorCodeSetup> GenerateCodeSetup(string secretKey, User user, Language language, TwoFactorAuthenticationType twoFactorAuthenticationType);
        
    }
}

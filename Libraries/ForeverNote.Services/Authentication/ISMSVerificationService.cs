using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public interface ISMSVerificationService
    {
        Task<bool> Authenticate(string secretKey, string token, User user);
        Task<TwoFactorCodeSetup> GenerateCode(string secretKey, User user, Language language);
    }
}

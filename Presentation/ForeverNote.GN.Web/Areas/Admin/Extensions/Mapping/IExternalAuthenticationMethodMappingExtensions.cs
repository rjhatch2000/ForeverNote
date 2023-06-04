using ForeverNote.Services.Authentication.External;
using ForeverNote.Web.Areas.Admin.Models.ExternalAuthentication;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class IExternalAuthenticationMethodMappingExtensions
    {
        public static AuthenticationMethodModel ToModel(this IExternalAuthenticationMethod entity)
        {
            return entity.MapTo<IExternalAuthenticationMethod, AuthenticationMethodModel>();
        }
    }
}
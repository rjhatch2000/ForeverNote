using ForeverNote.Core.Extensions;
using ForeverNote.Services.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class InstallationService
    {
        protected virtual async Task InstallLocaleResources()
        {
            //'English' language
            var language = _languageRepository.Table.Single(l => l.Name == "English");

            //save resources
            var filePath = CommonPath.MapPath("App_Data/Resources/DefaultLanguage.xml");
            var localesXml = File.ReadAllText(filePath);
            var translationService = _serviceProvider.GetRequiredService<ITranslationService>();
            await translationService.ImportResourcesFromXmlInstall(language, localesXml);
        }
    }
}

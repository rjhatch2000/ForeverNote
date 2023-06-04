using ForeverNote.Core.Plugins;
using System.Collections.Generic;

namespace ForeverNote.Web.Framework.Themes
{
    public partial interface IThemeProvider
    {
        ThemeConfiguration GetThemeConfiguration(string themeName);

        IList<ThemeConfiguration> GetThemeConfigurations();

        bool ThemeConfigurationExists(string themeName);

        ThemeDescriptor GetThemeDescriptorFromText(string text);
    }
}

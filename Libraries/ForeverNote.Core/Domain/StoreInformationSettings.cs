﻿using ForeverNote.Core.Configuration;

namespace ForeverNote.Core.Domain
{
    public class StoreInformationSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether "Forever-note.com Powered by ForeverNote" text should be displayed.
        /// </summary>
        public bool HidePoweredByForeverNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether store is closed
        /// </summary>
        public bool StoreClosed { get; set; }

        /// <summary>
        /// Gets or sets a default store theme
        /// </summary>
        public string DefaultStoreTheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select a theme
        /// </summary>
        public bool AllowUserToSelectTheme { get; set; }

        /// <summary>
        /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
        /// </summary>
        public string LogoPictureId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether we should display warnings about the new EU cookie law
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        /// <summary>
        /// Gets or sets a value of Facebook page URL of the site
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Twitter page URL of the site
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or sets a value of YouTube channel URL of the site
        /// </summary>
        public string YoutubeLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Instagram page URL of the site
        /// </summary>
        public string InstagramLink { get; set; }

        /// <summary>
        /// Gets or sets a value of LinkedIn page URL of the site
        /// </summary>
        public string LinkedInLink { get; set; }

        /// <summary>
        /// Gets or sets a value of Pinterest page URL of the site
        /// </summary>
        public string PinterestLink { get; set; }
    }
}

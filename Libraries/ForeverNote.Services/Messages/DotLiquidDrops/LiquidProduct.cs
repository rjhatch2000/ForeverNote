using DotLiquid;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidProduct : Drop
    {
        private readonly CommonSettings _commonSettings;
        private readonly Product _product;
        private readonly Language _language;

        public LiquidProduct(
            CommonSettings commonSettings,
            Product product,
            Language language
        )
        {
            _commonSettings = commonSettings;
            _product = product;
            _language = language;
            AdditionalTokens = new Dictionary<string, string>();
        }

        public string Id
        {
            get { return _product.Id.ToString(); }
        }

        public string Name
        {
            get { return _product.GetLocalized(x => x.Name, _language.Id); }
        }

        public string ShortDescription
        {
            get { return _product.GetLocalized(x => x.ShortDescription, _language.Id); }
        }

        public string ProductURLForCustomer
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    _commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url,
                    "/product/",
                    _product.Id
                );
            }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}
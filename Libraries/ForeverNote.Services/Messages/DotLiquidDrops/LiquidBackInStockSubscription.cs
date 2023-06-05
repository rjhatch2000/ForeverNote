using DotLiquid;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Localization;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidBackInStockSubscription : Drop
    {

        private readonly CommonSettings _commonSettings;
        private readonly Product _product;

        public LiquidBackInStockSubscription(
            CommonSettings commonSettings,
            Product product
        )
        {
            _commonSettings = commonSettings;
            _product = product;

            AdditionalTokens = new Dictionary<string, string>();
        }

        public string ProductName
        {
            get { return _product.Name; }
        }

        public string ProductUrl
        {
            get { return string.Format("{0}{1}{2}",
                _commonSettings.SslEnabled ? _commonSettings.SecureUrl : _commonSettings.Url,
                "/product/",
                _product.Id); }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}
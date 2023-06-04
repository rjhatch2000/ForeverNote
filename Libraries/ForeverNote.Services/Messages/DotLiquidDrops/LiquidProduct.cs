using DotLiquid;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Seo;
using System.Collections.Generic;

namespace ForeverNote.Services.Messages.DotLiquidDrops
{
    public partial class LiquidProduct : Drop
    {
        private Product _product;
        private Language _language;
        private Store _store;

        public LiquidProduct(Product product, Language language, Store store)
        {
            this._product = product;
            this._language = language;
            this._store = store;
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

        public string SKU
        {
            get { return _product.Sku; }
        }

        public string StockQuantity
        {
            get { return _product.GetTotalStockQuantity().ToString(); }
        }

        public decimal Price
        {
            get { return _product.Price; }
        }

        public string ProductURLForCustomer
        {
            get { return string.Format("{0}{1}", (_store.SslEnabled ? _store.SecureUrl : _store.Url), _product.GetSeName(_language.Id)); }
        }

        public IDictionary<string, string> AdditionalTokens { get; set; }
    }
}
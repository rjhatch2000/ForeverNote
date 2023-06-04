﻿using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Media;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Catalog
{
    public partial class ManufacturerModel : BaseForeverNoteEntityModel
    {
        public ManufacturerModel()
        {
            PictureModel = new PictureModel();
            FeaturedProducts = new List<ProductOverviewModel>();
            Products = new List<ProductOverviewModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string Icon { get; set; }
        public PictureModel PictureModel { get; set; }
        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<ProductOverviewModel> FeaturedProducts { get; set; }
        public IList<ProductOverviewModel> Products { get; set; }
    }
}
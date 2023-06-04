using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Web.Areas.Admin.Models.Catalog;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class ProductReviewMappingExtensions
    {
        public static ProductReview ToEntity(this ProductReviewModel model, ProductReview destination)
        {
            return model.MapTo(destination);
        }
    }
}
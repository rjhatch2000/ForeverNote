using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Areas.Admin.Models.Orders;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class GiftCardMappingExtensions
    {
        public static GiftCardModel ToModel(this GiftCard entity)
        {
            return entity.MapTo<GiftCard, GiftCardModel>();
        }

        public static GiftCard ToEntity(this GiftCardModel model)
        {
            return model.MapTo<GiftCardModel, GiftCard>();
        }

        public static GiftCard ToEntity(this GiftCardModel model, GiftCard destination)
        {
            return model.MapTo(destination);
        }
    }
}
﻿using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Areas.Admin.Models.Messages;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class NewsLetterSubscriptionMappingExtensions
    {
        public static NewsLetterSubscriptionModel ToModel(this NewsLetterSubscription entity)
        {
            return entity.MapTo<NewsLetterSubscription, NewsLetterSubscriptionModel>();
        }

        public static NewsLetterSubscription ToEntity(this NewsLetterSubscriptionModel model)
        {
            return model.MapTo<NewsLetterSubscriptionModel, NewsLetterSubscription>();
        }

        public static NewsLetterSubscription ToEntity(this NewsLetterSubscriptionModel model, NewsLetterSubscription destination)
        {
            return model.MapTo(destination);
        }
    }
}
﻿using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Areas.Admin.Models.Messages;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class NewsletterCategoryMappingExtensions
    {
        public static NewsletterCategoryModel ToModel(this NewsletterCategory entity)
        {
            return entity.MapTo<NewsletterCategory, NewsletterCategoryModel>();
        }

        public static NewsletterCategory ToEntity(this NewsletterCategoryModel model)
        {
            return model.MapTo<NewsletterCategoryModel, NewsletterCategory>();
        }

        public static NewsletterCategory ToEntity(this NewsletterCategoryModel model, NewsletterCategory destination)
        {
            return model.MapTo(destination);
        }
    }
}
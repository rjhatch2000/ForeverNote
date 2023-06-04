﻿using ForeverNote.Core.Domain.Topics;
using ForeverNote.Web.Areas.Admin.Models.Topics;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class TopicMappingExtensions
    {
        public static TopicModel ToModel(this Topic entity)
        {
            return entity.MapTo<Topic, TopicModel>();
        }

        public static Topic ToEntity(this TopicModel model)
        {
            return model.MapTo<TopicModel, Topic>();
        }

        public static Topic ToEntity(this TopicModel model, Topic destination)
        {
            return model.MapTo(destination);
        }
    }
}
﻿using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Areas.Admin.Models.Messages;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class QueuedEmailMappingExtensions
    {
        public static QueuedEmailModel ToModel(this QueuedEmail entity)
        {
            return entity.MapTo<QueuedEmail, QueuedEmailModel>();
        }

        public static QueuedEmail ToEntity(this QueuedEmailModel model)
        {
            return model.MapTo<QueuedEmailModel, QueuedEmail>();
        }

        public static QueuedEmail ToEntity(this QueuedEmailModel model, QueuedEmail destination)
        {
            return model.MapTo(destination);
        }
    }
}
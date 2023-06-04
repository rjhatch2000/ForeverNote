﻿using ForeverNote.Core.Domain.Directory;
using ForeverNote.Web.Areas.Admin.Models.Directory;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class MeasureWeightMappingExtensions
    {
        public static MeasureWeightModel ToModel(this MeasureWeight entity)
        {
            return entity.MapTo<MeasureWeight, MeasureWeightModel>();
        }

        public static MeasureWeight ToEntity(this MeasureWeightModel model)
        {
            return model.MapTo<MeasureWeightModel, MeasureWeight>();
        }

        public static MeasureWeight ToEntity(this MeasureWeightModel model, MeasureWeight destination)
        {
            return model.MapTo(destination);
        }
    }
}
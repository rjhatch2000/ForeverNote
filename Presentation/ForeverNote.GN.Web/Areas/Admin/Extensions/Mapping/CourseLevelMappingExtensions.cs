﻿using ForeverNote.Core.Domain.Courses;
using ForeverNote.Web.Areas.Admin.Models.Courses;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class CourseLevelMappingExtensions
    {
        public static CourseLevelModel ToModel(this CourseLevel entity)
        {
            return entity.MapTo<CourseLevel, CourseLevelModel>();
        }

        public static CourseLevel ToEntity(this CourseLevelModel model)
        {
            return model.MapTo<CourseLevelModel, CourseLevel>();
        }

        public static CourseLevel ToEntity(this CourseLevelModel model, CourseLevel destination)
        {
            return model.MapTo(destination);
        }
    }
}
using AutoMapper;
using ForeverNote.Core.Domain.Courses;
using ForeverNote.Core.Infrastructure.Mapper;
using ForeverNote.Web.Areas.Admin.Models.Courses;

namespace ForeverNote.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CourseSubjectProfile : Profile, IMapperProfile
    {
        public CourseSubjectProfile()
        {
            CreateMap<CourseSubject, CourseSubjectModel>();
            CreateMap<CourseSubjectModel, CourseSubject>()
                .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}
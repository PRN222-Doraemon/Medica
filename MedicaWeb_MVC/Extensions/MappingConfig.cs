using AutoMapper;
using Core.Entities;
using Elfie.Serialization;
using MedicaWeb_MVC.ViewModels;
using Resource = Core.Entities.Resource;

namespace MedicaWeb_MVC.Extensions
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Course, CourseVM>()
                .ForMember(dest => dest.CreatedByUserName, u => u.MapFrom(src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"))
                .ForMember(dest => dest.CategoryName, u => u.MapFrom(src => src.Category.Name));

                config.CreateMap<CourseChapter, CourseChapterVM>();
                config.CreateMap<Resource, ResourceVM>();
                config.CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.UserName, u => u.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            });
        }
    }
}

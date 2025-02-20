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
                .ForMember(dest => dest.CategoryName, u => u.MapFrom(src => src.Category.Name)).ReverseMap();

                config.CreateMap<Course, CourseCreateVM>()
                .ForMember(dest => dest.CategoryName, u => u.MapFrom(src => src.Category.Name));
                config.CreateMap<CourseCreateVM, Course>();

                config.CreateMap<CourseChapter, CourseChapterCreateVM>().ReverseMap();
                config.CreateMap<ResourceCreateVM, Resource>().ReverseMap();

                config.CreateMap<CourseChapter, CourseChapterVM>().ReverseMap();
                config.CreateMap<Resource, ResourceVM>().ReverseMap();
                config.CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.UserName, u => u.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

                config.CreateMap<Feedback, FeedbackVM>()
                .ForMember(dest => dest.StudentName, u => u.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}"))
                .ForMember(dest => dest.StudentImgUrl, u => u.MapFrom(src => src.Student.ImageUrl));

            });
        }
    }
}

using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
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

                // ===================================
                // === Courses & Chapters & Resources
                // ===================================

                config.CreateMap<Course, CourseVM>()
                .ForMember(dest => dest.CreatedByUserName, u => u.MapFrom(src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"))
                .ForMember(dest => dest.CategoryName, u => u.MapFrom(src => src.Category.Name)).ReverseMap();
                config.CreateMap<CourseChapter, CourseChapterVM>();
                config.CreateMap<Resource, ResourceVM>();

                config.CreateMap<Course, CourseCreateVM>()
                .ForMember(dest => dest.CategoryName, u => u.MapFrom(src => src.Category.Name));

                config.CreateMap<CourseCreateVM, Course>()
                .ForMember(dest => dest.CreatedByUserID, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Classrooms, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

                config.CreateMap<CourseChapterCreateVM, CourseChapter>().ReverseMap();
                config.CreateMap<ResourceCreateVM, Resource>().ReverseMap();


                // ==============================
                // === Comments & Feedbacks
                // ==============================

                config.CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.UserName, u => u.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

                config.CreateMap<Feedback, FeedbackVM>()
                .ForMember(dest => dest.StudentName, u => u.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}"))
                .ForMember(dest => dest.StudentImgUrl, u => u.MapFrom(src => src.Student.ImageUrl));

                // ==============================
                // === Accounts
                // ==============================

                config.CreateMap<RegisterVM, ApplicationUser>();
                config.CreateMap<Lecturer, LecturerVM>();

                // ==============================
                // === Classrooms
                // ==============================

                config.CreateMap<Classroom, ClassVM>();

                // ==============================
                // === Cart
                // ==============================

                config.CreateMap<CartItem, CartItemVM>()
                   .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                   .ForMember(dest => dest.InstructorName, opt => opt.Ignore())
                   .ForMember(dest => dest.StartDate, opt => opt.Ignore())
                   .ForMember(dest => dest.Duration, opt => opt.Ignore())
                   .ForMember(dest => dest.Mode, opt => opt.Ignore())
                   .ForMember(dest => dest.Description, opt => opt.Ignore())
                   .ReverseMap();
            });
        }
    }
}

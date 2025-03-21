﻿using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using MedicaWeb_MVC.ViewModels.Classes;
using MedicaWeb_MVC.ViewModels.Courses;
using MedicaWeb_MVC.ViewModels.Orders;
using MedicaWeb_MVC.ViewModels.User;
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
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Classrooms, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

                config.CreateMap<CourseChapterCreateVM, CourseChapter>().ReverseMap();
                config.CreateMap<ResourceCreateVM, Resource>().ReverseMap();


                // ==============================
                // === Comments & Feedbacks
                // ==============================

                config.CreateMap<Comment, CommentVM>()
                .ForMember(dest => dest.UserName, u => u.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.UserImgUrl, u => u.MapFrom(src => $"{src.User.ImageUrl}"));
                config.CreateMap<CommentCreateVM, Comment>();

                config.CreateMap<Feedback, FeedbackVM>()
                .ForMember(dest => dest.StudentName, u => u.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}"))
                .ForMember(dest => dest.StudentImgUrl, u => u.MapFrom(src => src.Student.ImageUrl));

                config.CreateMap<FeedbackUpsertVM, Feedback>()
                .ForMember(dest => dest.StudentId, opt => opt.Ignore());

                // ==============================
                // === Accounts
                // ==============================

                config.CreateMap<RegisterVM, ApplicationUser>();
                config.CreateMap<Lecturer, LecturerVM>();
                config.CreateMap<Student, StudentVM>();

                // ==============================
                // === Classrooms
                // ==============================

                config.CreateMap<Classroom, ClassVM>()
                .ForMember(dest => dest.Students, u => u.MapFrom(src => src.OrderDetails != null ? src.OrderDetails.Select(od => od.Order.Student).ToList() : new List<Student>()));
                config.CreateMap<ClassUpsertVM, Classroom>();
                config.CreateMap<Classroom, ClassUpsertVM>();

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

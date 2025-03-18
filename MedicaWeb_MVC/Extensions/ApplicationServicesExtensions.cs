using AutoMapper;
using Core.Helpers;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace MedicaWeb_MVC.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Register Auto Mapper
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register service
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILecturerService, LecturerService>();
            services.AddScoped<ICommentService, CommentService>();


            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, StripePaymentService>();

            // config cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}

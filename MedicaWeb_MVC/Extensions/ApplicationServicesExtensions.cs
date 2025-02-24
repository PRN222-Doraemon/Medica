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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IOrderService, OrderService>();

            // config cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }
    }
}

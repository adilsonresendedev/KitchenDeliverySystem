using KitchenDeliverySystem.Application.UseCases;
using KitchenDeliverySystem.Application.UseCases.Order.OrderCreate;
using KitchenDeliverySystem.Application.UseCases.Order.OrderDelete;
using KitchenDeliverySystem.Application.UseCases.Order.OrderGet;
using KitchenDeliverySystem.Application.UseCases.Order.OrderSearch;
using KitchenDeliverySystem.Application.UseCases.Order.OrderUpdate;
using KitchenDeliverySystem.Application.UseCases.User.UserInsert;
using KitchenDeliverySystem.Application.UseCases.User.UserLogin;
using KitchenDeliverySystem.CrossCutting.Options;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Infra.Context;
using KitchenDeliverySystem.Infra.Mappers;
using KitchenDeliverySystem.Infra.Persistence;
using KitchenDeliverySystem.Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace KitchenDeliverySystem.Api.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IUserLoginUseCase, UserLoginUseCase>();
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IDeleteOrderUseCase, DeleteOrderUseCase>();
            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
            services.AddScoped<IUpdateOrderUseCase, UpdateOrderUseCase>();
            services.AddScoped<ISearchOrderUseCase, SearchOrderUseCase>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();

            return services;
        }

        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(UserProfile).Assembly,
                typeof(OrderProfile).Assembly,
                typeof(OrderItemProfile).Assembly,
                typeof(OrderFilterProfile).Assembly);

            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            return services;
        }
    }
}

using KitchenDeliverySystem.Application.UseCases;

namespace KitchenDeliverySystem.Api.ServiceExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            var useCaseType = typeof(IUseCase);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => useCaseType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            foreach (var type in types)
            {
                services.AddTransient(useCaseType, type);
            }

            return services;
        }
    }
}

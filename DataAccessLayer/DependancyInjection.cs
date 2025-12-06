using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.ProductsService.DataAccessLayer;

public static class DependancyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
    {
        //services.AddDbContext<ApplicationDbContext>();
        return services;
    }
}

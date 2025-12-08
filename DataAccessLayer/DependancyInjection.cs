using eCommerce.DataAccessLayer.Context;
using eCommerce.ProductsService.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.ProductsService.DataAccessLayer;

public static class DependancyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(option => {
            option.UseMySQL(configuration.GetConnectionString("DefaulConnection")!);
            });
        return services;
    }
}

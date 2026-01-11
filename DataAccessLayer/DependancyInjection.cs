using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.Repositories;
using eCommerce.DataAccessLayer.RepositoryContracts;
using eCommerce.ProductsService.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.ProductsService.DataAccessLayer;

public static class DependancyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {

        string TempconnectionString = configuration.GetConnectionString("DefaultConnection")!;

      string connnctionstring =  TempconnectionString.Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
              .Replace("$MYSQL_PASSWORD",Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));
            
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseMySQL(connnctionstring);
        });
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}

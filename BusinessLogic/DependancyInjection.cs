using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.ProductsService.BuinessLogicLayer;

public static class DependancyInjection
{
    public static IServiceCollection AddBuinessLogicLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        return services;
    }
}

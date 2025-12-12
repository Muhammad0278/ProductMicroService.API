using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.BusinessLogicLayer.Services;
using eCommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace eCommerce.ProductMicroService.API.APIEndpoints;

public static class ProductAPIEndpoint
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //Get /api/products
        app.MapGet("/api/products", async (IProductsService productsService ) =>
        {
            List<ProductResponse?> products = await productsService.GetProducts();
            return Results.Ok(products);
        });
        //Get /api/products/search/product-id/000000-000000
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (Guid ProductID, IProductsService productService) =>
        {
            ProductResponse? product = await productService.GetProductByCondition(temp=>temp.ProductID==ProductID);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });

        //Get /api/products/search/product-id/000000-000000
        app.MapGet("/api/products/search/{SearchString}", async (string SearchString, IProductsService productService) =>
        {
            List<ProductResponse?> productbyName = await productService.GetProductsByCondition(temp => temp.ProductName != null 
            && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
            return productbyName is not null ? Results.Ok(productbyName) : Results.NotFound();
        });

        //Get /api/products/search/000000-000000
        app.MapGet("/api/products/search/{SearchString}", async (string SearchString, IProductsService productService) =>
        {
            List<ProductResponse?> productbyName = await productService.GetProductsByCondition(temp => temp.ProductName != null
          && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productbyCategory = await productService.GetProductsByCondition(temp => temp.Category != null
            && temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));
           
            var products = productbyName.Union(productbyCategory).ToList();


            return products is not null ? Results.Ok(products) : Results.NotFound();
        });

        //Post /api/products
        app.MapPost("/api/products", async (ProductAddRequest productAddRequest, IProductsService productService , IValidator<ProductAddRequest> ProductAddReqestValidator) =>
        {
            //Validate the ProductAddRequest object using Fluent Validation
            ValidationResult validtionResult = await ProductAddReqestValidator.ValidateAsync(productAddRequest);


            if (!validtionResult.IsValid)
            {
                Dictionary<string, string[]> errors = validtionResult.Errors
                  .GroupBy(k => k.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }


            var addedProductResponse = await productService.AddProduct(productAddRequest);
            if (addedProductResponse != null)
                return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
            else
                return Results.Problem("Error in adding product");
        });

        //PUT /api/products
        app.MapPut("/api/products", async (IProductsService productsService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
        {
            //Validate the ProductUpdateRequest object using Fluent Validation
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());
                return Results.ValidationProblem(errors);
            }


            var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
            if (updatedProductResponse != null)
                return Results.Ok(updatedProductResponse);
            else
                return Results.Problem("Error in updating product");
        });


        //DELETE /api/products/xxxx
        app.MapDelete("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            bool isDeleted = await productsService.DeleteProduct(ProductID);
            if (isDeleted)
                return Results.Ok(true);
            else
                return Results.Problem("Error in deleting product");
        });

        return app;
    }
}

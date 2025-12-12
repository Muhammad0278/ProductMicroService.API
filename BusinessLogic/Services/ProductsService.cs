using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.BusinessLogicLayer.Validators;
using eCommerce.DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.Repositories;
using eCommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;
using System.Web;


namespace eCommerce.BusinessLogicLayer.Services;

public class ProductsService : IProductsService
{
    private readonly IValidator<ProductAddRequest> _ProductAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productReporsitory;

    public ProductsService(IValidator<ProductAddRequest> ProductAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator,
        IMapper mapper,
        IProductRepository productReporsitory)
    {
        _ProductAddRequestValidator = ProductAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productReporsitory = productReporsitory;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }
        ValidationResult validationResult = await _ProductAddRequestValidator.ValidateAsync(productAddRequest);
        // check valudaiton
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);

        }
        Product productdata = _mapper.Map<Product>(productAddRequest);
        Product? addedproduct = await _productReporsitory.AddProduct(productdata);

        if (addedproduct == null)
        {
            return null;
        }
        ProductResponse addedproductrespons = _mapper.Map<ProductResponse>(addedproduct);
        return addedproductrespons;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _productReporsitory.GetProductByCondition(temp => temp.ProductID == productID);
        if (existingProduct == null)
        {
            return false;
        }
        bool isDeleted = await _productReporsitory.DeleteProduct(productID);
        return isDeleted;
    }


    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {

        Product? product = await _productReporsitory.GetProductByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }
        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
        return productResponse;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> product = await _productReporsitory.GetProducts();
        if (product == null)
        {
            return null;
        }
        IEnumerable<ProductResponse?> productResponse = _mapper.Map<IEnumerable<ProductResponse>>(product);
       
        return productResponse.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> product = await _productReporsitory.GetProductsByCondition(conditionExpression);
        if (product == null)
        {
            return null;
        }
        IEnumerable<ProductResponse?> productResponse = _mapper.Map<IEnumerable<ProductResponse>>(product);

        return productResponse.ToList();
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await _productReporsitory.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid ProductID");
        }

        //Validate the product using Fluent Validation
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        // Check the validation result
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2, ...
            throw new ArgumentException(errors);
        }


        //Map from ProductUpdateRequest to Product type
        Product product = _mapper.Map<Product>(productUpdateRequest); 

        Product? updatedProduct = await _productReporsitory.UpdateProduct(product);

        ProductResponse? updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);

        return updatedProductResponse;
    }
}

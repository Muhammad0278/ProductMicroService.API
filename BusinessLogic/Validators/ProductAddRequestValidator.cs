using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.BusinessLogicLayer.Validators;

public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
{
    public ProductAddRequestValidator()
    {
        //productname
        RuleFor(temp => temp.ProductName)
          .NotEmpty().WithMessage("Product Name can't be blank");

        //categoy
        RuleFor(temp => temp.Category)
          .IsInEnum().WithMessage("Category Name can't be blank");

        //unitprice
        RuleFor(temp => temp.UnitPrice)
          .InclusiveBetween(0, double.MaxValue).WithMessage($"Unit Price should between 0 to {double.MaxValue}");

        RuleFor(temp => temp.QuantityInStock)
          .InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity in Stock should between 0 to {int.MaxValue}");
    }
}
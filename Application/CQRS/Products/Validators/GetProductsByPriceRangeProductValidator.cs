using Application.CQRS.Products.Handlers;
using FluentValidation;

public class GetProductsByPriceRangeProductValidator : AbstractValidator<GetProductsByPriceRange.ProductRangeCommand>
{
    public GetProductsByPriceRangeProductValidator()
    {
        
    }
}



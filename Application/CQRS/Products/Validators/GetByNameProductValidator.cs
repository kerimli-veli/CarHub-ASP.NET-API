using Application.CQRS.Products.Handlers;
using FluentValidation;

namespace Application.CQRS.Products.Validators;

public class GetByNameProductValidator : AbstractValidator<GetByNameProduct.ProductGetByNameQuery>
{
    public GetByNameProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Məhsul adı boş ola bilməz.")
            .MinimumLength(3).WithMessage("Məhsul adı ən azı 3 simvoldan ibarət olmalıdır.")
            .MaximumLength(100).WithMessage("Məhsul adı ən çox 100 simvol ola bilər.");

        
    }
}

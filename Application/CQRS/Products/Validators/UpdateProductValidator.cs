using Application.CQRS.Products.Handlers;
using Application.CQRS.Products.ResponsesDto;
using FluentValidation;

namespace Application.CQRS.Products.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProduct.UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Məhsul adı boş ola bilməz.")
            .MinimumLength(3).WithMessage("Məhsul adı ən azı 3 simvoldan ibarət olmalıdır.")
            .MaximumLength(100).WithMessage("Məhsul adı ən çox 100 simvol ola bilər.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kateqoriya ID-si müsbət bir rəqəm olmalıdır.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Qiymət sıfırdan böyük olmalıdır.");

        RuleFor(x => x.UnitsInStock)
            .GreaterThanOrEqualTo(0).WithMessage("Stokda ən azı 0 ədəd olmalıdır.");

        RuleFor(x => x.Description)
            .MinimumLength(5).WithMessage("Təsvir ən azı 5 simvoldan ibarət olmalıdır.")
            .MaximumLength(500).WithMessage("Təsvir ən çox 500 simvol ola bilər.");
    }
}

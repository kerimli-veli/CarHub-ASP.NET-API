using Application.CQRS.Categories.Handlers;
using FluentValidation;

namespace Application.CQRS.Categories.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategory.CategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş ola bilməz.")
            .MinimumLength(3).WithMessage("Kategori adı ən azı 3 simvoldan ibarət olmalıdır.")
            .MaximumLength(100).WithMessage("Kategori adı ən çox 100 simvol ola bilər.")
            .Matches("^[a-zA-ZğüşöçıİĞÜŞÖÇ ]+$").WithMessage("Kategori adına ancaq hərflər və boşluq daxil edilə ola bilər.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Təsvir ən çox 500 simvol ola bilər.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description)); // verilibse yoxlanacaq
    }
}

using FluentValidation;
using MediatR;

using System.ComponentModel.DataAnnotations;
using ValidationException = FluentValidation.ValidationException;

namespace Application.PipelineBehaviour;

public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validator;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validator)
    {
        _validator = validator;
    }
    static ValidationPipelineBehaviour()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("az");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);


        var validationResults = await Task.WhenAll(_validator.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}

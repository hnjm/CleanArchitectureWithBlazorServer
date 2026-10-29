
namespace CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

public sealed class
    ValidationExceptionHandler<TRequest, TResponse> : MessageExceptionHandler<TRequest, TResponse, ValidationException>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{

    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(TRequest request, ValidationException exception,
        CancellationToken cancellationToken)
    {
        var errors = exception.Errors.Select(x => x.ErrorMessage).Distinct().ToArray();
        var failureResult = CreateFailureResult(errors);
        return Handled(failureResult);
    }

    private TResponse CreateFailureResult(string[] errors)
    {
        return ResultFailureFactory.Create<TResponse>(errors);
    }
}

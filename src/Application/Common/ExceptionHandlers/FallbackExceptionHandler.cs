namespace CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

public sealed class FallbackExceptionHandler<TRequest, TResponse> : MessageExceptionHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<FallbackExceptionHandler<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FallbackExceptionHandler{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public FallbackExceptionHandler(ILogger<FallbackExceptionHandler<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the exception and sets the failure result.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(TRequest request, Exception exception,
        CancellationToken cancellationToken)
    {
        TResponse failureResult;
        string[] errorMessages;

        // Handle specific exception types with custom error messages
        switch (exception)
        {
            case NotFoundException notFoundEx:
                errorMessages = new[] { notFoundEx.Message };
                _logger.LogWarning(notFoundEx, "Entity not found: {Message}", notFoundEx.Message);
                break;

            case ValidationException validationEx:
                var validationErrors = validationEx.Errors?
                    .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
                    .ToArray() ?? new[] { "Validation failed" };
                errorMessages = validationErrors.Any() ? validationErrors : new[] { "Validation failed with unknown errors" };
                _logger.LogWarning(validationEx, "Validation failed with {ErrorCount} errors", validationErrors.Length);
                break;

            default:
                errorMessages = new[] { $"An unexpected error occurred: {exception.Message}" };
                _logger.LogError(exception, "Unhandled exception occurred: {ExceptionType}", exception.GetType().Name);
                break;
        }

        failureResult = ResultFailureFactory.Create<TResponse>(errorMessages);

        return Handled(failureResult);
    }
}

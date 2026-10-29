namespace CleanArchitecture.Blazor.Application.Common.ExceptionHandlers;

/// <summary>
/// Handles NotFoundException and converts them into Result or Result&lt;T&gt; responses.
/// Provides user-friendly error messages for entity not found scenarios.
/// </summary>
public sealed class NotFoundExceptionHandler<TRequest, TResponse> : MessageExceptionHandler<TRequest, TResponse, NotFoundException>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<NotFoundExceptionHandler<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundExceptionHandler{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the NotFoundException and sets the failure result.
    /// </summary>
    /// <param name="request">The request that caused the exception.</param>
    /// <param name="exception">The NotFoundException to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(TRequest request, NotFoundException exception,
        CancellationToken cancellationToken)
    {
        var failureResult = CreateFailureResult(exception.Message);

        _logger.LogError(exception,
            "NotFoundException occurred for request {RequestType}: {ErrorMessage}",
            typeof(TRequest).Name,
            exception.Message);

        return Handled(failureResult);
    }

    /// <summary>
    /// Creates a failure result of the appropriate type.
    /// </summary>
    /// <param name="errorMessage">The error message to include in the result.</param>
    /// <returns>A failure result of type TResponse.</returns>
    private TResponse CreateFailureResult(string errorMessage)
    {
        return ResultFailureFactory.Create<TResponse>(errorMessage);
    }
}

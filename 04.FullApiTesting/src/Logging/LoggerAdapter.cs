namespace Users.Api.Logging;

/// <summary>
/// This Adapter/Wrapper class makes the ILogger<T> untestable extension methods testable
/// Hence, for unit testing, we can use this wrapper instead of ILogger<>.
/// </summary>
/// <typeparam name="TType"></typeparam>
public class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
    private readonly ILogger<TType> _logger;

    public LoggerAdapter(ILogger<TType> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string? message, params object?[] args)
    {
        _logger.LogInformation(message, args); // untestable code, testable through the Adapter
    }

    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        _logger.LogError(exception, message, args);
    }
}

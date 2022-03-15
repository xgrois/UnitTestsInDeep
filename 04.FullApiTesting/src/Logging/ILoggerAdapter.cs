namespace Users.Api.Logging;

/// <summary>
/// This interface is JUST for unit testing purposes.
/// ILogger<T> needs an Adapter to be unit testable, since e.g., LogInfo/LogError/... are extension methods
/// Methods below are just "copies" of the signatures of those methods
/// You can find them by inspecting the signature of LogInformation and LogError ILogger<> methods
/// E.g., the first LogInformation extension method signature is:
/// public static void LogInformation (this Microsoft.Extensions.Logging.ILogger logger, string? message, params object?[] args);
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface ILoggerAdapter<TType>
{
    void LogInformation(string? message, params object?[] args);

    void LogError(Exception? exception, string? message, params object?[] args);
}

using System.Diagnostics;
using System.Text;

namespace Dispatcher.API.Middleware;

public sealed class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    private const int SlowRequestThresholdMs = 400;

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request   = context.Request;

        string? requestBody = null;
        var contentType     = request.ContentType ?? string.Empty;
        var captureBody     = request.Method is "POST" or "PUT"
            && !contentType.StartsWith("multipart/")
            && !contentType.StartsWith("application/octet-stream");

        if (captureBody)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        Exception? caughtException = null;
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            caughtException = ex;
            throw;
        }
        finally
        {
            // No await here — avoids async state-machine interaction with the
            // in-flight exception from the catch block above.
            try
            {
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;

                var logMessage = new StringBuilder()
                    .AppendLine("HTTP Request/Response Log:")
                    .AppendLine($"  Path:     {request.Path}")
                    .AppendLine($"  Method:   {request.Method}")
                    .AppendLine($"  Status:   {context.Response.StatusCode}")
                    .AppendLine($"  Duration: {elapsed} ms");

                if (!string.IsNullOrWhiteSpace(requestBody))
                    logMessage.AppendLine($"  Request Body: {requestBody}");

                if (caughtException is not null)
                    logMessage.AppendLine($"  Exception: {caughtException.GetType().Name}: {caughtException.Message}");

                if (elapsed > SlowRequestThresholdMs)
                    logger.LogWarning("[SLOW REQUEST] {Path} took {Elapsed} ms", request.Path, elapsed);

                logger.LogInformation("{Log}", logMessage.ToString());
            }
            catch
            {
                // Swallow logging errors — never let finally throw and mask the real exception.
            }
        }
    }
}

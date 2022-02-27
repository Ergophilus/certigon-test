using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using ILogger = NLog.ILogger;

namespace Certigon.Test;

public static class RequestLoggingBuilderExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, ILogger logger)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.Use(async (context, next) =>
        {
            await LogRequest(context, logger);

            // Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream...
            using var responseBody = new MemoryStream();

            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            // Continue down the Middleware pipeline, eventually returning to this class
            await next.Invoke();

            // Format the response from the server
            await LogResponse(context, logger);

            // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        });

        return app;
    }

    private static async Task LogRequest(HttpContext context, ILogger logger)
    {
        try
        {
            var requestInfo = new StringBuilder();
            requestInfo.AppendLine("");
            requestInfo.AppendLine("Request");

            var queryString = context.Request.QueryString.ToString();
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                requestInfo.AppendLine($"QueryString: {queryString}");
            }

            // Allow the body stream to be read multiple times
            context.Request.EnableBuffering();

            // Read request body stream as text
            var requestBodyReader = new StreamReader(context.Request.Body);
            var requestBody = await requestBodyReader.ReadToEndAsync();

            // Position body stream at beginning to be readable further down the pipeline 
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                requestInfo.AppendLine($"Body: {requestBody}");
            }

            // Fetch the controller action descriptor
            var controllerActionDescriptor = context
                .GetEndpoint()?
                .Metadata
                .GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor != null)
            {
                requestInfo.AppendLine($"Method name: {controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}");
            }

            requestInfo.AppendLine($"URL: {context.Request.Path}");
            requestInfo.AppendLine($"HttpMethod: {context.Request.Method}");

            logger.Info(requestInfo);
        }
        catch (Exception e)
        {
            logger.Error(e, e.Message);
        }
    }

    private static async Task LogResponse(HttpContext context, ILogger logger)
    {
        try
        {
            var responseInfo = new StringBuilder();

            responseInfo.AppendLine("");
            responseInfo.AppendLine("Response");
            responseInfo.AppendLine($"StatusCode: {context.Response.StatusCode}");

            // Read response body stream as text
            var responseBodyReader = new StreamReader(context.Response.Body);
            var responseBody = await responseBodyReader.ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                responseInfo.AppendLine($"Body: {responseBody}");
            }

            // Position body stream at beginning
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            logger.Info(responseInfo);
        }
        catch (Exception e)
        {
            logger.Error(e, e.Message);
        }
    }
}
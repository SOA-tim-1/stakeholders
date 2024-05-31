namespace Explorer.API.Middleware
{
    public class GrpcLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GrpcLoggingMiddleware(RequestDelegate next, ILogger<GrpcLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType == "application/grpc")
            {
                _logger.LogInformation($"Incoming gRPC request: {context.Request.Path}");
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}

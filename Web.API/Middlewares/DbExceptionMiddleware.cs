namespace Application;

public class DbExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public DbExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 409;
            if (ex.HResult == -2146233088)
            {
                var response = new
                {
                    statusCode = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    ExceptionType = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    ex.InnerException?.Message,
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

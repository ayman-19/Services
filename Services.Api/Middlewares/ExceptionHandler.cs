namespace Services.Api.Middlewares
{
    public sealed class ExceptionHandler : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}

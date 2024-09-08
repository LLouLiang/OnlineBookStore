using System.Security.Claims;

namespace OnlineBookStore.WebApi.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext httpContext)
        {
            var appid = ((ClaimsIdentity)httpContext.User.Identity).Claims.Where(c => c.Type.EndsWith("appid")).Select(s => s.Value).FirstOrDefault();
            var email = ((ClaimsIdentity)httpContext.User.Identity).Claims.Where(c => c.Type.EndsWith("email")).Select(s => s.Value).FirstOrDefault();
            return _next(httpContext);
        }
    }
}

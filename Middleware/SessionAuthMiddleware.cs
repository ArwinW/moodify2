using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SessionAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SessionAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api"))
        {
            var path = context.Request.Path;
            var loggedIn = context.Session.GetString("UserName") != null;

            if (path.StartsWithSegments("/Login") || loggedIn)
            {
                await _next(context);
            }
            else
            {
                context.Response.Redirect("/Login/Index");
            }
        }
        else
        {
            await _next(context);
        }
    }
}
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
        var path = context.Request.Path;
        var loggedIn = context.Session.GetString("UserName") != null;
        var isAdmin = context.Session.GetString("UserRole") == "Admin";

        if (path.StartsWithSegments("/Login") || (loggedIn && isAdmin))
        {
            await _next(context);
        }
        else if (path.StartsWithSegments("/logs") && isAdmin)
        {
            await _next(context);
        }
        else if (!loggedIn)
        {
            context.Response.Redirect("/Login");
        }
        else
        {
            context.Response.Redirect("/Home"); // Redirect to the home page for non-admin users
        }
    }

}

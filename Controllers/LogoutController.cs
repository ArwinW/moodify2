using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class LogoutController : Controller
{
    public IActionResult Index()
    {
        // Perform any additional logout logic if needed

        // Clear user session or authentication cookies
        HttpContext.SignOutAsync();

        // Destroy the session
        HttpContext.Session.Clear();

        // Redirect to the Login page
        return Redirect("/Login");
    }
}

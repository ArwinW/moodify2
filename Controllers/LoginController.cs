using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using Moodify.Services;
using Moodify.Data;

namespace Moodify.Controllers
{
    public class LoginController : Controller
    {
        private readonly SecurityService securityService;

        public LoginController()
        {
            var userRepository = new UserRepository("your_connection_string");
            securityService = new SecurityService(userRepository);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessLogin(UserModel userModel)
        {
            if (securityService.IsValid(userModel))
            {
                return RedirectToAction("LoginSuccess");
            }
            else
            {
                return RedirectToAction("LoginFailure");
            }
        }

        public IActionResult LoginSuccess()
        {
            return View();
        }

        public IActionResult LoginFailure()
        {
            return View();
        }
    }
}

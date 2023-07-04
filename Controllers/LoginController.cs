using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using Moodify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel userModel) 
        {
            SecurityService securityService = new SecurityService();

            if (securityService.IsValid(userModel))
            {
                return View("LoginSucces", userModel);
            }else
            {
                return View("LoginFailure", userModel);
            }
            
        }
    }
}

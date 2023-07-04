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
        private readonly SecurityService _securityService;

        public LoginController()
        {
            _securityService = new SecurityService();
        }

        public IActionResult Index()
        {

            UserModel myModel = new UserModel();
            

            return View(myModel);
        }

        public IActionResult ProcessLogin(UserModel userModel)
        {

            if (_securityService.IsValid(userModel))
            {
                return View("LoginSucces", userModel);
            }
            else
            {
                return View("LoginFailure", userModel);
            }
        }
    }

}

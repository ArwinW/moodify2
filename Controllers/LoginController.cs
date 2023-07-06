using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moodify.db;
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
        private readonly DataAccess _database;

        public LoginController()
        {
            _securityService = new SecurityService();
            _database = new DataAccess();
        }

        public IActionResult Index()
        {

            UserModel myModel = new UserModel();
            

            return View(myModel);
        }

        [HttpPost]
        public IActionResult ProcessLogin(UserModel userModel)
        {
            if (_securityService.IsValid(userModel))
            {
                // Get the user from the repository
                var user = _database.GetUserByUsernameAndPassword(userModel.UserName, userModel.Password);

                if (user != null)
                {
                    // Start the session
                    HttpContext.Session.SetString("UserName", user.UserName);

                    if (user.IsAdmin == 1)
                    {
                        // Redirect to the admin page
                        return Redirect("/Logs");
                    }
                    else
                    {
                        // Redirect to the normal user page
                        return Redirect("Home");
                    }
                }
                else
                {
                    // User not found, redirect to login failure page
                    return RedirectToAction("LoginFailure");
                }
            }
            else
            {
                // Invalid credentials, redirect to login failure page
                return RedirectToAction("LoginFailure");
            }
        }

        public IActionResult Signup()
        {
            UserModel myModel = new UserModel();
            return View(myModel);
        }

        [HttpPost]
        public IActionResult ProcessSignup(UserModel userModel)
        {
            // Check if the username already exists in the database
            bool isUsernameTaken = _database.IsUsernameTaken(userModel.UserName);
            if (isUsernameTaken)
            {
                ModelState.AddModelError("UserName", "Username is already taken.");
                return View("Signup", userModel);
            }

            // Insert the user into the database
            _database.InsertUser(userModel);

            // Redirect to the login page or another appropriate page
            return RedirectToAction("Index");
        }
    }

}


using Microsoft.AspNetCore.Mvc;
using feedbackMgmt.Models;
using feedbackMgmt;

namespace feedbackMgmt.Controllers;

public class AuthController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(Credentials user)
    {
        // Verify the email and password

            if (DBManager.ValidateCredentials(user.Email,user.Password))
            {
                // Email and password match
                // Redirect to the protected page
                return View("Welcome");
            }
            else
            {
                // Email and password do not match
                // Show an error message
                ViewBag.ErrorMessage = "Invalid email or password.";
            return View("Error");
            }
        }

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(Credentials newUser)
    {
            if (DBManager.RegisterUser(newUser.Email, newUser.Password)) {
                return View("Welcome");
            }
             else {
                 return View("Error");
             }
        
        
    }

    public ActionResult Welcome()
    {
        return View();
    }
}


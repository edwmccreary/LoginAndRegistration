using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginAndRegistration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginAndRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger,MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser){
            if(ModelState.IsValid){
                if(_context.Users.Any(u => u.email == newUser.email))
                {
                    ModelState.AddModelError("email", "A User already exists for this email");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);

                _context.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("LoginPage");
            }
            else{
                return View("Index");
            }
        }

        [HttpGet("login_page")]
        public IActionResult LoginPage(){
            return View("Login");
        }

        [HttpPost("login")]
        public IActionResult Login(UserLogin newLogin){
            if(ModelState.IsValid){
                User userInDb = _context.Users.FirstOrDefault(u => u.email == newLogin.login_email);
                if(userInDb == null){
                    ModelState.AddModelError("login_email","The email or password you entered is incorrect");
                    return View("Login");
                }
                PasswordHasher<UserLogin> Hasher = new PasswordHasher<UserLogin>();
                PasswordVerificationResult result = Hasher.VerifyHashedPassword(newLogin, userInDb.password, newLogin.login_password);
                if(result == 0)
                {
                    ModelState.AddModelError("login_email","The email or password you entered is incorrect");
                    return View("Login");
                }
                if(HttpContext.Session.GetInt32("UserId")==null){
                    HttpContext.Session.SetInt32("UserId",userInDb.userId);
                }
                return RedirectToAction("Success");

            }
            else{
                return View("Login");
            }
        }

        [HttpGet("success")]
        public IActionResult Success(){
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                return View("Success");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

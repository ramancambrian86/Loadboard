using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportInventoryApp.Data;
using TransportInventoryApp.Models;

namespace TransportInventoryApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly TransportInventoryContext _context;

        public AccountController(TransportInventoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if(!_context.Users.Any(x => x.Username.Equals("test")))
            {
                User testUser = new User();
                testUser.Username = "test";
                testUser.Password = "123";

                _context.Users.Add(testUser);

                _context.SaveChanges();
            }

            return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username != null && password != null)
            {
                var loginUser = _context.Users.Find(username);
                if(loginUser != null && loginUser.Password.Equals(password))
                {
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ViewBag.error = "Wrong Credentials";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index");
        }
    }
}

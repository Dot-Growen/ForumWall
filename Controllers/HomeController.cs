using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheWall.Models;

namespace TheWall.Controllers
{
    public class HomeController : Controller
    {

        private MyContext _context;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }


        //************** GET REQUEST *************//

        [HttpGet ("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet ("loginpage")]
        public ViewResult LoginPage () {
            return View ();
        }

        [HttpGet ("wall")]
        public IActionResult Wall () {
            if (HttpContext.Session.GetInt32 ("UserId") == null) {
                return RedirectToAction ("loginpage");
            } else {
                User user = _context.Users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32 ("UserId"));
                return View (user);
            }
        }

        [HttpGet ("logout")]
        public IActionResult Logout () {
            Console.WriteLine ($"I WAS login. My Id => {HttpContext.Session.GetInt32 ("UserId")}");
            HttpContext.Session.Clear ();
            Console.WriteLine ($"NOW IM out. Id => {HttpContext.Session.GetInt32 ("UserId")}");
            return View ("loginpage");
        }

        [HttpGet("deletemsg")]
        public IActionResult Delete (int Id){
            if (HttpContext.Session.GetInt32 ("UserId") == null) {
                return RedirectToAction ("loginpage");
            } else {
                Message GetMsg = _context.Messages.SingleOrDefault(m => m.MessageId == Id);
                _context.Messages.Remove(GetMsg);
                _context.SaveChanges();
                return RedirectToAction ("wall");
            }
        }

        [HttpGet("deletecmt")]
        public IActionResult Comment (int Id){
            if (HttpContext.Session.GetInt32 ("UserId") == null) {
                return RedirectToAction ("loginpage");
            } else {
                Comment GetCmt = _context.Comments.SingleOrDefault(m => m.CommentId == Id);
                _context.Comments.Remove(GetCmt);
                _context.SaveChanges();
                return RedirectToAction ("wall");
            }
        }

        //*************** POST REQUEST ****************//

        [HttpPost ("login")]
        public IActionResult Login (LoginUser log) {
            if (ModelState.IsValid) {
                User userInDb = _context.Users.FirstOrDefault (u => u.Email == log.LoginEmail);
                Console.WriteLine (userInDb);
                if (userInDb == null) {
                    ModelState.AddModelError ("LoginUserName", "Invalid UserName/Password");
                    return View ("loginpage");
                } else {
                    var hasher = new PasswordHasher<LoginUser> ();
                    var result = hasher.VerifyHashedPassword (log, userInDb.Password, log.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginUserName", "Invalid UserName/Password");
                        return View ("loginpage");
                    } else {
                        HttpContext.Session.SetInt32 ("UserId", userInDb.UserId);
                        return RedirectToAction ("wall");
                    }
                }
            } else {
                Console.WriteLine (log.LoginEmail);
                return View ("loginpage");
            }
        }

        [HttpPost ("register")]
        public IActionResult Register (User user) {
            if (ModelState.IsValid) {
                if (_context.Users.Any (u => u.Email == user.Email)) {
                    ModelState.AddModelError ("UserName", "UserName already in use!");
                    return View ("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    user.Password = Hasher.HashPassword (user, user.Password);
                    _context.Users.Add (user);
                    _context.SaveChanges ();
                    HttpContext.Session.SetInt32 ("UserId", user.UserId);
                    Console.WriteLine ($"User id: {user.UserId}\nFirst Name: {user.FirstName}\nLastName: {user.LastName}\nUserName: {user.Email}\nSessionId: {HttpContext.Session.GetInt32("UserId")}");
                    return RedirectToAction ("wall");
                }
            } else {
                return View ("Index");
            }
        }

        // [HttpPost("addmessage")]


    }
}

using ProntoChef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProntoChef.Controllers
{
    public class AuthController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var loggedUser = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();

            if (loggedUser == null)
            {
                TempData["LoginError"] = true;
                return RedirectToAction("Login");
            }

            FormsAuthentication.SetAuthCookie(loggedUser.UserId.ToString(), true);
            return RedirectToAction("Index", "Home");
        }

        [Authorize, HttpPost]
        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
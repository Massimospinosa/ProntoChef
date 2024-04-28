using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProntoChef.Models;

namespace ProntoChef.Controllers
{
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (HttpContext.User.IsInRole("admin"))
            {               
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                if (HttpContext.User.Identity.Name != id.ToString())
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(user);
            }          

        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Email,Password,Name,Surname,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "user";
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,Password,Name,Surname,Phone,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" ,"Home");
            }
            return View(user);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            if (HttpContext.User.IsInRole("admin"))
            {
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                if (HttpContext.User.Identity.Name != id.ToString())
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(user);
            }
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                db.Users.Remove(user);
                db.SaveChanges();

                if (User.IsInRole("admin") || (User.IsInRole("user") && user.UserId == id))
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

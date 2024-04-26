using ProntoChef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProntoChef.Controllers
{
    public class HomeController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
                   
            return View(db.Products.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult AsyncCalls() 
        {
            return View();
        }


    }
}

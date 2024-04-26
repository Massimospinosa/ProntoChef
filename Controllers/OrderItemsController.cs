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
    public class OrderItemsController : Controller
    {
        private DBContext db = new DBContext();

       
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var orderItems = db.OrderItems.Include(o => o.OrderSummary).Include(o => o.Product);
            return View(orderItems.ToList());
        }

        
        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Quantity")] OrderItem orderItem, int id)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.Identity.Name;
                var summaryId = db.OrderSummaries.Where(o => o.UserId.ToString() == userId && o.State == "NON EVASO").FirstOrDefault();
                var product = db.Products.Where(p => p.ProductId == id).FirstOrDefault();

                OrderItem newItem = new OrderItem();
                newItem.OrderSummaryId = summaryId.OrderSummaryId;
                newItem.ProductId = id;
                newItem.Quantity = orderItem.Quantity;
                newItem.ItemPrice = product.ProductPrice * (decimal)orderItem.Quantity;

                db.OrderItems.Add(newItem);
                db.SaveChanges();

                TempData["ProductSuccess"] = true;

                return RedirectToAction("Details", "Products", new { id = id });             
            }

            return View(orderItem);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

       
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderItemId,OrderSummaryId,ProductId,UserId,Quantity,ItemPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
            return RedirectToAction("Index");
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

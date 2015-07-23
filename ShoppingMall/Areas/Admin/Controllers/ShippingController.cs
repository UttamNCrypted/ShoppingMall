using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingMall.Models;

namespace ShoppingMall.Areas.Admin.Controllers
{
    public class ShippingController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/Shipping/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                var shippings = db.Shippings.Include(s => s.Order);
                return View(shippings.ToList());
            }
        }

        //
        // GET: /Admin/Shipping/Details/5

        public ActionResult Details(Guid id)
        {
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        //
        // GET: /Admin/Shipping/Create

        public ActionResult Create()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
                return View();
            }
        }

        //
        // POST: /Admin/Shipping/Create

        [HttpPost]
        public ActionResult Create(Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                shipping.ShippingID = Guid.NewGuid();
                db.Shippings.Add(shipping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", shipping.OrderID);
            return View(shipping);
        }

        //
        // GET: /Admin/Shipping/Edit/5

        public ActionResult Edit(Guid id)
        {
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", shipping.OrderID);
            return View(shipping);
        }

        //
        // POST: /Admin/Shipping/Edit/5

        [HttpPost]
        public ActionResult Edit(Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", shipping.OrderID);
            return View(shipping);
        }

        //
        // GET: /Admin/Shipping/Delete/5

        public ActionResult Delete(Guid id)
        {
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        //
        // POST: /Admin/Shipping/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Shipping shipping = db.Shippings.Find(id);
            db.Shippings.Remove(shipping);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
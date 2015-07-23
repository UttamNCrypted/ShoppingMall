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
    public class OrderDetailController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/OrderDetail/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                var orderdetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
                return View(orderdetails.ToList());
            }
        }

        //
        // GET: /Admin/OrderDetail/Details/5

        public ActionResult Details(Guid id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        //
        // GET: /Admin/OrderDetail/Create

        public ActionResult Create()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
                ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
                return View();
            }
        }

        //
        // POST: /Admin/OrderDetail/Create

        [HttpPost]
        public ActionResult Create(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                orderdetail.OrderDetailID = Guid.NewGuid();
                db.OrderDetails.Add(orderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderdetail.ProductID);
            return View(orderdetail);
        }

        //
        // GET: /Admin/OrderDetail/Edit/5

        public ActionResult Edit(Guid id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderdetail.ProductID);
            return View(orderdetail);
        }

        //
        // POST: /Admin/OrderDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderdetail.ProductID);
            return View(orderdetail);
        }

        //
        // GET: /Admin/OrderDetail/Delete/5

        public ActionResult Delete(Guid id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        //
        // POST: /Admin/OrderDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderdetail);
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
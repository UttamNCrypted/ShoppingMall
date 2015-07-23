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
    public class OrderController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/Order/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            { 
                var orders = db.Orders.Include(o => o.User);
                return View(orders.ToList());
            }

        }

        //
        // GET: /Admin/Order/Details/5

        public ActionResult Details(Guid id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Admin/Order/Create

        public ActionResult Create()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                ViewBag.UsersID = new SelectList(db.Users, "UsersID", "FristName");
                return View();
            }
        }

        //
        // POST: /Admin/Order/Create

        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderID = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersID = new SelectList(db.Users, "UsersID", "FristName", order.UsersID);
            return View(order);
        }

        //
        // GET: /Admin/Order/Edit/5

        public ActionResult Edit(Guid id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersID = new SelectList(db.Users, "UsersID", "FristName", order.UsersID);
            return View(order);
        }

        //
        // POST: /Admin/Order/Edit/5

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersID = new SelectList(db.Users, "UsersID", "FristName", order.UsersID);
            return View(order);
        }

        //
        // GET: /Admin/Order/Delete/5

        public ActionResult Delete(Guid id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Admin/Order/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
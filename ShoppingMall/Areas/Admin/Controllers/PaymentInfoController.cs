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
    public class PaymentInfoController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/PaymentInfo/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                var paymentinfoes = db.PaymentInfoes.Include(p => p.Order);
                return View(paymentinfoes.ToList());
            }
        }

        //
        // GET: /Admin/PaymentInfo/Details/5

        public ActionResult Details(Guid id)
        {
            PaymentInfo paymentinfo = db.PaymentInfoes.Find(id);
            if (paymentinfo == null)
            {
                return HttpNotFound();
            }
            return View(paymentinfo);
        }

        //
        // GET: /Admin/PaymentInfo/Create

        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
            return View();
        }

        //
        // POST: /Admin/PaymentInfo/Create

        [HttpPost]
        public ActionResult Create(PaymentInfo paymentinfo)
        {
            if (ModelState.IsValid)
            {
                paymentinfo.PaymentInfoID = Guid.NewGuid();
                db.PaymentInfoes.Add(paymentinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", paymentinfo.OrderID);
            return View(paymentinfo);
        }

        //
        // GET: /Admin/PaymentInfo/Edit/5

        public ActionResult Edit(Guid id)
        {
            PaymentInfo paymentinfo = db.PaymentInfoes.Find(id);
            if (paymentinfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", paymentinfo.OrderID);
            return View(paymentinfo);
        }

        //
        // POST: /Admin/PaymentInfo/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentInfo paymentinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", paymentinfo.OrderID);
            return View(paymentinfo);
        }

        //
        // GET: /Admin/PaymentInfo/Delete/5

        public ActionResult Delete(Guid id)
        {
            PaymentInfo paymentinfo = db.PaymentInfoes.Find(id);
            if (paymentinfo == null)
            {
                return HttpNotFound();
            }
            return View(paymentinfo);
        }

        //
        // POST: /Admin/PaymentInfo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PaymentInfo paymentinfo = db.PaymentInfoes.Find(id);
            db.PaymentInfoes.Remove(paymentinfo);
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
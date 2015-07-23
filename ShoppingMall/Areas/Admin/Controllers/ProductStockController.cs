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
    public class ProductStockController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/ProductStock/

        public ActionResult Index()
        {
            var productstocks = db.ProductStocks.Include(p => p.Product);
            return View(productstocks.ToList());
        }

        //
        // GET: /Admin/ProductStock/Details/5

        public ActionResult Details(Guid id)
        {
            ProductStock productstock = db.ProductStocks.Find(id);
            if (productstock == null)
            {
                return HttpNotFound();
            }
            return View(productstock);
        }

        //
        // GET: /Admin/ProductStock/Create

        public ActionResult Create()
        {
            ViewBag.ProductsId = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        //
        // POST: /Admin/ProductStock/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductStock productstock)
        {
            if (ModelState.IsValid)
            {
                productstock.CreatedDate = DateTime.Now;
                productstock.ModifiedDate = DateTime.Now;
                productstock.ProductStockId = Guid.NewGuid();
                db.ProductStocks.Add(productstock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductsId = new SelectList(db.Products, "ProductID", "ProductName", productstock.ProductsId);
            return View(productstock);
        }

        //
        // GET: /Admin/ProductStock/Edit/5

        public ActionResult Edit(Guid id)
        {
            ProductStock productstock = db.ProductStocks.Find(id);
            if (productstock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductsId = new SelectList(db.Products, "ProductID", "ProductName", productstock.ProductsId);
            return View(productstock);
        }

        //
        // POST: /Admin/ProductStock/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductStock productstock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productstock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductsId = new SelectList(db.Products, "ProductID", "ProductName", productstock.ProductsId);
            return View(productstock);
        }

        //
        // GET: /Admin/ProductStock/Delete/5

        public ActionResult Delete(Guid id)
        {
            ProductStock productstock = db.ProductStocks.Find(id);
            if (productstock == null)
            {
                return HttpNotFound();
            }
            return View(productstock);
        }

        //
        // POST: /Admin/ProductStock/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductStock productstock = db.ProductStocks.Find(id);
            db.ProductStocks.Remove(productstock);
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
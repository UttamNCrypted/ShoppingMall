using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingMall.Models;
using System.IO;

namespace ShoppingMall.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/Products/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                var products = db.Products.Include(p => p.SubCategory);
                return View(products.ToList());
            }
        }

        //
        // GET: /Admin/Products/Details/5

        public ActionResult Details(Guid id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Admin/Products/Create

        public ActionResult Create()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                ViewBag.SubcategoyID = new SelectList(db.SubCategories, "SubCategoryID", "SubCategoryName");
                return View();
            }
        }

        //
        // POST: /Admin/Products/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            //if (ModelState.IsValid)
            //{
                HttpPostedFileBase file = Request.Files["ImageUpload"];

                product.ProductID = Guid.NewGuid();
                product.CreateDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;

                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
                    {
                        TempData["Errormsg"] = "Image File Extension is Not valid";
                        return View(product);
                    }
                    else
                    {
                        product.ProductImage = product.ProductID + fi.Extension;
                        file.SaveAs(Server.MapPath("~/Content/" + product.ProductID + fi.Extension));
                    }
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}

            //ViewBag.SubcategoyID = new SelectList(db.SubCategories, "SubCategoryID", "SubCategoryName", product.SubcategoyID);
            return View(product);
        }

        //
        // GET: /Admin/Products/Edit/5

        public ActionResult Edit(Guid id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubcategoyID = new SelectList(db.SubCategories, "SubCategoryID", "SubCategoryName", product.SubcategoyID);
            return View(product);
        }

        //
        // POST: /Admin/Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubcategoyID = new SelectList(db.SubCategories, "SubCategoryID", "SubCategoryName", product.SubcategoyID);
            return View(product);
        }

        //
        // GET: /Admin/Products/Delete/5

        public ActionResult Delete(Guid id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Admin/Products/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
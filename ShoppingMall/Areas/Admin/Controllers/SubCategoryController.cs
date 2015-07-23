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
    public class SubCategoryController : Controller
    {
        private ShoppingCartDatabaseContexts db = new ShoppingCartDatabaseContexts();

        //
        // GET: /Admin/SubCategory/

        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                var subcategories = db.SubCategories.Include(s => s.Category);
                return View(subcategories.ToList());
            }
        }

        //
        // GET: /Admin/SubCategory/Details/5

        public ActionResult Details(Guid id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        //
        // GET: /Admin/SubCategory/Create

        public ActionResult Create()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login", "AdminHome");
            else
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                return View();
            }
        }

        //
        // POST: /Admin/SubCategory/Create

        [HttpPost]
        public ActionResult Create(SubCategory subcategory)
        {
            subcategory.CreateDate = DateTime.Now;
            subcategory.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                subcategory.SubCategoryID = Guid.NewGuid();
                db.SubCategories.Add(subcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subcategory.CategoryID);
            return View(subcategory);
        }

        //
        // GET: /Admin/SubCategory/Edit/5

        public ActionResult Edit(Guid id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subcategory.CategoryID);
            return View(subcategory);
        }

        //
        // POST: /Admin/SubCategory/Edit/5

        [HttpPost]
        public ActionResult Edit(SubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subcategory.CategoryID);
            return View(subcategory);
        }

        //
        // GET: /Admin/SubCategory/Delete/5

        public ActionResult Delete(Guid id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        //
        // POST: /Admin/SubCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SubCategory subcategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subcategory);
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
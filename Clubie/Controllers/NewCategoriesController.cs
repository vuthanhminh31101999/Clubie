using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clubie.Models;

namespace Clubie.Controllers
{
    public class NewCategoriesController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();

        // GET: NewCategories
        public ActionResult Index()
        {
            return View(db.NewCategories.ToList());
        }

        // GET: NewCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewCategory newCategory = db.NewCategories.Find(id);
            if (newCategory == null)
            {
                return HttpNotFound();
            }
            return View(newCategory);
        }

        // GET: NewCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewCategoryId,NewCategoryName")] NewCategory newCategory)
        {
            if (ModelState.IsValid)
            {
                db.NewCategories.Add(newCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newCategory);
        }

        // GET: NewCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewCategory newCategory = db.NewCategories.Find(id);
            if (newCategory == null)
            {
                return HttpNotFound();
            }
            return View(newCategory);
        }

        // POST: NewCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewCategoryId,NewCategoryName")] NewCategory newCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newCategory);
        }

        // GET: NewCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewCategory newCategory = db.NewCategories.Find(id);
            if (newCategory == null)
            {
                return HttpNotFound();
            }
            return View(newCategory);
        }

        // POST: NewCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewCategory newCategory = db.NewCategories.Find(id);
            db.NewCategories.Remove(newCategory);
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

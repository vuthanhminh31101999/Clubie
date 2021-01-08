using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clubie.Models;

namespace Clubie.Controllers
{
    public class ProductsController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();

        //Add to order
        public ActionResult AddToOrder(int id)
        {
            if (Convert.ToBoolean(Session["Login"]) != true)
            {
                return RedirectToAction("Login", "Users");
            }
            bool exist = false;
            foreach (var i in db.OrderDetails.Include(o => o.Order))
            {
                if (i.OrderId == Convert.ToInt32(Session["OrderId"]))
                if (i.ProductId == id)
                {
                    i.Amount++;
                    exist = true;
                    break;
                }
            }
            if (exist != true)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderDetailId = 0;
                foreach (var i in db.OrderDetails)
                {
                    if (orderDetail.OrderDetailId == i.OrderDetailId)
                        orderDetail.OrderDetailId = orderDetail.OrderDetailId + 1;
                    else
                        break;
                }
                orderDetail.OrderId = Convert.ToInt32(Session["OrderId"]);
                orderDetail.ProductId = id;
                orderDetail.Amount = 1;
                orderDetail.ProductPrice = db.Products.Find(id).Price;
                orderDetail.ProductPromotionPrice = db.Products.Find(id).PromotionPrice;
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();
            return RedirectToAction("Search");
        }

        //Search
        public ActionResult Search(string searchString)
        {
            if (searchString == null)
            {
                searchString = "";
            }
            var products = db.Products.Where(p => p.ProductName.Contains(searchString)).Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "ProductCategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductCategoryId,ProductCode,ProductName,MetaTitle,Color,Size,Description,Price,PromotionPrice,View,Classification,Status")] Product product, string[] files)
        {
            if (ModelState.IsValid)
            {
                foreach (var i in db.Products)
                {
                    if (product.ProductId == i.ProductId)
                        product.ProductId = product.ProductId + 1;
                    else
                        break;
                }
                db.Products.Add(product);
                db.SaveChanges();
                foreach (string file in files)
                {
                    ProductImage productImage = new ProductImage();
                    if (file != null)
                    {
                        foreach (var i in db.ProductImages)
                        {
                            if (productImage.ImageId == i.ImageId)
                                productImage.ImageId = productImage.ImageId + 1;
                            else
                                break;
                        }
                        productImage.ImageName = file;
                        productImage.ProductId = product.ProductId;
                        db.ProductImages.Add(productImage);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductCategoryId,ProductCode,ProductName,MetaTitle,Color,Size,Description,Price,PromotionPrice,View,Classification,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

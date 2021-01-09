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
    public class HomeController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();

        [ChildActionOnly]
        public ActionResult Banner()
        {
            return PartialView("Banner");
        }

        public ActionResult Index(string searchString)
        {
            if (searchString == null)
            {
                searchString = "";
            }
            var products = db.Products.Where(p => p.ProductName.Contains(searchString) || p.ProductCategory.ProductCategoryName == searchString).Where(p => p.Status == true).Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
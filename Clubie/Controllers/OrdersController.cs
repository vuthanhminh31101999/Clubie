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
    public class OrdersController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();
        // GET: Orders
        public ActionResult Index()
        {
            var order = db.Orders.Include(o => o.User);
            return View(order.ToList());
        }

        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
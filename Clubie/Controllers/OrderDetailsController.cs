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
    public class OrderDetailsController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();
        // GET: OrderDetails
        
        public ActionResult Index()
        {
            int orderId = Convert.ToInt32(Session["OrderId"]);
            var orderDetails = db.OrderDetails.Include(o => o.Product).Include(o => o.Order).Where(o => o.OrderId == orderId);
            return View(orderDetails.ToList());
        }
        
        //Delete Order Detail
        public ActionResult Delete(int id)
        {
            OrderDetail orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
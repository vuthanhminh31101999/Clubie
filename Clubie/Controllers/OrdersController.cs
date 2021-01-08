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

        public ActionResult DeleteCart()
        {
            var orders = db.Orders.Include(o => o.User);
            Order order = new Order();
            foreach (var i in orders)
            {
                if (i.DeliveryStatus == "None" && i.UserId == Convert.ToInt32(Session["UserId"]))
                {
                    order = i;
                    break;
                }
            }
            List<int> listRemove = new List<int>();
            var orderDetail = db.OrderDetails.Where(o => o.OrderId == order.OrderId).Include(o => o.Order).Include(o => o.Product);
            foreach (var i in orderDetail)
            {
                listRemove.Add(i.OrderDetailId);
            }
            for (int i = 0; i < listRemove.Count(); i++)
            {
                db.OrderDetails.Remove(db.OrderDetails.Find(listRemove[i]));
                db.SaveChanges();
            }
            return RedirectToAction("InOrder", "OrderDetails");
        }

        //Proceed to Checkout
        public ActionResult ProceedToCheckout()
        {
            return View();
        }

        //Add order
        public ActionResult AddOrder(string city, string province, string streetAddress, string phoneNumber)
        {
            Order order = db.Orders.Find(Convert.ToInt32(Session["OrderId"]));
            order.DeliveryAddress = streetAddress + ", " + province + ", " + city;
            order.DeliveryPhoneNumber = phoneNumber;
            order.OrderDate = DateTime.Today;
            order.DeliveryStatus = "Waiting for progessing";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            NewOrder();
            return RedirectToAction("Search", "Products");
        }

        public void NewOrder()
        {
            Order order = new Order();
            order.OrderId = 0;
            foreach (var k in db.Orders)
            {
                if (order.OrderId == k.OrderId)
                    order.OrderId = order.OrderId + 1;
                else
                    break;
            }
            Session["OrderId"] = order.OrderId;
            order.UserId = Convert.ToInt32(Session["UserId"]);
            order.DeliveryStatus = "None";
            db.Orders.Add(order);
            db.SaveChanges();
        }
    }
}
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
    public class UsersController : Controller
    {
        private ClothingStoreEntities db = new ClothingStoreEntities();

        //Log out
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        public bool FindExistOrder(int roleId)
        {
            var orders = db.Orders.Include(o => o.User).Where(o => o.UserId == roleId);
            foreach (var i in orders)
            {
                if (i.DeliveryStatus == "None")
                {
                    Session["OrderId"] = i.OrderId;
                    return true;
                }
            }
            return false;
        }

        public ActionResult DoLogin(string username, string password)
        {
            foreach (var i in db.Users)
            {
                if (i.Username == username && i.Password == password)
                {
                    Session["Login"] = true;
                    Session["UserId"] = i.UserId;
                    Session["Username"] = i.Username;
                    Session["RoleId"] = i.RoleId;
                }
            }
            if (!FindExistOrder(Convert.ToInt32(Session["RoleId"])))
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

            if (db.Roles.Find(Convert.ToInt32(Session["RoleId"])).RoleName == "Admin")
            {
                return RedirectToAction("Index", "Users");
            }
            else
                return RedirectToAction("Search", "Products");
        }

        //Register
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult DoRegister(string username, string password, string rePassword)
        {
            User user = new User();
            user.UserId = 0;
            foreach (var i in db.Users)
            {
                if (user.UserId == i.UserId)
                    user.UserId = user.UserId + 1;
                else
                    break;
            }
            user.Role = db.Roles.Where(n => n.RoleName == "Customer").FirstOrDefault();
            user.Username = username;
            user.Password = password;
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,Name,BirthDay,Address,Email,PhoneNumber,Points,RoleId,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                foreach (var i in db.Users)
                {
                    if (user.UserId == i.UserId)
                        user.UserId = user.UserId + 1;
                    else
                        break;
                }
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Name,BirthDay,Address,Email,PhoneNumber,Points,RoleId,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

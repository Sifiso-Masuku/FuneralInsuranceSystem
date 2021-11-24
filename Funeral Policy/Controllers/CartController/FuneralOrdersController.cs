using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Funeral_Policy.Models.CartModels;
using Funeral_Policy.Services.CartServices;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace Funeral_Policy.Controllers.CartController
{
    public class FuneralOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Cart_Service cart_Service = new Cart_Service();

        // GET: FuneralOrders
        public ActionResult Index()
        {
            var UserName = User.Identity.GetUserName();
            return View(db.FuneralOrders.ToList().Where(x => x.UserEmail == UserName && x.OrderStatus != "Checked Out"));

        }
        public ActionResult COnfirmOrder()
        {
            var UserName = User.Identity.GetUserName();
            ViewBag.Total = cart_Service.GetCartTotal(cart_Service.GetCartID());
            ViewBag.TotalQTY = cart_Service.GetCartItems().FindAll(x => x.cart_id == cart_Service.GetCartID()).Sum(q => q.quantity);
            var confirm = db.FuneralOrders.ToList();
            var cart = db.Cart_Items.ToList();

            TombstoneOrder tombstoneOrder = new TombstoneOrder();
            tombstoneOrder.OrderNumber = tombstoneOrder.GenVoucher();
            tombstoneOrder.Total = ViewBag.Total;
            tombstoneOrder.UserOrder = UserName;
            tombstoneOrder.Status = "Paid";
            tombstoneOrder.OrderDate = DateTime.Now.Date.ToLongDateString();
            db.TombstoneOrders.Add(tombstoneOrder);
            db.SaveChanges();

            FuneralOrder funeralOrder = new FuneralOrder();

            foreach (var item in confirm)
            {
                foreach (var i in cart)
                {
                    if (UserName == item.UserEmail && item.cart_id == i.cart_id)
                    {
                        var statusUpdate = db.FuneralOrders.Find(item.cart_item_id);
                        statusUpdate.OrderStatus = "Checked Out";
                        statusUpdate.OrderId = tombstoneOrder.OrderId;
                        db.Entry(statusUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                        cart_Service.EmptyCart();


                    }
                }

            }

            return RedirectToAction("OnceOff", new { tot = tombstoneOrder.Total });
        }
        public ActionResult Error()
        {
            return View();
        }


    }
}
//        // GET: FuneralOrders/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            FuneralOrder funeralOrder = db.FuneralOrders.Find(id);
//            if (funeralOrder == null)
//            {
//                return HttpNotFound();
//            }
//            return View(funeralOrder);
//        }

//        // GET: FuneralOrders/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: FuneralOrders/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "cart_item_id,cart_id,item_id,quantity,price,ItemName,UserEmail,Picture,Total,OrderStatus,OrderDate,DayOfWik,OrderId")] FuneralOrder funeralOrder)
//        {
//            if (ModelState.IsValid)
//            {
//                db.FuneralOrders.Add(funeralOrder);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(funeralOrder);
//        }

//        // GET: FuneralOrders/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            FuneralOrder funeralOrder = db.FuneralOrders.Find(id);
//            if (funeralOrder == null)
//            {
//                return HttpNotFound();
//            }
//            return View(funeralOrder);
//        }

//        // POST: FuneralOrders/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "cart_item_id,cart_id,item_id,quantity,price,ItemName,UserEmail,Picture,Total,OrderStatus,OrderDate,DayOfWik,OrderId")] FuneralOrder funeralOrder)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(funeralOrder).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(funeralOrder);
//        }

//        // GET: FuneralOrders/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            FuneralOrder funeralOrder = db.FuneralOrders.Find(id);
//            if (funeralOrder == null)
//            {
//                return HttpNotFound();
//            }
//            return View(funeralOrder);
//        }

//        // POST: FuneralOrders/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            FuneralOrder funeralOrder = db.FuneralOrders.Find(id);
//            db.FuneralOrders.Remove(funeralOrder);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}

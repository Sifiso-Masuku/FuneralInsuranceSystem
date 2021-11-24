using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using PagedList;
using System.Web;
using System.Web.Mvc;
using Funeral_Policy.Models;
using Funeral_Policy.ViewModel;
using IdentitySample.Models;
using Rotativa;

namespace Funeral_Policy.Controllers
{
    public class FuneralBookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FuneralBookings
        public ActionResult Index()
        {
            var funeralBookings = db.FuneralBookings.Include(f => f.FuneralType);
            var kl = db.FuneralBookings.ToList();
            if (User.IsInRole("Member"))
            {
                kl = kl.Where(o => o.creator == User.Identity.Name).ToList();
            }
            return View(kl);
            // return View(funeralBookings.ToList());
        }
        public ActionResult Index2(string option, string search, int? pageNumber, string sort)
        {
            //if a user choose the radio button option as Subject  
            if (option == "address")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.FuneralBookings.Where(x => x.address.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else if (option == "Status")
            {
                return View(db.FuneralBookings.Where(x => x.Status.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else
            {
                return View(db.FuneralBookings.Where(x => x.creator.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }
            //var kl = db.FuneralBookings.ToList();
            //return View(kl);
        }
        public ActionResult PrintInvoice(int id, string st)
        {
            var report = new ActionAsPdf("CreateInvoice", new { id = id, st = st });
            return report;
        }

        // GET: FuneralBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralBooking funeralBooking = db.FuneralBookings.Find(id);
            if (funeralBooking == null)
            {
                return HttpNotFound();
            }
            return View(funeralBooking);
        }

        // GET: FuneralBookings/Create
        public ActionResult Create(int id,decimal c)
        {
            ViewBag.Id = id;
            ViewBag.funeralTypeId = new SelectList(db.FuneralTypes, "funeralTypeId", "funeralTypeName");
            FuneralBooking funeralBooking = new FuneralBooking();
            funeralBooking.coffinId = id;
            funeralBooking.TotalCost = c;

            return View(funeralBooking);
        }

        // POST: FuneralBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "funeralBookingId,FuneralDate,Time,funeralTypeId,address")]*/ FuneralBooking funeralBooking)
        {
            var d = db.FuneralBookings.ToList().Where(p => p.FuneralDate== funeralBooking.FuneralDate).Count();
            var dt = db.FuneralBookings.ToList().Where(p => p.Time == funeralBooking.Time).Count();
            ViewBag.coffinId =funeralBooking.coffinId;
            var coffin = db.Coffins.ToList().Where(p => p.coffinId == ViewBag.coffinId).Select(x => x.coffinType).FirstOrDefault();
            ViewBag.funeralTypeId = new SelectList(db.FuneralTypes, "funeralTypeId", "funeralTypeName", funeralBooking.funeralTypeId);
            
            //if (d != 0 && dt != 0)
            //{
            //    TempData["AlertMessage"] = "Please book for other dates, Please book for a date later than - " + dta;
            //}

            if (ModelState.IsValid)
            {
                funeralBooking.CoffinPrice = Convert.ToDouble(funeralBooking.GetCoffinPrice());
                funeralBooking.FuneralPrice = Convert.ToDouble(funeralBooking.GetFuneralPrice());
                funeralBooking.CoffinName = funeralBooking.GetCoffinName();
                funeralBooking.FuneralName = funeralBooking.GetFuneralName();
                funeralBooking.TotalCost = Convert.ToDecimal(funeralBooking.CalcTotalCost());
                funeralBooking.creator = User.Identity.Name;
                funeralBooking.DateCreated = DateTime.Now;
                
                db.FuneralBookings.Add(funeralBooking);
                db.SaveChanges();
                return RedirectToAction("Index", "Shopping");
            }

           
            return View(funeralBooking);
        }
        public ActionResult ConfirmBooking(int id)
        {
            var d = db.FuneralBookings.Where(p => p.funeralBookingId == id).FirstOrDefault();
            if (d.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Cannot assign staff to a booking that has been rejected";
                return RedirectToAction("Index2");

            }
            else if (d.Status == "Approved")
            {
                TempData["AlertMessage"] = "Cannot assign a booking with staff twice";
                return RedirectToAction("Index2");

            }
            else
            {
                return RedirectToAction("Create", "AssignStaffs", new { id = id });

            }
        }
            // GET: FuneralBookings/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralBooking funeralBooking = db.FuneralBookings.Find(id);
            if (funeralBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.funeralTypeId = new SelectList(db.FuneralTypes, "funeralTypeId", "funeralTypeName", funeralBooking.funeralTypeId);
            return View(funeralBooking);
        }

        // POST: FuneralBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "funeralBookingId,FuneralDate,Time,funeralTypeId,address")] FuneralBooking funeralBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funeralBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.funeralTypeId = new SelectList(db.FuneralTypes, "funeralTypeId", "funeralTypeName", funeralBooking.funeralTypeId);
            return View(funeralBooking);
        }
        public ActionResult Approve(int Id)
        {
            var order = db.FuneralBookings.Where(ui => ui.funeralBookingId == Id).FirstOrDefault();
            if (order.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Cannot Approve an order that has been Rejected Already";
                return RedirectToAction("Index2");

            }
            else
            if (order.Status == "Approved")
            {
                TempData["AlertMessage"] = "Order Already Approved";
                return RedirectToAction("Index2");
            }
            else
            {
                order.Status = "Approved";
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "AssignStaffs", new { id = order.funeralBookingId });
            }
        }
        public ActionResult Reject(int Id)
        {
            var order = db.FuneralBookings.Where(ui => ui.funeralBookingId == Id).FirstOrDefault();
            if (order.Status == "Approved")
            {
                TempData["AlertMessage"] = "Cannot reject an order that has been Approved Already";
                return RedirectToAction("Index2");

            }
            else
            if (order.Status == "Rejected")
            {
                TempData["AlertMessage"] = "Order Already Rejected";
                return RedirectToAction("Index2");
            }
            else
            {
                order.Status = "Rejected";
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }

        }
        public FuneralBooking ordersd(int? id)
        {
            var df = db.FuneralBookings.Where(k => k.funeralBookingId == id).FirstOrDefault();
            return df;
        }

        // GET: FuneralBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralBooking funeralBooking = db.FuneralBookings.Find(id);
            if (funeralBooking == null)
            {
                return HttpNotFound();
            }
            return View(funeralBooking);
        }
        public ActionResult CreateInvoice(int id, string st)
        {
            var allConfirmedOrders = db.FuneralOrders.Where(o => o.OrderId == id).ToList();
            CustomerOrderProduct allRecords = new CustomerOrderProduct();
            List<OrderProduct> allRe = new List<OrderProduct>();

            foreach (var item in allConfirmedOrders)
            {
                OrderProduct kl = new OrderProduct()
                {
                    ProductID = item.item_id,
                    ProductName = item.ItemName,
                    UnitPrice = item.price,
                    TotalPrice = (item.price * item.quantity),
                    dty = item.quantity

                    ///OrderStatus = item.OrderStatus,

                };
                allRe.Add(kl);
                CustomerOrderProduct obj2 = new CustomerOrderProduct()
                {
                    CustomerEmail = item.UserEmail,
                    OrderID = (int)item.OrderId,
                    OrderStatus = st,
                    funeralname = ordersd(item.OrderId).FuneralName,
                    funeralprice = ordersd(item.OrderId).FuneralPrice,
                    FuneralType = ordersd(item.OrderId).funeralTypeId,
                    Coffin = ordersd(item.OrderId).CoffinName,
                    CoffinPrice = (double)ordersd(item.OrderId).GetCoffinPrice(),
                    funeralDate = ordersd(item.OrderId).FuneralDate,
                    Time = (DateTime)ordersd(item.OrderId).Time,
                    TotalAmount = ordersd(item.OrderId).TotalCost,
                    //CustomerName = db.Users.Where(o => o.Email == item.UserEmail).Select(p => p.UserName).FirstOrDefault()
                    CustomerName=db.MemberApplications.Where(o=>o.Email==item.UserEmail).Select(p=>p.Name ).FirstOrDefault()
                };
                allRecords = obj2;
            }

            allRecords.orderProducts = allRe;

            ViewBag.invoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            return View(allRecords);
        }



        // POST: FuneralBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuneralBooking funeralBooking = db.FuneralBookings.Find(id);
            db.FuneralBookings.Remove(funeralBooking);
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

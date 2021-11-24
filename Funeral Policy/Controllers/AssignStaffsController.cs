using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Funeral_Policy.Models;
using IdentitySample.Models;

namespace Funeral_Policy.Controllers
{
    public class AssignStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignStaffs
        public ActionResult Index()
        {
            var assignStaffs = db.AssignStaffs.Include(a => a.Staff);
            return View(assignStaffs.ToList());
        }
        public ActionResult Index2()
        {
            var assignStaffs = db.AssignStaffs.Include(a => a.Staff);
            return View(assignStaffs.ToList());
        }

        // GET: AssignStaffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignStaff assignStaff = db.AssignStaffs.Find(id);
            if (assignStaff == null)
            {
                return HttpNotFound();
            }
            return View(assignStaff);
        }

        // GET: AssignStaffs/Create
        public ActionResult Create(int id)
        {
            ViewBag.Id = id;
            AssignStaff assignStaff = new AssignStaff();
            assignStaff.funeralBookingId = id;
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name");
            return View(assignStaff);
        }

        // POST: AssignStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignId,StaffId,funeralBookingId")] AssignStaff assignStaff)
        {
            if (ModelState.IsValid)
            {
                var d = db.FuneralBookings.Where(p => p.funeralBookingId == assignStaff.funeralBookingId).FirstOrDefault();
                d.Status = "Approved";
                assignStaff.Member = assignStaff.user();
                assignStaff.FuneralType = assignStaff.funeralTypes();
                assignStaff.Coffin = assignStaff.Coffins();
                assignStaff.FuneraralDate = assignStaff.Date();
                assignStaff.adress = assignStaff.FuneralAddress();
                db.AssignStaffs.Add(assignStaff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", assignStaff.StaffId);
            return View(assignStaff);
        }

        // GET: AssignStaffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignStaff assignStaff = db.AssignStaffs.Find(id);
            if (assignStaff == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", assignStaff.StaffId);
            return View(assignStaff);
        }

        // POST: AssignStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignId,StaffId,funeralBookingId")] AssignStaff assignStaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignStaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffId = new SelectList(db.Staffs, "StaffId", "Name", assignStaff.StaffId);
            return View(assignStaff);
        }

        // GET: AssignStaffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignStaff assignStaff = db.AssignStaffs.Find(id);
            if (assignStaff == null)
            {
                return HttpNotFound();
            }
            return View(assignStaff);
        }

        // POST: AssignStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignStaff assignStaff = db.AssignStaffs.Find(id);
            db.AssignStaffs.Remove(assignStaff);
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

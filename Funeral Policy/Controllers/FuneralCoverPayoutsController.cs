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
    public class FuneralCoverPayoutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FuneralCoverPayouts
        public ActionResult Index()
        {
            return View(db.funeralCoverPayouts.ToList());
        }
        
        // GET: FuneralCoverPayouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralCoverPayout funeralCoverPayout = db.funeralCoverPayouts.Find(id);
            if (funeralCoverPayout == null)
            {
                return HttpNotFound();
            }
            return View(funeralCoverPayout);
        }

        // GET: FuneralCoverPayouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuneralCoverPayouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "funeralCoverPayoutId,PayoutAmount")] FuneralCoverPayout funeralCoverPayout)
        {
            if (ModelState.IsValid)
            {
                db.funeralCoverPayouts.Add(funeralCoverPayout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(funeralCoverPayout);
        }

        // GET: FuneralCoverPayouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralCoverPayout funeralCoverPayout = db.funeralCoverPayouts.Find(id);
            if (funeralCoverPayout == null)
            {
                return HttpNotFound();
            }
            return View(funeralCoverPayout);
        }

        // POST: FuneralCoverPayouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "funeralCoverPayoutId,PayoutAmount")] FuneralCoverPayout funeralCoverPayout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funeralCoverPayout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(funeralCoverPayout);
        }

        // GET: FuneralCoverPayouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralCoverPayout funeralCoverPayout = db.funeralCoverPayouts.Find(id);
            if (funeralCoverPayout == null)
            {
                return HttpNotFound();
            }
            return View(funeralCoverPayout);
        }

        // POST: FuneralCoverPayouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuneralCoverPayout funeralCoverPayout = db.funeralCoverPayouts.Find(id);
            db.funeralCoverPayouts.Remove(funeralCoverPayout);
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

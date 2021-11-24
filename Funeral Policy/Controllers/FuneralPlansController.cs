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
    public class FuneralPlansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
        
        // GET: FuneralPlans
        public ActionResult Index()
        {
            return View(db.funeralPlans.ToList());
        }
        public ActionResult FuneralPlanView()
        {
            return View(db.funeralPlans.ToList());
        }


        // GET: FuneralPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralPlan funeralPlan = db.funeralPlans.Find(id);
            if (funeralPlan == null)
            {
                return HttpNotFound();
            }
            return View(funeralPlan);
        }

        // GET: FuneralPlans/Create
        public ActionResult Create()
        {
            ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount");
            return View();
        }

        // POST: FuneralPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FuneralPlanId,FuneralPlanName,funeralCoverPayoutId,FuneralPlanDescription")] FuneralPlan funeralPlan)
        {
            if (ModelState.IsValid)
            {
                db.funeralPlans.Add(funeralPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", funeralPlan.funeralCoverPayoutId);
            return View(funeralPlan);
        }

        // GET: FuneralPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralPlan funeralPlan = db.funeralPlans.Find(id);
            if (funeralPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", funeralPlan.funeralCoverPayoutId);
            return View(funeralPlan);
        }

        // POST: FuneralPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FuneralPlanId,FuneralPlanName,funeralCoverPayoutId,FuneralPlanDescription")] FuneralPlan funeralPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funeralPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", funeralPlan.funeralCoverPayoutId);
            return View(funeralPlan);
        }

        // GET: FuneralPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralPlan funeralPlan = db.funeralPlans.Find(id);
            if (funeralPlan == null)
            {
                return HttpNotFound();
            }
            return View(funeralPlan);
        }

        // POST: FuneralPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuneralPlan funeralPlan = db.funeralPlans.Find(id);
            db.funeralPlans.Remove(funeralPlan);
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

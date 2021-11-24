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
    public class FuneralTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FuneralTypes
        public ActionResult Index()
        {
            return View(db.FuneralTypes.ToList());
        }
        public ActionResult FuneralTpeView()
        {
            return View(db.FuneralTypes.ToList());
        }



        // GET: FuneralTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralType funeralType = db.FuneralTypes.Find(id);
            if (funeralType == null)
            {
                return HttpNotFound();
            }
            return View(funeralType);
        }

        // GET: FuneralTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FuneralTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "funeralTypeId,funeralTypeName,funeralCost,description")] FuneralType funeralType)
        {
            if (ModelState.IsValid)
            {
                db.FuneralTypes.Add(funeralType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(funeralType);
        }

        // GET: FuneralTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralType funeralType = db.FuneralTypes.Find(id);
            if (funeralType == null)
            {
                return HttpNotFound();
            }
            return View(funeralType);
        }

        // POST: FuneralTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "funeralTypeId,funeralTypeName,funeralCost,description")] FuneralType funeralType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funeralType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(funeralType);
        }

        // GET: FuneralTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FuneralType funeralType = db.FuneralTypes.Find(id);
            if (funeralType == null)
            {
                return HttpNotFound();
            }
            return View(funeralType);
        }

        // POST: FuneralTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FuneralType funeralType = db.FuneralTypes.Find(id);
            db.FuneralTypes.Remove(funeralType);
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

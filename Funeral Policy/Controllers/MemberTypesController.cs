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
    public class MemberTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MemberTypes
        public ActionResult Index()
        {
            return View(db.MemberTypes.ToList());
        }
        public ActionResult MemberTypeView()
        {
            return View(db.MemberTypes.ToList());
        }
        [HttpGet]
        public JsonResult List()
        {
            return Json(db.FamilyMembers.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: MemberTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.membertypeid = id;
            MemberType memberType = db.MemberTypes.Find(id);
            if (memberType == null)
            {
                return HttpNotFound();
            }
            return View(memberType);
        }
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember addMembr = new FamilyMember();
            addMembr.familyMemberId = (int)id;

            return View(addMembr);
        }
        [HttpPost]
        public ActionResult Add([Bind(Include = "familyMemberId,Name,Surname,Gender,DateOfBirth")] FamilyMember addMembr)
        {
            var memberApp = db.MemberTypes.Where(m => m.memberTypeId == addMembr.familyMemberId).FirstOrDefault();
            try
            {


                memberApp.Name = memberApp.Name;
                memberApp.Surname = memberApp.Surname;
                memberApp.Gender = memberApp.Gender;
                memberApp.DateOfBirth = memberApp.DateOfBirth;
                db.Entry(memberApp).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ModelState.AddModelError("" + e.ToString(), "Unable to save changes. " +
                 "Try again, and if the problem persists see your system administrator.");


                return View(addMembr);
            }
        }

        [HttpPost]
        public JsonResult Add(int memberid, int membertypeid)
        {
            AdditionalMemberView mt = new AdditionalMemberView();
            mt.MemberTypeID = membertypeid;
            mt.MemberID = memberid;

            db.AdditionalMembers.Add(mt);
            db.SaveChanges();

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var familyMember = db.FamilyMembers.ToList().Find(x => x.familyMemberId.Equals(ID));
            return Json(familyMember, JsonRequestBehavior.AllowGet);
        }

        // GET: MemberTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "memberTypeId,TypeName,payout")] MemberType memberType)
        {
            if (ModelState.IsValid)
            {
                db.MemberTypes.Add(memberType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberType);
        }

        // GET: MemberTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberType memberType = db.MemberTypes.Find(id);
            if (memberType == null)
            {
                return HttpNotFound();
            }
            return View(memberType);
        }

        // POST: MemberTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "memberTypeId,TypeName,payout")] MemberType memberType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberType);
        }

        // GET: MemberTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberType memberType = db.MemberTypes.Find(id);
            if (memberType == null)
            {
                return HttpNotFound();
            }
            return View(memberType);
        }

        // POST: MemberTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberType memberType = db.MemberTypes.Find(id);
            db.MemberTypes.Remove(memberType);
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

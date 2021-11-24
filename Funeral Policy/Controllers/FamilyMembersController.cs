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
    public class FamilyMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AdditionalMemberView additionalMemberView = new AdditionalMemberView();
        public ActionResult FamilyMemberView(int? id)
        {

            var results = db.FamilyMembers.ToList();
            if (User.IsInRole("Member"))
            {

                results = results.Where(x => x.HolderMailer == User.Identity.Name).ToList();


            }
            return View(results);
        }
        // GET: FamilyMembers
        //public ActionResult Index()
        //{
        //    var familyMembers = db.FamilyMembers.;
        //    return View(familyMembers.ToList());
        //}
        [HttpGet]
        public JsonResult List()
        {
            return Json(db.FamilyMembers.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.membertypeid = id;
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
            return View(familyMember);
        }
        [HttpGet]
        public JsonResult Add(int memberid, int membertypeid)
        {
            AdditionalMemberView mt = new AdditionalMemberView();
            mt.MemberTypeID = membertypeid;
            mt.MemberID = memberid;

            db.AdditionalMembers.Add(mt);
            db.SaveChanges();

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult Add(FamilyMember familyMember)
        //{
        //    db.FamilyMembers.Add(familyMember);
        //    db.SaveChanges();
        //    return Json(null, JsonRequestBehavior.AllowGet);



        //}



        
        //public JsonResult getMemberType( )
        //{
            

        //    List<MemberType> memberTypes = db.MemberTypes.ToList();

        //    return Json(memberTypes, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult getMemberTypeInFamily(int id)
        //{
        //    List<AdditionalMember> additionalMembers = db.AdditionalMembers.Where(x => x.MemberID == id).ToList();
        //    List<MemberType> memberTypes = new List<MemberType>();

        //    db.MemberTypes.ToList().ForEach(item =>
        //    {
        //        additionalMembers.ForEach(item2 =>
        //        {
        //            if (item.memberTypeId == item2.MemberTypeID)
        //            {
        //                memberTypes.Add(item);
        //            }
        //        });
        //    });

        //    return Json(memberTypes, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetbyID(int ID)
        {
            var familyMember = db.FamilyMembers.ToList().Find(x => x.familyMemberId.Equals(ID));
            return Json(familyMember, JsonRequestBehavior.AllowGet);
        }
        
       

        // GET: FamilyMembers/Create
        public ActionResult Create()
        {
           // var member = db.MemberApplications.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            
            return View();
        }

        // POST: FamilyMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "familyMemberId,Name,Surname,Gender,DateOfBirth")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                //FamilyMember fm = new FamilyMember();
                //fm.HolderMailer = User.Identity.Name;
                //fm.Name = fm.Name;
                //fm.Surname = fm.Surname;
                //fm.MemberType = fm.MemberType;
                db.FamilyMembers.Add(familyMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
            return View(familyMember);
        }

        // GET: FamilyMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
           
            return View(familyMember);
        }

        // POST: FamilyMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "familyMemberId,Name,Surname,Gender,DateOfBirth")] FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(familyMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(familyMember);
        }

        // GET: FamilyMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            if (familyMember == null)
            {
                return HttpNotFound();
            }
            return View(familyMember);
        }

        // POST: FamilyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FamilyMember familyMember = db.FamilyMembers.Find(id);
            db.FamilyMembers.Remove(familyMember);
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

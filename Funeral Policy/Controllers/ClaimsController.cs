using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Funeral_Policy.Models;
using IdentitySample.Models;

namespace Funeral_Policy.Controllers
{
    public class ClaimsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ClaimViewcs claimapp = new ClaimViewcs();
        public ActionResult CreateClaim()
        {
            return View();
        }
        // GET: Claims
        public ActionResult Index()
        {
            return View(db.Claims.ToList());
        }
        public ActionResult Index2()
        {
            var kl = db.Claims.ToList();
            if (User.IsInRole("Member"))
            {
                kl = kl.Where(o => o.CreatorClaim == User.Identity.Name).ToList();
            }
            return View(kl);
           // return View(db.Claims.ToList());
        }
        public ActionResult DeceasedView(int? id)
        {
            var results = db.Claims.ToList();
            if (User.IsInRole("Member"))
            {
                results = results.Where(x => x.CreatorClaim == User.Identity.Name).ToList();
            }
            // var results = db.Claims.Where(x => x.claimId==id).ToList();
            return View(results);
        }
        public ActionResult DeceasedView2(int? id)
        {
            var results = db.Claims.ToList();
            if (User.IsInRole("Admin"))
            {
                // results = results.Where(x => x.CreatorClaim == User.Identity.Name).ToList();
                results = db.Claims.Where(x => x.claimId== id).ToList();
            }
            // var results = db.Claims.Where(x => x.claimId==id).ToList();
            return View(results);
        }
        [HttpPost]
        public JsonResult SaveApplication(Claim claim)
        {
            db.Claims.Add(claim);
            db.SaveChanges();

            if (claim.claimId > 0)
            {
                return new JsonResult { Data = new { status = true, url = Url.Action("SaveDocuments", new { id = claim.claimId }) } };
            }
            return new JsonResult { Data = new { status = false } };
        }

        public ActionResult SaveDocuments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDocument appsDocs = new ClaimDocument();
            appsDocs.ClaimDocumentId = (int)id;

            return View(appsDocs);
        }

        [HttpPost]
        public ActionResult SaveDocuments([Bind(Exclude = "ClaimantCertifiedID,BankStatement,DeathCertificate,DeceasedCertifiedID")] ClaimDocument appDocs)
        {
            var claimApp = db.Claims.Where(m => m.claimId == appDocs.ClaimDocumentId).FirstOrDefault();
            try
            {
                claimApp.ClaimantCertifiedID = getFileById("ClaimantCertifiedID");
                claimApp.BankStatement = getFileById("BankStatement");
                claimApp.DeathCertificate = getFileById("DeathCertificate");
                claimApp.DeceasedCertifiedID = getFileById("DeceasedCertifiedID");
                claimApp.Status = "Waiting for Approval";
                claimApp.ClaimDate = DateTime.Now.Date;
                claimApp.CreatorClaim= User.Identity.Name;
                db.Entry(claimApp).State = EntityState.Modified;
                db.SaveChanges();
                //RegisterViewModel registerModel = new RegisterViewModel();
                //registerModel.MemberId = appDocs.applicationDocumentsId;

                //return RedirectToAction("CreateAccount", registerModel);
                return RedirectToAction("Index", "Home");

            }
            catch (Exception e)
            {
                ModelState.AddModelError("" + e.ToString(), "Unable to save changes. " +
                 "Try again, and if the problem persists see your system administrator.");


                return View(appDocs);
            }
        }


        // GET: Claims/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }
        public ActionResult OnceOff(int id)
        {

            return RedirectToAction("OnceOff", "PaymentPay", new { id = id });
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "claimId,MemberID,ClaimDate,Approved,ApprovedBy")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Claims.Add(claim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claim);
        }
        public ActionResult MakeClaim(int? id)
        {
            BILogic.ApplicationStatus(id, "Waiting For Approval");
            return RedirectToAction("CreateClaim", "Claims");
        }
        public ActionResult MakeFuneral(int? id)
        {
            BILogic.ApplicationStatus(id, "Funeral Booked");
          
            return RedirectToAction("Coffins", "Coffins");
        }
        public ActionResult Approve(int? id)
        {
            
                    
            BILogic.ApplicationStatus(id, "Approved");
            TempData["AlertMessage"] = "Claim hass passed review and is ready for Approval";
            return RedirectToAction("Index","MemberApplications");
        }
        
        public ActionResult Decline(int? id)
        {
            BILogic.ApplicationStatus(id, "Declined");
            TempData["AlertMessage"] = "The Claim has already been Declined";
            return RedirectToAction("Index");
        }

        // GET: Claims/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "claimId,MemberID,ClaimDate,Approved,ApprovedBy")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(claim);
        }

        // GET: Claims/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Claim claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private byte[] getFileById(string id)
        {
            byte[] fileData = null;
            HttpPostedFileBase poImgFile = Request.Files[id];

            using (var binary = new BinaryReader(poImgFile.InputStream))
            {
                fileData = binary.ReadBytes(poImgFile.ContentLength);
            }

            return fileData;
        }

        public FileResult OpenPDFIDDoc(int id)
        {
            var filebytes = db.Claims.Where(m => m.claimId == id).Select(k => k.ClaimantCertifiedID).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFBankStatementDoc(int id)
        {
            var filebytes = db.Claims.Where(m => m.claimId == id).Select(k => k.BankStatement).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFPaySlipDoc(int id)
        {
            var filebytes = db.Claims.Where(m => m.claimId == id).Select(k => k.DeathCertificate).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFProofOfAddressDoc(int id)
        {
            var filebytes = db.Claims.Where(m => m.claimId == id).Select(k => k.DeceasedCertifiedID).FirstOrDefault();
            return File(filebytes, "application/pdf");
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

using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Funeral_Policy.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace Funeral_Policy.Controllers
{
    public class QuotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MemberApplicationView membrapp = new MemberApplicationView();
        public ActionResult PlanView(int? id)
        {
            
            var results = db.quotes.ToList();
            if (User.IsInRole("Member"))
            {
               
                 results =results. Where(x => x.MemberEmail == User.Identity.Name).ToList();
                

            }
            return View(results);
        }
        public ActionResult PlanView2(int? id)
        {

            

            var results = db.quotes.ToList();
            if (User.IsInRole("Admin"))
            {



                results = db.quotes.Where(x => x.quoteId==id).ToList();


            }
                return View(results);

             
               
        }

        // GET: Quotes
        public ActionResult Index()
        {
            var kl = db.quotes.ToList();
            if (User.IsInRole("Member"))
            {
                kl = kl.Where(o => o.MemberEmail == User.Identity.Name).ToList();
            }
            return View(kl);
            
        }

        // GET: Quotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // GET: Quotes/Create
        public ActionResult Create()
        {
            
            //ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount");
            ViewBag.FuneralPlanId = new SelectList(db.funeralPlans, "FuneralPlanId", "FuneralPlanName");
           

            var memberapp = db.MemberApplications.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
            Quote qt = new Quote();
            qt.Name = memberapp.Name;
            qt.Surname = memberapp.Surname;
            qt.Gender = memberapp.Gender;
            qt.Phone = memberapp.Phone;
            qt.MemberAplicationId = memberapp.MemberAplicationId;
            
            //Quote quote = new Quote();
            //quote.PremiumCost = (decimal)c;
            return View(qt);
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "quoteId,Name,Surname,Gender,Age,Phone,FuneralPlanId,Quote_Date,PremiumCost")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                var memberApp = db.MemberApplications.Where(m => m.MemberAplicationId == quote.MemberAplicationId).FirstOrDefault();
                quote.Quote_Date = System.DateTime.Today;
                if (quote.Age <= 0)
                {
                    ViewBag.ErrorData = "Age must be a positive number";
                    return View(quote);
                }
                Quote quote1 = new Quote();
                quote1.MemberAplicationId = quote.MemberAplicationId;
                quote.MemberEmail = User.Identity.Name;
                quote.PlanName = quote.GetPlanName();
                quote.PlanDescription=quote.GetPlanDescription();
                quote.Funeralpayout = (double)quote.GetFuneralPayout();
                quote.PremiumCost = Convert.ToDecimal(quote.CalculatePrice());
                db.quotes.Add(quote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", quote.funeralCoverPayoutId);
            ViewBag.FuneralPlanId = new SelectList(db.funeralPlans, "FuneralPlanId", "FuneralPlanName", quote.FuneralPlanId);
            return View(quote);
        }
        public ActionResult QuoteAccept(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Incomplete" || memberApplication.Status == "Complete")
            {
               TempData["AlertMessage"] = "The application has been is completed";
               //  return RedirectToAction("QuoteAccept", "Quotes");
            }
            else
            {
                
                var username = User.Identity.GetUserName();
                memberApplication.PremiumCost = memberApplication.PremiumCost;
                
                memberApplication.Status = "Completed";
                db.Entry(memberApplication).State = EntityState.Modified;

                db.SaveChanges();

                //EmailService service = new EmailService();
                //string message = "Hi " + memberApplication.Tittle + " " + memberApplication.Surname + "\n your application for funeral has been approved, please login to accept policy. \n Kind Regards, \n Heavenly Sent Funerals";
                //service.CaptureEmail(memberApplication.Email, "Membership Application Approved", message);


            };

            // return RedirectToAction("Index2");
            return View();
        }




        // GET: Quotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            //ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", quote.funeralCoverPayoutId);
            ViewBag.FuneralPlanId = new SelectList(db.funeralPlans, "FuneralPlanId", "FuneralPlanName", quote.FuneralPlanId);
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "quoteId,Name,Surname,Gender,Age,Phone,FuneralPlanId,Quote_Date,PremiumCost")] Quote quote)
        {
            if (ModelState.IsValid)
            {
               

                db.Entry(quote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.funeralCoverPayoutId = new SelectList(db.funeralCoverPayouts, "funeralCoverPayoutId", "PayoutAmount", quote.funeralCoverPayoutId);
            ViewBag.FuneralPlanId = new SelectList(db.funeralPlans, "FuneralPlanId", "FuneralPlanName", quote.FuneralPlanId);
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quote quote = db.quotes.Find(id);
            db.quotes.Remove(quote);
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

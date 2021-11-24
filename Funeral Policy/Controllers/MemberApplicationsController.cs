using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Funeral_Policy.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using PayFast;
using PayFast.AspNet;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Rotativa;
using Funeral_Policy.ViewModel;

namespace Funeral_Policy.Controllers
{
    public class MemberApplicationsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        private MemberApplicationView membrapp = new MemberApplicationView();

        // GET: MemberApplications
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Index(string option, string search, int? pageNumber, string sort)
        {
            //if a user choose the radio button option as Subject  
            if (option == "MembershipNumber")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.MemberApplications.Where(x => x.MembershipNumber.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else if (option == "Status")
            {
                return View(db.MemberApplications.Where(x => x.Status.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 5));
            }
            else
            {
                return View(db.MemberApplications.Where(x => x.NID.StartsWith(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }

            // return View(db.MemberApplications.ToList());

        }
        public ActionResult Index2()
        {
            var kl = db.MemberApplications.ToList();

            if (User.IsInRole("Member"))
            {
                kl = kl.Where(o => o.Email == User.Identity.Name).ToList();
            }
            return View(kl);
        }
        


        [HttpPost]
        public JsonResult SaveApplication(MemberApplication memberApplication)
        {
            db.MemberApplications.Add(memberApplication);
            db.SaveChanges();

            if (memberApplication.MemberAplicationId > 0)
            {
                return new JsonResult { Data = new { status = true, url = Url.Action("SaveDocuments", new { id = memberApplication.MemberAplicationId }) } };
            }
            return new JsonResult { Data = new { status = false } };
        }

        public ActionResult SaveDocuments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationDocuments appsDocs = new ApplicationDocuments();
            appsDocs.applicationDocumentsId = (int)id;

            return View(appsDocs);
        }

        [HttpPost]
        public ActionResult SaveDocuments([Bind(Exclude = "CertifiedID,BankStatement,PaySlip,ProofOfAddress")] ApplicationDocuments appDocs)
        {
            var memberApp = db.MemberApplications.Where(m => m.MemberAplicationId == appDocs.applicationDocumentsId).FirstOrDefault();
            try
            {
                memberApp.CertifiedID = getFileById("CertifiedID");
                memberApp.BankStatement = getFileById("BankStatement");
                memberApp.PaySlip = getFileById("PaySlip");
                memberApp.ProofOfAddress = getFileById("ProofOfAddress");
                memberApp.Status = "Waiting For Approval";
                db.Entry(memberApp).State = EntityState.Modified;
                db.SaveChanges();
                RegisterViewModel registerModel = new RegisterViewModel();
                registerModel.MemberId = appDocs.applicationDocumentsId;
                
                return RedirectToAction("CreateAccount", registerModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("" + e.ToString(), "Unable to save changes. " +
                 "Try again, and if the problem persists see your system administrator.");


                return View(appDocs);
            }
        }

        // GET: MemberApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication == null)
            {
                return HttpNotFound();
            }
            return View(memberApplication);
        }
        public ActionResult ApproveApplication(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Approved" || memberApplication.Status == "Rejected")
            {
                TempData["AlertMessage"] = "The application has already been Approved";
                return RedirectToAction("Index");
            }
            else
            {
                memberApplication.Status = "Approved";
                db.Entry(memberApplication).State = EntityState.Modified;

                db.SaveChanges();

                EmailService service = new EmailService();
                string message = "Hi " + memberApplication.Tittle + " " + memberApplication.Surname + "\n your application for funeral has been approved, please login to accept policy. \n Kind Regards, \n Heavenly Sent Funerals";
                service.CaptureEmail(memberApplication.Email, "Membership Application Approved", message);


            };

            return RedirectToAction("Index");
        }

        public ActionResult AcceptPolicy(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            TempData["AlertMessage"] = "Policy Succesfully Accepted";
           
            memberApplication.Status = "Policy Accepted";
            db.Entry(memberApplication).State = EntityState.Modified;
            // db.MemberApplications.Add(memberApplication);
            db.SaveChanges();
            return RedirectToAction("Index2");

        }
        public ActionResult EmailMemberInfor(int id)
        {
            Session["bookID"] = id;
            EmailService service = new EmailService();
            MemberApplication memberApplication = db.MemberApplications.Where(p => p.MemberAplicationId == id).FirstOrDefault();
            var username = User.Identity.GetUserName();
            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(GeneratePDF(id)), "Proof of registration", "application/pdf"));
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(username, memberApplication.Name));
            string message = $"Hello {memberApplication.Tittle}, \n\n {memberApplication.Name} you been successfully registered for your membership. Please Find the attached registration information(Proof of Membership) \n Regards,\n Heaveanly Sent Funerals <br/> .";

            TempData["AlertMessage"] = $"{memberApplication.Name} has been successfully Registered";
            service.CaptureEmail(memberApplication.Email, "Membership Registered", message);
            //service.SendEmail(new Content()






            return RedirectToAction("Index2");
        }
        public ActionResult ApproveClaim(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Claimed" )
            {
                TempData["AlertMessage"] = "The Claim has already been Approved";
                return RedirectToAction("Index");
            }
            else
            {
                memberApplication.Status = "Claimed";
                db.Entry(memberApplication).State = EntityState.Modified;

                db.SaveChanges();

                EmailService service = new EmailService();
                string message = "Hi " + memberApplication.Tittle + " " + memberApplication.Surname + "\n your Claim has passed the review process thus approved, Expect payment within 48hrs. You can start preparing for your funeral \n Kind Regards, \n Heavenly Sent Funerals";
                service.CaptureEmail(memberApplication.Email, "Claim Approved", message);


            };

            return RedirectToAction("Index","Claims");
        }

        public ActionResult registerMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Registered" )
            {
                TempData["AlertMessage"] = "You have succesfully registered your policy";
                return RedirectToAction("Index");
            }
            if (memberApplication == null)
            {
                return HttpNotFound();
            }

            memberApplication.MembershipNumber = membrapp.generateMemberNumber(memberApplication.NID);
            memberApplication.Status = "Registered";
            db.Entry(memberApplication).State = EntityState.Modified;
            db.SaveChanges();

            var username = User.Identity.GetUserName();
            Member member = db.members.Where(p => p.NID == memberApplication.NID).FirstOrDefault();

            memberApplication.MembershipNumber = membrapp.generateMemberNumber(memberApplication.NID);
            memberApplication.Status = "Registered";
            db.Entry(memberApplication).State = EntityState.Modified;
            db.SaveChanges();
            return View(memberApplication);
            //return RedirectToAction("Index2");
        }
        public ActionResult RejectApplication(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Approved" || memberApplication.Status == "Rejected")
            {
                TempData["AlertMessage"] = "The application has already been Rejected";
                return RedirectToAction("Index");
            }
            else
            {
                memberApplication.Status = "Rejected";
                db.Entry(memberApplication).State = EntityState.Modified;

                db.SaveChanges();
                EmailService service = new EmailService();
                string message = "Hi " + memberApplication.Tittle + " " + memberApplication.Surname + "\n your application for funeral has been rejected, we will call you within 7 days to advise further. \n Kind Regards, \n Heavenly Sent Funerals";
                service.CaptureEmail(memberApplication.Email, "Membership Application Rejected", message);


            };

            return RedirectToAction("Index");
        }
        public ActionResult CountinueApp(int? id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication.Status == "Waiting For Approval")
            {
                TempData["AlertMessage"] = "The application has been completed";
                return RedirectToAction("Create", "Quotes");
            }
            else
            {
                Quote quote = new Quote();
                quote.MemberAplicationId = memberApplication.MemberAplicationId;

                var username = User.Identity.GetUserName();
                memberApplication.PremiumCost = memberApplication.PremiumCost;

                memberApplication.Status = "Complete";
                db.Entry(memberApplication).State = EntityState.Modified;

                db.SaveChanges();

                //EmailService service = new EmailService();
                //string message = "Hi " + memberApplication.Tittle + " " + memberApplication.Surname + "\n your application for funeral has been approved, please login to accept policy. \n Kind Regards, \n Heavenly Sent Funerals";
                //service.CaptureEmail(memberApplication.Email, "Membership Application Approved", message);


            };

            return RedirectToAction("Index");
        }


        // GET: MemberApplications/Create
        public ActionResult Create(int id, decimal c)
        {
            var memberapp = db.quotes.Where(m => m.MemberEmail == User.Identity.Name).FirstOrDefault();
            var member = db.quotes.Where(l => l.MemberAplicationId == membrapp.MemberAplicationId).FirstOrDefault();
            ViewBag.Id = id;
            MemberApplication memberApplication = new MemberApplication();
            memberapp.MemberAplicationId= id;
            memberApplication.PremiumCost = c;
            memberApplication.Payout = c;

            //qf.FuneralCoverPayout = quote.FuneralCoverPayout;

            return View(memberApplication);
        }
        public ActionResult CreateAccount(RegisterViewModel registerModel)
        {
            return View(registerModel);
        }

        // POST: MemberApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberApplication memberApplication)
        {
            var username = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                var memberapp = db.MemberApplications.Where(m => m.Email == User.Identity.Name).FirstOrDefault();
                Member member = new Member();
                member.Name = membrapp.Name;
                member.Surname = membrapp.Surname;
                member.Phone = membrapp.Phone;
                member.PremiumCost = membrapp.GetPremiumcost();
                member.Status = membrapp.Status;
                member.Status = "Pending Payment";

                db.members.Add(member);

                db.SaveChanges();


                //Quote quote = new Quote();
                //quote.FuneralPlan = quote.FuneralPlan;

                //quote.FuneralCoverPayout = quote.FuneralCoverPayout;
                //memberApplication.PremiumCost =memberApplication.GetPremiumCost();
                // db.MemberApplications.Add(memberApplication);
                // db.SaveChanges();
                // return RedirectToAction("Index");
            }

            return View(memberApplication);
        }

        // GET: MemberApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication == null)
            {
                return HttpNotFound();
            }
            return View(memberApplication);
        }

        // POST: MemberApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberAplicationId,Tittle,Name,Surname,Gender,NID,Race,Email,Phone,HomeAddress,city,Province,MonthlEarnings,Occupation,Education")] MemberApplication memberApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberApplication);
        }
        public FuneralBooking ordersd(int? id)
        {
            var df = db.FuneralBookings.Where(k => k.funeralBookingId == id).FirstOrDefault();
            return df;
        }

        // GET: MemberApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            if (memberApplication == null)
            {
                return HttpNotFound();
            }
            return View(memberApplication);
        }
       
        // POST: MemberApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberApplication memberApplication = db.MemberApplications.Find(id);
            db.MemberApplications.Remove(memberApplication);
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
            var filebytes = db.MemberApplications.Where(m => m.MemberAplicationId == id).Select(k => k.CertifiedID).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFBankStatementDoc(int id)
        {
            var filebytes = db.MemberApplications.Where(m => m.MemberAplicationId == id).Select(k => k.BankStatement).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFPaySlipDoc(int id)
        {
            var filebytes = db.MemberApplications.Where(m => m.MemberAplicationId == id).Select(k => k.PaySlip).FirstOrDefault();
            return File(filebytes, "application/pdf");
        }
        public FileResult OpenPDFProofOfAddressDoc(int id)
        {
            var filebytes = db.MemberApplications.Where(m => m.MemberAplicationId == id).Select(k => k.ProofOfAddress).FirstOrDefault();
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
        public byte[] GeneratePDF(int ReservationID)
        {
            
            MemoryStream memoryStream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            int ID = int.Parse(Session["bookID"].ToString());
            MemberApplication memberApplication = new MemberApplication();
            memberApplication = db.MemberApplications.Find(ID);

            //var tenant1 = db.Tenants.Find(roomBooking.TenantId);


            //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
            //var user = _iUserService.Get(reservation.UserID);

            iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.BaseColor.BLUE);

            // Create the heading paragraph with the headig font
            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(5);
            PdfPTable table3 = new PdfPTable(1);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            seperator.Offset = -6f;
            // Remove table cell
            table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table1.WidthPercentage = 80;
            table1.SetWidths(new float[] { 100 });
            table2.WidthPercentage = 80;
            table3.SetWidths(new float[] { 100 });
            table3.WidthPercentage = 80;

            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            table1.AddCell("\n");
            table1.AddCell(cell);
            table1.AddCell("\n\n");
            table1.AddCell(
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
                "Heavenly Sent Funerals \n" +
                "Email :Heavenly sent Funerals" + "\n" +
                "\n" + "\n");
            table1.AddCell("------------Member Details--------------!");
            table1.AddCell("Title : \t" + memberApplication.Tittle);
            table1.AddCell("Membership Number : \t" + memberApplication.MembershipNumber);
            table1.AddCell("Full Name : \t" + memberApplication.Name);
            table1.AddCell("Last Name : \t" + memberApplication.Surname);
            table1.AddCell("Identity Number : \t" + memberApplication.NID);
            table1.AddCell("Cell Phone : \t" + memberApplication.Phone);
            table1.AddCell("Home Address : \t" + memberApplication.HomeAddress);
            table1.AddCell("Gender : \t" + memberApplication.Gender);
            table1.AddCell("Monthly Earnings: \t" + memberApplication.MonthlyEarnings);
            table1.AddCell("Premium Cost : \t" + memberApplication.PremiumCost);
            table1.AddCell("Status : \t" + memberApplication.Status);
          
            table1.AddCell("\n");

            table3.AddCell("------------Looking forward to hear from you soon--------------!");

            //////Intergrate information into 1 document
            //var qrCode = iTextSharp.text.Image.GetInstance(reservation.QrCodeImage);
            //qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            //document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
        public MemberApplicationsController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }
        //Payment
        #region Fields

        private readonly PayFastSettings payFastSettings;

        #endregion Fields

        #region Constructor

        //public ApprovedOwnersController()
        //{

        //}

        #endregion Constructor

        #region Methods



        public ActionResult Recurring()
        {
            var recurringRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            recurringRequest.merchant_id = this.payFastSettings.MerchantId;
            recurringRequest.merchant_key = this.payFastSettings.MerchantKey;
            recurringRequest.return_url = this.payFastSettings.ReturnUrl;
            recurringRequest.cancel_url = this.payFastSettings.CancelUrl;
            recurringRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            recurringRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            recurringRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            recurringRequest.amount = 20;
            recurringRequest.item_name = "Recurring Option";
            recurringRequest.item_description = "Some details about the recurring option";

            // Transaction Options
            recurringRequest.email_confirmation = true;
            recurringRequest.confirmation_address = "drnendwandwe@gmail.com";

            // Recurring Billing Details
            recurringRequest.subscription_type = SubscriptionType.Subscription;
            recurringRequest.billing_date = DateTime.Now;
            recurringRequest.recurring_amount = 20;
            recurringRequest.frequency = BillingFrequency.Monthly;
            recurringRequest.cycles = 0;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{recurringRequest.ToString()}";

            return Redirect(redirectUrl);
        }
        //public ActionResult pay(int? id)
        //{
        //    StudentApplication studentApplication = db.Studentapplications.Find(id);
        //    var priceId = db.ClassFees.Where(p => p.ClassNameId == studentApplication.ClassNameId).Select(p => p.FeeTypeId).FirstOrDefault();
        //    var price = db.FeeTypes.Where(p => p.Id == priceId).Select(p => p.FeeAmount).FirstOrDefault();
        //    studentApplication.Status = "Paid";
        //    db.Entry(studentApplication).State = EntityState.Modified;

        //    // db.Studentapplications.Add(studentApplication);
        //    db.SaveChanges();
        //    return RedirectToAction("Index2");

        //}
        public ActionResult OnceOff(int? id)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details

            onceOffRequest.email_address = "sbtu01@payfast.co.za";
            //onceOffRequest.email_address = "sbtu01@payfast.co.za";
            MemberApplication memberApplication = db.MemberApplications.Find(id);

            var premium = db.quotes.Where(p => p.quoteId == id).Select(p => p.PremiumCost).FirstOrDefault();
            //var price = db.FeeTypes.Where(p => p.Id == priceId.FeeTypeId && p.Name == "Deposit").FirstOrDefault();
            memberApplication.Status = "Premium Paid";

            db.Entry(memberApplication).State = EntityState.Modified;
            db.SaveChanges();

            // Transaction Details
            var userName = User.Identity.GetUserName();
            Quote quote = db.quotes.Where(p => p.quoteId == id).FirstOrDefault();
            //student.deposit = SchoolLogic.getPrice("Deposit", (priceId.FeeTypeId + 1));
            //student.deposit = price.FeeAmount;


            onceOffRequest.m_payment_id = "";
            onceOffRequest.amount = Convert.ToDouble(quote.PremiumCost);
            onceOffRequest.item_name = "Monthly Premium";
            onceOffRequest.item_description = "Some details about the once off payment";

            quote.PremiumCost = quote.PremiumCost;
            quote.Status = "Premium Paid";
            db.Entry(quote).State = EntityState.Modified;
            db.SaveChanges();
            EmailService service = new EmailService();
            //var mailTo = new List<MailAddress>();
            //mailTo.Add(new MailAddress(memberApplication.Name, memberApplication.Surname));
            string message = $"Hello {memberApplication.Name}, Premium Paid R{quote.PremiumCost}, Date:{DateTime.Now.Date}\n Kind Regards, \n Heavenly Sent Funerals";
            service.CaptureEmail(memberApplication.Email, "Premium payment", message);





            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";
            
            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";
            return Redirect(redirectUrl);
           // return Redirect("Payment_Successfull");

        }

        public ActionResult AdHoc()
        {
            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            adHocRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            adHocRequest.m_payment_id = "";
            adHocRequest.amount = 70;
            adHocRequest.item_name = "Adhoc Agreement";
            adHocRequest.item_description = "Some details about the adhoc agreement";

            // Transaction Options
            adHocRequest.email_confirmation = true;
            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

            // Recurring Billing Details
            adHocRequest.subscription_type = SubscriptionType.AdHoc;

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

            return Redirect(redirectUrl);
        }

        public ActionResult Return()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
        {
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

            // Currently seems that the data validation only works for successful payments
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Error()
        {
            return View();
        }

        #endregion Methods
    
}
}

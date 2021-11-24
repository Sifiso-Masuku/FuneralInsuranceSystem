using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Funeral_Policy.Models;
using IdentitySample.Models;

namespace Funeral_Policy.Controllers
{
    public class CoffinsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Coffins
        public ActionResult Index()
        {
            return View(db.Coffins.ToList());
        }
        public ActionResult Coffins()
        {
            return View(db.Coffins.ToList());
        }

        // GET: Coffins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffin coffin = db.Coffins.Find(id);
            if (coffin == null)
            {
                return HttpNotFound();
            }
            return View(coffin);
        }

        // GET: Coffins/Create
        public ActionResult Create()
        {
            return View();
        }
        //Image
        // Convert file to binary
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }
        //Image
        //Display File
        public FileStreamResult RenderImage(int id)
        {
            MemoryStream ms = null;

            var item = db.Coffins.FirstOrDefault(x => x.coffinId == id);
           
            if (item != null)
            {
              
                ms = new MemoryStream(item.Image);
            }
            return new FileStreamResult(ms, item.ImageType);
        }



        // POST: Coffins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "coffinId,coffinName,coffinType,coffinPrice,ImageType,Image")] Coffin coffin, HttpPostedFileBase img_upload)
        {
            //Image
            if (img_upload != null && img_upload.ContentLength > 0)
            {
                coffin.ImageType = Path.GetExtension(img_upload.FileName);
                coffin.Image = ConvertToBytes(img_upload);
            }
            //
            var ve = db.Coffins.Where(p => p.coffinName == coffin.coffinName).Count();
            if (ve != 0)
            {
                TempData["AlertMessage"] = "Cannot add " + coffin.coffinName + " because it already exists";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Coffins.Add(coffin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

                return View(coffin);
            }
        

        // GET: Coffins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffin coffin = db.Coffins.Find(id);
            if (coffin == null)
            {
                return HttpNotFound();
            }
            return View(coffin);
        }

        // POST: Coffins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "coffinId,coffinName,coffinType,coffinPrice,ImageType,Image")] Coffin coffin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coffin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coffin);
        }

        // GET: Coffins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffin coffin = db.Coffins.Find(id);
            if (coffin == null)
            {
                return HttpNotFound();
            }
            return View(coffin);
        }

        // POST: Coffins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coffin coffin = db.Coffins.Find(id);
            db.Coffins.Remove(coffin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public PartialViewResult Coffin()
        {
            string[] arr = { "Coffin Type", "Coffin" };
            return PartialView("_Coffin", arr);
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

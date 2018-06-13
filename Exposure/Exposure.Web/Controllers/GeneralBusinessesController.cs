using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exposure.Entities;
using Exposure.Web.DataContexts;

namespace Exposure.Web.Controllers
{
    public class GeneralBusinessesController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: GeneralBusinesses
        public ActionResult Index()
        {
            return View(db.GeneralBusinesses.ToList());
        }

        // GET: GeneralBusinesses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralBusiness generalBusiness = db.GeneralBusinesses.Find(id);
            if (generalBusiness == null)
            {
                return HttpNotFound();
            }
            return View(generalBusiness);
        }

        // GET: GeneralBusinesses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeneralBusinesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude="CompanyLogo",Include = "ID,CompanyName,Slogan,Description")] GeneralBusiness gb)
        {

            byte[] imageData = null;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ProfilePic"];
                var image = poImgFile;
                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }

            }

            gb.CompanyLogo = imageData;

            if (ModelState.IsValid)
            {
                db.GeneralBusinesses.Add(gb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gb);
        }

        // GET: GeneralBusinesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralBusiness generalBusiness = db.GeneralBusinesses.Find(id);
            if (generalBusiness == null)
            {
                return HttpNotFound();
            }
            return View(generalBusiness);
        }

        // POST: GeneralBusinesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CompanyName,Slogan,Description")] GeneralBusiness generalBusiness)
        {
            if (ModelState.IsValid)
            {
                db.Entry(generalBusiness).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalBusiness);
        }

        // GET: GeneralBusinesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralBusiness generalBusiness = db.GeneralBusinesses.Find(id);
            if (generalBusiness == null)
            {
                return HttpNotFound();
            }
            return View(generalBusiness);
        }

        // POST: GeneralBusinesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeneralBusiness generalBusiness = db.GeneralBusinesses.Find(id);
            db.GeneralBusinesses.Remove(generalBusiness);
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

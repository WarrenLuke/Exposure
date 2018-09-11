using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exposure.Entities;
using Exposure.Web.DataContexts;
using PagedList;

namespace Exposure.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class HelpsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Helps
        public ActionResult Index(int page = 1, int pageSize = 10 )
        {
            var FAQ = db.Helps.OrderBy(x => x.HelpQuestion).ToList();
            PagedList<Help> model = new PagedList<Help>(FAQ, page, pageSize);
            ViewBag.FAQ = model;


            return View(model);
        }

        // GET: Helps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Help help = db.Helps.Find(id);
            if (help == null)
            {
                return HttpNotFound();
            }
            return View(help);
        }

        // GET: Helps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Helps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HelpID,HelpQuestion,HelpAnswer")] Help help)
        {
            if (ModelState.IsValid)
            {
                db.Helps.Add(help);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(help);
        }

        // GET: Helps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Help help = db.Helps.Find(id);
            if (help == null)
            {
                return HttpNotFound();
            }
            return View(help);
        }

        // POST: Helps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HelpID,HelpQuestion,HelpAnswer")] Help help)
        {
            if (ModelState.IsValid)
            {
                db.Entry(help).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(help);
        }

        

        // POST: Helps/Delete/5        
        public JsonResult Delete(int id)
        {
            bool result = false;
            Help help = db.Helps.Find(id);
            if(help!= null)
            {
                db.Helps.Remove(help);
                db.SaveChanges();
                result = true;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
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

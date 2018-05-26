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

namespace Exposure.Web.Controllers
{
    public class WorkerSkillsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: WorkerSkills
        public ActionResult Index()
        {
            var workerSkills = db.WorkerSkills.Include(w => w.Skill).Include(w => w.Worker);
            return View(workerSkills.ToList());
        }

        // GET: WorkerSkills/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkerSkill workerSkill = db.WorkerSkills.Find(id);
            if (workerSkill == null)
            {
                return HttpNotFound();
            }
            return View(workerSkill);
        }

        // GET: WorkerSkills/Create
        public ActionResult Create()
        {
            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription");
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID");
            return View();
        }

        // POST: WorkerSkills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkerID,SkillID,YearsOfExperience")] WorkerSkill workerSkill)
        {
            if (ModelState.IsValid)
            {
                db.WorkerSkills.Add(workerSkill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription", workerSkill.SkillID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", workerSkill.WorkerID);
            return View(workerSkill);
        }

        // GET: WorkerSkills/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkerSkill workerSkill = db.WorkerSkills.Find(id);
            if (workerSkill == null)
            {
                return HttpNotFound();
            }
            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription", workerSkill.SkillID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", workerSkill.WorkerID);
            return View(workerSkill);
        }

        // POST: WorkerSkills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkerID,SkillID,YearsOfExperience")] WorkerSkill workerSkill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workerSkill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription", workerSkill.SkillID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", workerSkill.WorkerID);
            return View(workerSkill);
        }

        // GET: WorkerSkills/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkerSkill workerSkill = db.WorkerSkills.Find(id);
            if (workerSkill == null)
            {
                return HttpNotFound();
            }
            return View(workerSkill);
        }

        // POST: WorkerSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WorkerSkill workerSkill = db.WorkerSkills.Find(id);
            db.WorkerSkills.Remove(workerSkill);
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

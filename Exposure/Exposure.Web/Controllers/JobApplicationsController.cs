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
    public class JobApplicationsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: JobApplications
        public ActionResult Index()
        {
            var jobApplications = db.JobApplications.Include(j => j.Job).Include(j => j.Worker);
            return View(jobApplications.ToList());
        }

        // GET: JobApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            return View(jobApplication);
        }

        // GET: JobApplications/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "EmployerID");
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID");
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobApplicationID,JobID,WorkerID,Motivation,Response")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                db.JobApplications.Add(jobApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "EmployerID", jobApplication.JobID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", jobApplication.WorkerID);
            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "EmployerID", jobApplication.JobID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", jobApplication.WorkerID);
            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobApplicationID,JobID,WorkerID,Motivation,Response")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "JobID", "EmployerID", jobApplication.JobID);
            ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerID", jobApplication.WorkerID);
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication jobApplication = db.JobApplications.Find(id);
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobApplication jobApplication = db.JobApplications.Find(id);
            db.JobApplications.Remove(jobApplication);
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

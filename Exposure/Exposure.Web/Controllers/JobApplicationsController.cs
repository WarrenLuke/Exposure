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
using Microsoft.AspNet.Identity;


namespace Exposure.Web.Controllers
{
    [Authorize]
    public class JobApplicationsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: JobApplications
        public ActionResult Index(int? skill)
        {

            var jobApplications =db.JobApplications.Include(j => j.Job).Include(w=>w.Worker).Include(e=>e.Job.Employer).Include(s=>s.Job.Skill).Where(f=>f.Flagged.Equals(false)); 

            if(User.IsInRole("Worker"))
            {
                var userID = User.Identity.GetUserId();
                jobApplications = jobApplications.Where(w => w.Worker.WorkerID.Equals(userID)).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }
            else if(User.IsInRole("Employer"))
            {
                var userID = User.Identity.GetUserId();
                jobApplications = jobApplications.Where(e => e.Job.Employer.EmployerID.Equals(userID)).Include(w=>w.Worker).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }

            if(skill!=null)
            {
                jobApplications = jobApplications.Where(s => s.Job.Skill.SkillID.Equals(skill)).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }
            
            ViewBag.Applications = jobApplications;
            ViewBag.skill = new SelectList(db.Skills, "SkillID", "SkillDescription");
            return View();
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
        [Authorize(Roles =("Worker"))]
        public ActionResult Create(int id)
        {
            ViewBag.Job = db.Jobs.Include(s => s.Employer).Include(s => s.Employer.ApplicationUser).Include(s => s.Suburb).Where(s => s.JobID.Equals(id));
            ViewBag.JobID = id;
            ViewBag.WorkerID = User.Identity.GetUserId(); 
            
            
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude="WorkerID, ApplicationDate, Response",Include = "JobApplicationID,JobID,Motivation, Flagged")] JobApplication jobApplication)
        {
            jobApplication.ApplicationDate = DateTime.UtcNow;
            jobApplication.WorkerID = User.Identity.GetUserId();
            jobApplication.Response = Reply.Pending;
            var state = ModelState;
            var apps = db.JobApplications.Where(w => w.WorkerID == User.Identity.GetUserId()).Where(j => j.JobID == jobApplication.JobID);            
            if(apps != null)
            {
                TempData["ApplicationTrue"] = "You have already applied for this Job. Please select a different one.";
                return RedirectToAction("Create", jobApplication.JobID);
            }

            
            if(ModelState.IsValid)
            {
                db.JobApplications.Add(jobApplication);
                db.SaveChanges();
                TempData["ApplicationSuccess"] = "Your application has been submitted";
                return RedirectToRoute("Defualt", new { controler = "JobApplications", action = "Index" });
            }           
            

            ViewBag.JobID = jobApplication.JobID;
            ViewBag.Employer = db.Jobs.Include(s => s.Employer).Include(s => s.Employer.ApplicationUser).Include(s => s.Suburb).Where(s => s.JobID.Equals(jobApplication.JobID));
            ViewBag.WorkerID = User.Identity.GetUserId();
            TempData["ApplicationSuccess"] = "Your application was not submitted. Please review the all details on this form";
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
        public ActionResult DeleteConfirmed(int id, JobApplication jobApplication)
        {
            jobApplication = db.JobApplications.Find(id);

            jobApplication.Flagged = true;
            //db.JobApplications.Remove(jobApplication);
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(jobApplication).State = EntityState.Modified;
                    db.SaveChanges();
                }catch
                {
                    db.SaveChanges();
                }
                
            }
            
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

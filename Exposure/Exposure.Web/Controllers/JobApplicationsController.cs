﻿using System;
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
        public ActionResult Index(string currentFilter, string sortOrder, int? skill, string search, int? location, DateTime? frmDate, DateTime? toDate, int page = 1, int pageSize = 10)
        {

            var jobApplications = db.JobApplications.Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false));

            switch (sortOrder)
            {
                case "title_desc":
                    jobApplications = jobApplications.OrderBy(j => j.Job.Title);
                    break;
                case "date":
                    jobApplications = jobApplications.OrderByDescending(j => j.Job.StartDate);
                    break;
                case "date_desc":
                    jobApplications = jobApplications.OrderBy(j => j.Job.StartDate);
                    break;
                case "rate_desc":
                    jobApplications = jobApplications.OrderByDescending(j => j.Job.Rate);
                    break;
                case "rate":
                    jobApplications = jobApplications.OrderBy(j => j.Job.Rate);
                    break;
                default:
                    jobApplications = jobApplications.OrderByDescending(j => j.Job.StartDate);
                    break;
            }

            if (frmDate != null && toDate != null)
            {
                jobApplications = jobApplications.Where(x => x.Job.StartDate > frmDate && x.Job.StartDate < toDate);
            }
            else if (frmDate != null)
            {
                jobApplications = jobApplications.Where(x => x.Job.StartDate == frmDate);
            }
            else if (toDate != null)
            {
                jobApplications = jobApplications.Where(x => x.Job.StartDate == toDate);
            }

            if (location != null)
            {
                jobApplications = jobApplications.Where(j => j.Job.Suburb.SuburbID == location);
            }

            if (User.IsInRole("Worker"))
            {
                var userID = User.Identity.GetUserId();
                jobApplications = jobApplications.Where(w => w.Worker.WorkerID.Equals(userID)).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }
            else if (User.IsInRole("Employer"))
            {
                var userID = User.Identity.GetUserId();
                jobApplications = jobApplications.Where(e => e.Job.Employer.EmployerID.Equals(userID)).Include(w => w.Worker).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }

            if (skill != null)
            {
                jobApplications = jobApplications.Where(s => s.Job.Skill.SkillID == skill).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false)); ;
            }

            var rows = jobApplications.Count();

            if (rows == 0)
            {
                TempData["Empty"] = "No applications available for this type.";
            }

            ViewBag.Rejected = Reply.Rejected;
            ViewBag.Hired = Reply.Hired;
            ViewBag.Completed = true;
            ViewBag.Applications = jobApplications;
            ViewBag.location = new SelectList(db.Suburbs, "SuburbID", "SubName");
            ViewBag.skill = new SelectList(db.Skills.OrderBy(x => x.SkillDescription), "SkillID", "SkillDescription");

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
        [Authorize(Roles = ("Worker"))]
        public ActionResult Create(int id, int skill, string notification)
        {
            var userId = User.Identity.GetUserId();
            Worker worker = db.Workers.Find(userId);
            var currentDate = DateTime.UtcNow;

            var skills = db.WorkerSkills.Include(a => a.Worker).Include(q => q.Skill).Where(w => w.Worker.WorkerID == userId);
            var job = db.Jobs.Include(s => s.Employer).Include(s => s.Employer.ApplicationUser).Include(s => s.Suburb).Where(s => s.JobID==id);
            var jobExpiry = db.Jobs.Include(s => s.Employer).Include(s => s.Employer.ApplicationUser).Include(s => s.Suburb).Where(s => s.JobID == id).Where(x => x.ExpiryDate > currentDate);

            ViewBag.Job = job;
            ViewBag.JobID = id;
            var Appcheck = 0;
            var check = skills.Count();
            var apps = db.JobApplications.Where(w => w.WorkerID == userId).Where(j => j.JobID == id).Where(x=>x.Flagged==false).Count();
            var totApps = db.JobApplications.Where(j => j.JobID == id).Count();

            if(jobExpiry.Count() == 0)
            {
                TempData["AppClose"] = "Applications for this job has closed.";
            }

            ViewBag.totApps = totApps;
            if (apps > 0)
            {
                Appcheck = 1;
            }

            if (Appcheck > 0)
            {
                TempData["ApplicationTrue"] = "You have already applied for this Job. Please ";
                return View();
            }

            var result = false;
            if (check > 0)
            {

                foreach (var s in skills)
                {
                    if (skill == s.SkillID)
                    {
                        result = true;
                    }
                }
            }          

            if (result == false)
            {
                TempData["Skill"] = "You dont have the necessary skill to apply for this job";
            }

            ViewBag.Notification = notification;
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "WorkerID, ApplicationDate, Response", Include = "JobApplicationID,JobID,Motivation, Flagged")] JobApplication jobApplication)
        {
            var id = User.Identity.GetUserId();
            jobApplication.ApplicationDate = DateTime.UtcNow;
            jobApplication.WorkerID = id;
            jobApplication.Response = Reply.Pending;
            var state = ModelState;


            try
            {
                db.JobApplications.Add(jobApplication);
                db.SaveChanges();
                TempData["ApplicationSuccess"] = "Your application has been submitted";
                return RedirectToRoute("Default", new { controler = "JobApplications", action = "Index" });
            }
            catch { }
            //catch
            //{
            //    db.SaveChanges();
            //    TempData["ApplicationSuccess"] = "Your application has been submitted";
            //    return RedirectToRoute("Defualt", new { controler = "JobApplications", action = "Index" });
            //}           

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
            if (User.IsInRole("Employer"))
            {
                if (jobApplication.Replied == true)
                {
                    TempData["Replied"] = "You have already replied to this application.";
                }
            }
            ViewBag.JobAppID = db.JobApplications.Where(i => i.JobApplicationID == id);
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
            JobApplication ja = db.JobApplications.Find(jobApplication.JobApplicationID);


            var id = jobApplication.JobApplicationID;

            var apps = db.JobApplications.Where(x => x.JobApplicationID != id).Where(x=>x.JobID== jobApplication.JobID).Select(x => x.JobApplicationID);

            var appList = db.JobApplications.Where(j => j.JobID == jobApplication.JobID).Where(j => j.JobApplicationID != jobApplication.JobApplicationID);         


            
            if (User.IsInRole("Worker"))
            {
                ja.Motivation = jobApplication.Motivation;
            }
            else if (User.IsInRole("Employer"))
            {
                ja.Response = jobApplication.Response;

                ja.Replied = true;   
                if(jobApplication.Response == Reply.Hired)
                {
                    foreach (var item in apps)
                    {
                        JobApplication j = db.JobApplications.Find(item);
                        j.Response = Reply.Rejected;
                        j.Replied = true;
                        db.Entry(j).State = EntityState.Modified;
                    }
                }                

            }

            if(ModelState.IsValid)
            {
                db.Entry(ja).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }  
            
            
            ViewBag.JobAppID = db.JobApplications.Where(i => i.JobApplicationID == ja.JobApplicationID);
            ViewBag.JobID = jobApplication.JobID;
            ViewBag.WorkerID = jobApplication.WorkerID;

            return View(ja);
        }

        
        public JsonResult Delete(int id)
        {
            JobApplication ja = db.JobApplications.Find(id);

            //db.JobApplications.Remove(jobApplication);
            bool result = false;
            if(ja != null)
            {
                ja.Flagged = true;
                db.Entry(ja).State = EntityState.Modified;
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

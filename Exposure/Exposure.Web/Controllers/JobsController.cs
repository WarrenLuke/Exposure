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
using PagedList;

namespace Exposure.Web.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Jobs
        public ActionResult Index(string id, int? skill, DateTime? startDate, string search, int? page)
        {

            ViewBag.Skills = new SelectList(db.Skills, "SkillID", "SkillDescription");
            var jobs = db.Jobs.Include(j => j.Employer);
            //var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //var start = Request.Form.GetValues("start").FirstOrDefault();
            //var length = Request.Form.GetValues("length").FirstOrDefault();

            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;
            //int recordsTotal = 0;

            if (User.IsInRole("Admin"))
            {
                jobs = db.Jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).OrderBy(j => j.DateAdvertised);
            }            
            else if(User.IsInRole("Employer"))
            {
                var userId = User.Identity.GetUserId();
                jobs = db.Jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).OrderBy(j => j.DateAdvertised).Where(j => j.Employer.EmployerID == userId);

            }
            else if (User.IsInRole("Worker"))
            {
                var jobHistory = db.JobApplications.Include(j => j.Job.Employer).Include(j => j.Job).Include(j => j.WorkerID.Equals(id));
            }


            if (!String.IsNullOrEmpty(id))
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.EmployerID.Equals(id)).OrderBy(j => j.DateAdvertised);
            }

            if (skill != null)
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.SkillID == skill).OrderBy(j => j.DateAdvertised);
            }

            if (startDate != null)
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.StartDate >= startDate).OrderBy(j => j.DateAdvertised);
            }

            if (!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.Title.Contains(search)).OrderBy(j => j.DateAdvertised);
            }

            //var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser);
            var workers = db.JobApplications.Include(j => j.Job).Include(j => j.Worker).Include(j => j.Worker.ApplicationUser).Include(w => w.Worker).Where(w => w.Response == Reply.Hired);

            //if (User.IsInRole("Worker"))
            //{
            //    ViewBag.Worker = User.Identity.GetUserId();

            //}
            //else if (User.IsInRole("Employer"))
            //{
            //    ViewBag.Employer = User.Identity.GetUserId();
            //}


            //ViewBag.JobID = job;
            //ViewBag.JList = jobs;
            ViewBag.WList = workers;
            //ViewBag.Jobs = jobs;

            return View(jobs);
                                 
            
        }

        [AllowAnonymous]
        public ActionResult Search(string currentFilter,string sortOrder, int? skill, string search, int? page, int? location)
        {                      

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;              

            ViewBag.skill = new SelectList(db.Skills.OrderBy(x=>x.SkillDescription), "SkillID", "SkillDescription");
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.location = new SelectList(db.Suburbs, "SuburbID", "SubName");
            var jobs = db.Jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(c=>c.Completed==false);

            switch (sortOrder)
            {
                case "title_desc":
                    jobs = jobs.OrderByDescending(j => j.Title);
                    break;
                case "date":
                    jobs = jobs.OrderBy(j => j.DateAdvertised);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(j => j.DateAdvertised);
                    break;
                case "rate_desc":
                    jobs = jobs.OrderByDescending(j => j.Rate);
                    break;
                case "rate":
                    jobs = jobs.OrderBy(j => j.Rate);
                    break;
                default:
                    jobs = jobs.OrderBy(j => j.DateAdvertised);
                    break;
            }
        
            if (skill != null)
            {                              
                   jobs = jobs.Where(j => j.SkillID==skill);             
                                
            }

            if (location!=null)
            {
                jobs = jobs.Where(j => j.Suburb.SuburbID==location);
            }

            if (!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j => j.Title.Contains(search));                                        
            }
            
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            var rows = jobs.Count();
            if(rows==0)
            {
                TempData["Empty"] = "No jobs available. Please edit your search criteria or check back later";
                
            }
            
            ViewBag.Jobs = jobs;

            return View(jobs);
        }

        public ActionResult History(string SortOrder, string id, int? skill, DateTime? startDate, string search)
        {
            var userId = User.Identity.GetUserId();
            var jobsHistory = db.JobApplications.Include(j => j.Job.Employer).Include(j => j.Job.Skill).Include(j => j.Job.Suburb).Where(j => j.Job.Employer.EmployerID.Equals(id)).OrderBy(j => j.Job.DateAdvertised).Include(w=>w.WorkerID.Equals(userId));

            if (!String.IsNullOrEmpty(id))
            {
                jobsHistory = jobsHistory.Where(j => j.Job.Employer.EmployerID.Equals(id));
            }

            if (skill != null)
            {
                jobsHistory = jobsHistory.Where(j => j.Job.SkillID == skill);
            }

            if (startDate != null)
            {
                jobsHistory = jobsHistory.Where(j => j.Job.StartDate >= startDate);
            }

            if (!String.IsNullOrEmpty(search))
            {
                jobsHistory = jobsHistory.Where(j => j.Job.Title.Contains(search));
            }

            ViewBag.jobHistory = jobsHistory;
            return View(jobsHistory);

        }

        public ActionResult JobApplication(int? id)
        {
            return RedirectToRoute("Default", new { controller = "JobApplications", action = "Create", id=id });
        }

        ////GET: Jobs/Update
        [Authorize(Roles = "Admin, Employer")]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Job job = db.Jobs.Find(id);

            if (job == null)
            {
                return HttpNotFound();
            }

            var AppCheck = db.JobApplications.Where(j => j.JobID == (id)).Where(j => j.Response == Reply.Hired);
            var rows = AppCheck.Count();

            if(rows==0)
            {
                TempData["Application"] = "No application has been accepted/submitted for this job yet.";
            }

            if(job.Completed==true)
            {
                TempData["Completed"] = "Job already completed";
            }
            
            ViewBag.Job = db.Jobs.Where(j => j.JobID == id);
            ViewBag.JobID = id;
            ViewBag.JobStart = Convert.ToDateTime(job.StartDate);
            ViewBag.JobEnd = Convert.ToDateTime(job.EndDate);
            ViewBag.SuburbID = new SelectList(db.Suburbs,"SuburbID", "SubName", job.SuburbID);
            return View(job);

        }

        //POST: Jobs/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Employer")]
        public ActionResult Update([Bind(Include = "JobID,Completed")]Job job)
        {           
            Job a = db.Jobs.Find(job.JobID);

            a.Completed = job.Completed;     

            var state = ModelState;
            try
            {
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Job = db.Jobs.Where(j => j.JobID == job.JobID);
                ViewBag.JobID = job.JobID;
                ViewBag.SuburbID = new SelectList(db.Suburbs, job.SuburbID);
                return View(job);
            }        
                        
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        [Authorize(Roles =("Admin, Employer"))]
        public ActionResult Create()
        {
            var skills = db.Skills.ToList().OrderBy(s=>s.SkillDescription);
            ViewBag.Skills = skills;
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.SkillID = new SelectList(db.Skills.OrderBy(x=>x.SkillDescription), "SkillID", "SkillDescription");
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(s=>s.SubName), "SuburbID", "SubName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =("Admin, Employer"))]
        public ActionResult Create([Bind(Exclude ="EmployerID, DateAdvertised", Include = "JobID,Title,StartDate, EndDate,SkillID,Description,StartTime,EndTime,Rate,SuburbID, AddressLine1,AddressLine2")] Job job)
        {
            job.EmployerID = User.Identity.GetUserId();

            job.DateAdvertised = DateTime.UtcNow;

            var state = ModelState;
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                TempData["JobSuccess"] = "Job Successfully created";
                return RedirectToRoute("Default", new { controller = "Jobs", action = "Index", id=User.Identity.GetUserId() });
            }

            
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.SkillID = new SelectList(db.Skills.OrderBy(x=>x.SkillDescription), "SkillID", "SkillDescription", job.SkillID);
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(s=>s.SubName), "SuburbID", "SubName", job.SuburbID);
            TempData["JobSuccess"] = "Job could not be created. Please try again.";
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {           
                    

            ViewBag.jobSkills = new SelectList(db.Skills, "SkillID", "SkillDescription");

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if(job==null)
            {
                return HttpNotFound();
            }

            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", job.SuburbID);
            ViewBag.JobID = id;

            return View(job);
            
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude ="DateAdvertised, Title",Include = "JobID,Title,DateAdvertised,SkillID,Description,StartTime,EndTime,Rate,SuburbID, AddressLine1,AddressLine2,StartDate, EndDate")] Job job)
        {
            Job j = db.Jobs.Find(job.JobID);
                                
            j.Description = job.Description;
            j.StartTime = job.StartTime;
            j.EndDate = job.EndDate;
            j.EndTime = job.EndTime;
            j.Rate = job.Rate;
            j.SuburbID = job.SuburbID;
            j.AddressLine1 = job.AddressLine1;
            j.AddressLine2 = job.AddressLine2;

           
                try
                {
                    db.Entry(j).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new {id=User.Identity.GetUserId() });
                }
                catch
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
                }
                    
                        
            ViewBag.jobSkills = new SelectList(db.Skills, "SkillID", "SkillDescription");
            ViewBag.JobID = j.JobID;
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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

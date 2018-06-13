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

            var jobs = db.Jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).OrderBy(j => j.DateAdvertised);


            if (!String.IsNullOrEmpty(id))
            {
                jobs = jobs.Where(j => j.EmployerID.Equals(id)).OrderBy(j => j.DateAdvertised);
            }

            if (skill != null)
            {
                jobs = jobs.Where(j => j.SkillID == skill).OrderBy(j => j.DateAdvertised);
            }

            if (startDate != null)
            {
                jobs = jobs.Where(j => j.StartDate >= startDate).OrderBy(j => j.DateAdvertised);
            }

            if (!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j => j.Title.Contains(search)).OrderBy(j => j.DateAdvertised);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            
            ViewBag.Jobs = jobs;


            return View(jobs);
        }

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

            ViewBag.skill = new SelectList(db.Skills, "SkillID", "SkillDescription");
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
            
            ViewBag.Jobs = jobs;

            return View(jobs);
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

            ViewBag.Job = db.Jobs.Where(j => j.JobID == id);
            ViewBag.JobID = id;
            ViewBag.JobStart = Convert.ToDateTime(job.StartDate);
            ViewBag.JobEnd = Convert.ToDateTime(job.EndDate);

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
            
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription");
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName");
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
                return RedirectToRoute("Default", new { controller = "Jobs", action = "Index" });
            }

            
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.SkillID = new SelectList(db.Skills, "SkillID", "SkillDescription", job.SkillID);
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", job.SuburbID);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            var SkillsLst = new List<string>();

            var SkillsQry = from d in db.Skills
                            orderby d.SkillDescription
                            select d.SkillDescription;

            SkillsLst.AddRange(SkillsQry.Distinct());

            ViewBag.jobSkills = new SelectList(SkillsLst, "SkillID", "SkillDescription");

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if(job==null)
            {
                return HttpNotFound();
            }
            


            return View(job);
            
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobID,Title,DateAdvertised,SkillID,Description,StartTime,EndTime,Rate,SuburbID, AddressLine1,AddressLine2,StartDate, EndDate")] Job job)
        {
            Job j = db.Jobs.Find(job.JobID);
            j.Title = job.Title;
            j.DateAdvertised = job.DateAdvertised;            
            j.Description = job.Description;
            j.StartTime = job.StartTime;
            j.EndDate = job.EndDate;
            j.EndTime = job.EndTime;
            j.Rate = job.Rate;
            j.SuburbID = job.SuburbID;
            j.AddressLine1 = job.AddressLine1;
            j.AddressLine2 = job.AddressLine2;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(j).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    db.SaveChanges();
                }               
                
                
            }

            var SkillsLst = new List<string>();

            var SkillsQry = from d in db.Skills
                            orderby d.SkillDescription
                            select d.SkillDescription;

            SkillsLst.AddRange(SkillsQry.Distinct());
            ViewBag.jobSkills = new SelectList(SkillsLst, "SkillID", "SkillDescription");
            
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

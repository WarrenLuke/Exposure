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
using Exposure.Web.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Exposure.Web.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Jobs
        public ActionResult Index(string id, int? skill, DateTime? frmDate, DateTime? toDate, string search, string sortOrder, int? location, int page = 1, int pageSize = 10)
        {

            var jobs = db.Jobs.Include(j => j.Employer);
            var jobHistory = db.JobApplications.Include(j => j.Job);

            #region Sorting
            switch (sortOrder)
            {
                case "title_desc":
                    jobs = jobs.OrderByDescending(j => j.Title);
                    jobHistory = jobHistory.OrderByDescending(j => j.Job.Title);
                    break;
                case "date":
                    jobs = jobs.OrderBy(j => j.DateAdvertised);
                    jobHistory = jobHistory.OrderBy(j => j.Job.StartDate);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(j => j.DateAdvertised);
                    jobHistory = jobHistory.OrderByDescending(j => j.Job.StartDate);
                    break;
                case "rate_desc":
                    jobs = jobs.OrderByDescending(j => j.Rate);
                    jobHistory = jobHistory.OrderByDescending(j => j.Job.Rate);
                    break;
                case "rate":
                    jobs = jobs.OrderBy(j => j.Rate);
                    jobHistory = jobHistory.OrderBy(j => j.Job.Rate);
                    break;
                default:
                    jobs = jobs.OrderBy(j => j.DateAdvertised);
                    jobHistory = jobHistory.OrderBy(j => j.Job.StartDate);
                    break;
            }
            #endregion

            #region RoleCheck
            if (User.IsInRole("Admin"))
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb);
            }
            else if (User.IsInRole("Employer"))
            {
                var userId = User.Identity.GetUserId();
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.Employer.EmployerID == userId);

            }
            else if (User.IsInRole("Worker"))
            {
                jobHistory = jobHistory.Where(j => j.WorkerID == id).Where(x => x.Flagged != true).Where(x => x.Response == Reply.Hired);
            }
            #endregion

            #region DateParams
            if (frmDate != null && toDate != null)
            {
                jobs = jobs.Where(x => x.StartDate >= frmDate && x.StartDate <= toDate);
                jobHistory = jobHistory.Where(x => x.Job.StartDate > frmDate && x.Job.StartDate < toDate);
            }
            else if (frmDate != null)
            {
                jobs = jobs.Where(x => x.StartDate >= frmDate);
                jobHistory = jobHistory.Where(x => x.Job.StartDate >= frmDate);

            }
            else if (toDate != null)
            {
                jobs = jobs.Where(x => x.StartDate >= toDate);
                jobHistory = jobHistory.Where(x => x.Job.StartDate >= toDate);

            }
            #endregion


            if (!String.IsNullOrEmpty(id))
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.EmployerID.Equals(id));
            }

            if (location != null)
            {
                jobs = jobs.Where(x => x.SuburbID == location);
            }

            if (skill != null)
            {
                jobs = jobs.Where(j => j.SkillID == skill);
            }


            if (!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(j => j.Title.Contains(search)).Where(j => j.Description.Contains(search));
            }

            //var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser);
            var workers = db.JobApplications.Include(j => j.Job).Include(j => j.Worker).Include(j => j.Worker.ApplicationUser).Include(w => w.Worker).Where(w => w.Response == Reply.Hired);

            PagedList<Job> model = new PagedList<Job>(jobs, page, pageSize);
            PagedList<JobApplication> jaModel = new PagedList<JobApplication>(jobHistory, page, pageSize);
            //ViewBag.JobID = job;
            //ViewBag.JList = jobs;
            ViewBag.Jobs = model;
            ViewBag.WList = workers;
            ViewBag.JobAmt = jobs.Count();
            ViewBag.JobHistory = jaModel;
            ViewBag.skill = new SelectList(db.Skills.OrderBy(x => x.SkillDescription), "SkillID", "SkillDescription");
            ViewBag.location = new SelectList(db.Suburbs.OrderBy(x => x.SubName), "SuburbID", "SubName");
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            return View(model);


        }

        [AllowAnonymous]
        public ActionResult Search(string currentFilter, string sortOrder, int? skill, string search, int? location, DateTime? frmDate, DateTime? toDate, int page = 1, int pageSize = 10)
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

            ViewBag.skill = new SelectList(db.Skills.OrderBy(x => x.SkillDescription), "SkillID", "SkillDescription");
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.location = new SelectList(db.Suburbs, "SuburbID", "SubName");
            var currentDate = DateTime.UtcNow;
            var jobs = db.Jobs.Include(j => j.Employer).Include(j => j.Skill).Include(j => j.Suburb).Where(c => c.Completed == false).Where(x => x.ExpiryDate > currentDate);

            #region Sorting
            switch (sortOrder)
            {
                case "title_desc":
                    jobs = jobs.OrderBy(j => j.Title);
                    break;
                case "date":
                    jobs = jobs.OrderByDescending(j => j.DateAdvertised);
                    break;
                case "date_desc":
                    jobs = jobs.OrderBy(j => j.DateAdvertised);
                    break;
                case "rate_desc":
                    jobs = jobs.OrderByDescending(j => j.Rate);
                    break;
                case "rate":
                    jobs = jobs.OrderBy(j => j.Rate);
                    break;
                default:
                    jobs = jobs.OrderByDescending(j => j.DateAdvertised);
                    break;
            }
            #endregion

            #region DateParams
            if (frmDate != null && toDate != null)
            {
                jobs = jobs.Where(x => x.StartDate > frmDate && x.StartDate < toDate);
            }
            else if (frmDate != null)
            {
                jobs = jobs.Where(x => x.StartDate == frmDate);
            }
            else if (toDate != null)
            {
                jobs = jobs.Where(x => x.StartDate == toDate);
            }
            #endregion

            if (skill != null)
            {
                jobs = jobs.Where(j => j.SkillID == skill);
            }

            if (location != null)
            {
                jobs = jobs.Where(j => j.Suburb.SuburbID == location);
            }

            if (!String.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j => j.Title.Contains(search));
            }

            var jobsList = jobs.ToList();
            PagedList<Job> model = new PagedList<Job>(jobsList, page, pageSize);

            var rows = jobs.Count();
            if (rows == 0)
            {
                TempData["Empty"] = "No jobs available. Please edit your search criteria or check back later";
            }

            ViewBag.Jobs = model;

            return View(model);
        }

        public ActionResult History(string SortOrder, string id, int? skill, DateTime? startDate, string search)
        {
            var userId = User.Identity.GetUserId();
            var jobsHistory = db.JobApplications.Include(j => j.Job.Employer).Include(j => j.Job.Skill).Include(j => j.Job.Suburb).Where(j => j.Job.Employer.EmployerID.Equals(id)).OrderBy(j => j.Job.DateAdvertised).Include(w => w.WorkerID.Equals(userId));

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
            return RedirectToRoute("Default", new { controller = "JobApplications", action = "Create", id = id });
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

            if (rows == 0)
            {
                TempData["Application"] = "No application has been accepted/submitted for this job yet.";
            }


            if (job.Completed == true)
            {
                TempData["Completed"] = "Job already completed";
            }

            ViewBag.Job = db.Jobs.Where(j => j.JobID == id);
            ViewBag.JobID = id;
            ViewBag.JobStart = Convert.ToDateTime(job.StartDate);
            ViewBag.JobEnd = Convert.ToDateTime(job.EndDate);
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", job.SuburbID);
            return View(job);

        }

        //POST: Jobs/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employer")]
        public ActionResult Update([Bind(Include = "JobID,Completed,StartDate, EndDate, EndTime, StartTime, DateAdvertised, Title, Description, AddressLine1, Rate, SuburbID")]Job job)
        {
            Job a = db.Jobs.Find(job.JobID);

            a.Completed = job.Completed;


            if (ModelState.IsValid)
            {
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.Job = db.Jobs.Where(j => j.JobID == job.JobID);
            ViewBag.JobID = job.JobID;
            ViewBag.SuburbID = new SelectList(db.Suburbs, job.SuburbID);
            return View(a);


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

        public JsonResult Calculate(int id, DateTime start, DateTime end, DateTime sTime, DateTime eTime)
        {
            Skill sk = db.Skills.Find(id);

            var st = Convert.ToDouble(sTime.Hour);
            var et = Convert.ToDouble(eTime.Hour);
            double totDays = 0;
            if (end != start)
            {
                var days = end - start;
                totDays = Convert.ToDouble(days.Days);

            }
            else
            {
                totDays = 1;
            }

            var hours = et - st;

            var PayPerDay = sk.Recom_Rate * hours;
            double jobPay = PayPerDay * totDays;
            var skill = sk.SkillDescription;
            var result = new { rate = sk.Recom_Rate, payPerDay = PayPerDay, JobPay = jobPay };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        // GET: Jobs/Create
        [Authorize(Roles = ("Admin, Employer"))]
        public ActionResult Create()
        {
            var skills = db.Skills.ToList().OrderBy(s => s.SkillDescription);
            ViewBag.Skills = skills;
            var id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);
            var minDate = DateTime.Now.AddDays(3);
            ViewBag.minMonth = minDate.Month;
            ViewBag.minDay = minDate.Day;
            ViewBag.minYear = minDate.Year;
            ViewBag.stringDate = minDate.ToString();

            ViewData["User"] = user;
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.SkillID = new SelectList(db.Skills.OrderBy(x => x.SkillDescription), "SkillID", "SkillDescription");
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(s => s.SubName), "SuburbID", "SubName", user.SuburbID );
            Job job = new Job();
            job.AddressLine1 = user.AddressLine1;
            job.AddressLine2 = user.AddressLine2;
            ViewData["Job"] = job;
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ("Admin, Employer"))]
        public ActionResult Create([Bind(Exclude = "EmployerID, DateAdvertised", Include = "JobID,Title,StartDate, EndDate,SkillID,Description,StartTime,EndTime,Rate,SuburbID, AddressLine1,AddressLine2")] Job job)
        {
            job.EmployerID = User.Identity.GetUserId();

            //var sDate = job.StartDate;
            job.DateAdvertised = DateTime.UtcNow;
            job.ExpiryDate = job.StartDate.AddDays(-1);

            Notification notice = new Notification();
            var skill = job.SkillID;
            var skillName = db.Skills.Where(x => x.SkillID == skill).Select(x => x.SkillDescription);
            var workers = db.WorkerSkills.Where(x => x.SkillID == skill);
            var Name = skillName.FirstOrDefault();
            var state = ModelState;

            if (ModelState.IsValid)
            {
                foreach (var item in workers)
                {
                    notice.Message = "A new job for " + Name + " has been advertised";
                    notice.User = item.WorkerID;
                    notice.Updated = DateTime.UtcNow;
                    notice.Job = job.JobID;
                    db.Notifications.Add(notice);
                }

                db.Jobs.Add(job);
                db.SaveChanges();
                TempData["JobSuccess"] = "Job Successfully created";
                return RedirectToRoute("Default", new { controller = "Jobs", action = "Index", id = User.Identity.GetUserId() });
            }

            var skills = db.Skills.ToList().OrderBy(s => s.SkillDescription);
            ViewBag.EmployerName = User.Identity.Name;
            ViewBag.Skills = skills;
            ViewBag.SkillID = new SelectList(db.Skills.OrderBy(x => x.SkillDescription), "SkillID", "SkillDescription", job.SkillID);
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(s => s.SubName), "SuburbID", "SubName", job.SuburbID);
            TempData["JobSuccess"] = "Job could not be created. Please try again.";
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {


            ViewBag.jobSkills = new SelectList(db.Skills, "SkillID", "SkillDescription");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            #region minDate
            var minDate = DateTime.Now.AddDays(3);
            ViewBag.minMonth = minDate.Month;
            ViewBag.minDay = minDate.Day;
            ViewBag.minYear = minDate.Year;
            #endregion

            #region defaultDate
            var endDate = Convert.ToDateTime(job.EndDate);
            ViewBag.eMonth = endDate.Month.ToString();
            ViewBag.eDay = endDate.Day.ToString();
            ViewBag.eYear = endDate.Year.ToString();

            var startDate = Convert.ToDateTime(job.StartDate);
            ViewBag.sMonth = startDate.Month;
            ViewBag.sDay = startDate.Day;
            ViewBag.sYear = startDate.Year;
            #endregion

            #region DefaultTime
            ViewBag.endTime = job.EndTime.ToShortTimeString();
            //ViewBag.endHour = endTime.Hour;
            //ViewBag.endMinute = endTime.Minute;

            ViewBag.startTime = job.StartTime.ToShortTimeString();
            //ViewBag.startHour = startTime.Hour;
            //ViewBag.startMinute = startTime.Minute;
            #endregion

            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", job.SuburbID);
            ViewBag.JobID = id;

            return View(job);

        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "DateAdvertised", Include = "JobID,Title,DateAdvertised,SkillID,Description,StartTime,EndTime,Rate,SuburbID, AddressLine1,AddressLine2,StartDate, EndDate")] Job job)
        {
            Job j = db.Jobs.Find(job.JobID);

            j.Description = job.Description;
            j.StartTime = job.StartTime;
            j.StartDate = job.StartDate;
            j.EndDate = job.EndDate;
            j.EndTime = job.EndTime;
            j.Rate = job.Rate;
            j.SuburbID = job.SuburbID;
            j.AddressLine1 = job.AddressLine1;
            j.AddressLine2 = job.AddressLine2;

            if (ModelState.IsValid)
            {
                db.Entry(j).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
            }


            ViewBag.jobSkills = new SelectList(db.Skills, "SkillID", "SkillDescription");
            ViewBag.JobID = j.JobID;
            return View(job);
        }


        public JsonResult Delete(int id)
        {
            bool result = false;

            Job job = db.Jobs.Find(id);
            if (job != null)
            {
                db.Jobs.Remove(job);
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

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
using Exposure.Web.Controllers;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Exposure.Web.Controllers
{
    [Authorize(Roles ="Admin, Employer")]
    public class EmployersController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Employers
        public ActionResult Index()
        {
            var employers = db.Employers.Include(e => e.ApplicationUser).Include(e => e.Suburb);
            return View(employers.ToList());
        }

        public ActionResult Dashboard(int page=1, int pageSize = 5)
        {
            var userID = User.Identity.GetUserId();
            var currentDate = DateTime.UtcNow;
            var ratingCount = db.Reviews.Where(x => x.Reviewee == userID).Count();
            double avgRating = 0;
            if (ratingCount != 0)
            {
                 avgRating = db.Reviews.Where(x => x.Reviewee == userID).Average(x => x.Rating);                 
            }          

            var totalJobs = db.Jobs.Where(x => x.EmployerID == userID).Where(x => x.Completed == true).Count();
            var upcomingJob = db.Jobs.Where(x => x.EmployerID == userID).Where(x => x.Completed == false).OrderByDescending(x => x.StartDate).First();
            var reviews = db.UserReviews.Where(u => u.Review.Reviewee == userID).Include(u => u.Review).Include(u => u.ApplicationUser).OrderByDescending(x => x.Review.ReportDate);
            if(reviews.Count() == 0)
            {
                TempData["Reviews"] = "There are currently no reviews to display";
            }
            var currentJob = db.JobApplications.Include(x => x.Job).Where(x => x.Job.EmployerID == userID).Where(x => x.Job.StartDate <= currentDate && x.Job.EndDate >= currentDate).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == false);
            if (currentJob.Count() == 0)
            {
                TempData["currentJob"] = "You dont have any active jobs";
            }
            var TotalSpent = db.Jobs.Where(x => x.EmployerID == userID).Where(x => x.Completed == true).Sum(x => x.Rate);
            var monthAvg = db.Jobs.Where(x => x.EmployerID == userID).Where(x => x.Completed == true).Average(x => x.Rate);
            PagedList<UserReviews> model = new PagedList<UserReviews>(reviews, page, pageSize);

            if (TotalSpent == 0)
            {
                ViewBag.TotalSpent = 0;
            }
            else
            {
                ViewBag.TotalSpent = TotalSpent;
            }

            if (monthAvg == 0)
            {
                ViewBag.AVG = 0;
            }
            else
            {
                ViewBag.AVG = monthAvg;
            }

            if(totalJobs== 0)
            {
                TempData["totalJobs"] = "You have not advertised any Jobs yet.";
            }

            ViewBag.TotalJobs = totalJobs;
            ViewData["upcomingJob"] = upcomingJob;
            ViewBag.UpcomingJob = upcomingJob;
            ViewBag.Reviews = reviews;
            ViewBag.AvgRating = avgRating;
            ViewBag.CurrentJob = currentJob;

            return View(model);
        }

        // GET: Employers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // GET: Employers/Create
        
        [Authorize(Roles ="Employer, Admin")]
        public ActionResult Create()
        {
            ViewBag.EmployerID = User.Identity.GetUserId();
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName");
            return View();
        }

        // POST: Employers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployerID,WorkName,WorkNumber,WorkAddress1,WorkAddress2,SuburbID")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Employers.Add(employer);
                db.SaveChanges();
                TempData["WorkDetails"] = "Work details successfully updated";
                return RedirectToRoute("Default", new {controller = "Manage", action="Index" });
            }

            
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", employer.SuburbID);
            TempData["WorkDetails"] = "Work details could not be updated. Please enter valid details";
            return View(employer);
        }

        // GET: Employers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employer = User.Identity.GetUserId();
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", employer.SuburbID);
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployerID,WorkName,WorkNumber,WorkAddress1,WorkAddress2,SuburbID")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToRoute("Default", new {controller="Manage", action="Index" });
            }
            
            ViewBag.SuburbID = new SelectList(db.Suburbs, "SuburbID", "SubName", employer.SuburbID);
            return View(employer);
        }

        // GET: Employers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Employer employer = db.Employers.Find(id);
            db.Employers.Remove(employer);
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

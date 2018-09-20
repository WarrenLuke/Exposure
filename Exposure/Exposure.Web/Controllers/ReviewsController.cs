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
    public class ReviewsController : Controller
    {

        private IdentityDb db = new IdentityDb();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Job);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize(Roles = "Worker, Employer")]
        public ActionResult Create(int job, string employer, string worker)
        {
            var userID = User.Identity.GetUserId();
            if (employer != null)
            {
                var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser).Where(e => e.EmployerID == employer);
                ViewBag.JList = jobs;
            }
            else
            {
                var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser).Where(e => e.EmployerID == userID);
                ViewBag.JList = jobs;

            }
            var workers = db.JobApplications.Include(j => j.Job).Include(j => j.Worker).Include(j => j.Worker.ApplicationUser).Include(w => w.Worker).Where(j => j.JobID == job).Where(w => w.Response == Reply.Hired);

            var reviews = db.UserReviews.Include(x => x.Review).Where(x=>x.UserID == userID);
            var result = false;
            foreach (var item in reviews)
            {
                if (employer != null)
                {
                    if(item.Review.Reviewee == worker && item.Review.JobID==job)
                    {
                        result = true;
                    }
                }
                else
                {
                    if (item.Review.Reviewee == employer && item.Review.JobID == job)
                    {
                        result = true;
                    }
                }
            }

            if(result== true)
            {
                TempData["ReviewTrue"] = "You have already reviewed this job.";
            }

            ViewBag.JobID = job;
            ViewBag.WList = workers;

            Review model = new Review();
            model.JobID = job;

            return View(model);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ReportDate, ", Include = "ReviewID,Rating,Comment, JobID, Reviewee")] Review review, int? job, string employer)
        {

            review.ReportDate = DateTime.UtcNow;
            //review.Reviewee = Reviewee;
            UserReviews rv = new UserReviews();
            rv.ReviewID = review.ReviewID;
            rv.UserID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.UserReviews.Add(rv);
                db.SaveChanges();
                TempData["ReviewSuccess"] = "Review successfully submitted";
                return RedirectToRoute("Default", new { controller = "Jobs", action = "Index" });
            }

            var userID = User.Identity.GetUserId();
            if (employer != null)
            {
                var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser).Where(e => e.EmployerID == employer);
                ViewBag.JList = jobs;
            }
            else
            {
                var jobs = db.Jobs.Where(w => w.JobID == job).Include(e => e.Employer).Include(e => e.Employer.ApplicationUser).Where(e => e.EmployerID == userID);
                ViewBag.JList = jobs;

            }
            var workers = db.JobApplications.Include(j => j.Job).Include(j => j.Worker).Include(j => j.Worker.ApplicationUser).Include(w => w.Worker).Where(j => j.JobID == job).Where(w => w.Response == Reply.Hired);

            ViewBag.WList = workers;
            return View(review);
        }


        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", review.JobID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,Rating,Comment,ReportDate,JobApplicationID")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", review.JobID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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

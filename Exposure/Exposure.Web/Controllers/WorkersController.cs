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
    public class WorkersController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Workers
        public ActionResult Dashboard(int page =1, int pageSize= 5)
        {
            var userID = User.Identity.GetUserId();
            var currentDate = DateTime.UtcNow;
            var averageRating = db.Reviews.Where(x => x.Reviewee == userID).Average(x => x.Rating);
            var totalJobs = db.JobApplications.Where(x => x.WorkerID == userID).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == true).Count();
            var lastJob = db.JobApplications.Include(x => x.Job).Where(x => x.WorkerID == userID).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == true).OrderByDescending(x => x.Job.EndDate).FirstOrDefault(); ;
            var currentJob = db.JobApplications.Include(x => x.Job).Where(x => x.WorkerID == userID).Where(x => x.Job.StartDate <= currentDate && x.Job.EndDate >= currentDate).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == false);
            var reviews = db.UserReviews.Where(u => u.Review.Reviewee == userID).Include(u => u.Review).Include(u => u.ApplicationUser).OrderByDescending(x=>x.Review.ReportDate);
            PagedList<UserReviews> model = new PagedList<UserReviews>(reviews, page, pageSize);            
            var ETD = db.JobApplications.Where(x => x.WorkerID == userID).Where(x => x.Job.Completed == true).Where(x => x.Response == Reply.Hired).Sum(x => x.Job.Rate);
            var avg = db.JobApplications.Where(x => x.WorkerID == userID).Where(x => x.Job.Completed == true).Where(x => x.Response == Reply.Hired).Average(x => x.Job.Rate);

            if (ETD == 0)
            {
                ViewBag.ETD = 0;
            }
            else
            {
                ViewBag.ETD = ETD;
            }

            if (avg == 0)
            {
                ViewBag.AVG = 0;
            }
            else
            {
                ViewBag.AVG = avg;
            }

            if (currentJob.Count() == 0)
            {
                TempData["currentJob"] = "You currently dont have any active job";
            }

            
            ViewBag.AvgRating = averageRating;
            ViewBag.TotalJobs = totalJobs;
            ViewData["lastJob"] = lastJob;
            ViewData["currentJob"] = currentJob;
            ViewBag.CurrentJob = currentJob;
            ViewBag.Reviews = reviews;

            return View(model);
        }
    }
}
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
    public class WorkersController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Workers
        public ActionResult Dashboard(DateTime? startDate, DateTime? endDate)
        {
            var userID = User.Identity.GetUserId();

            var averageRating = db.Reviews.Where(x => x.Reviewee == userID).Average(x => x.Rating);
            var totalJobs = db.JobApplications.Where(x => x.WorkerID == userID).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == true).Count();
            var lastJob = db.JobApplications.Where(x => x.WorkerID == userID).Where(x => x.Response == Reply.Hired).Where(x => x.Job.Completed == true).OrderByDescending(x => x.Job.EndDate).FirstOrDefault(); ;

            #region EarningChart

            List<string> dates = new List<string>();
            List<double> payments = new List<double>();
            var jobs = db.JobApplications.Include(x => x.Job).Where(x => x.WorkerID == userID);

            if (startDate == null || endDate == null)
            {
                startDate = DateTime.UtcNow.AddDays(-30);
                endDate = DateTime.UtcNow;
            }

            while (startDate <= endDate)
            {
                var dateString = startDate.Value.ToShortDateString();
                dates.Add(dateString);
                startDate = startDate.Value.AddDays(1);
            }
            

            //foreach(var item in dates)
            //{
            //    var d = Convert.ToDateTime(item);
            //    payments.Add(jobs.Where(x => x.Job.EndDate == d).Select(x=>x.Job.Rate).First());
            //}

            var jobPay = payments;
            var jobDates = dates;
            ViewBag.JobPay = jobPay;
            ViewBag.JobDates = jobDates;
            #endregion

            ViewBag.AvgRating = averageRating;
            ViewBag.TotalJobs = totalJobs;
            ViewBag.LastJob = lastJob;

            return View();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Exposure.Entities;
using Exposure.Web.DataContexts;
using Microsoft.AspNet.Identity;
using PagedList;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Exposure.Web.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Incidents
        public ActionResult Index(int page=1, int pageSize=10)
        {
            var userId = User.Identity.GetUserId();
            //var jobApplications = db.JobApplications.Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged.Equals(false));
            //var userId = User.Identity.GetUserId();
            //Worker worker = db.Workers.Find(userId);

            //var jobHistory = db.JobApplications.Include(j => j.Job);

            //if (User.IsInRole("Worker"))
            //{
            //    var userID = User.Identity.GetUserId();
            //    jobApplications = jobApplications.Where(w => w.Worker.WorkerID.Equals(userID)).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Include(s => s.Job.Skill).Where(f => f.Flagged == false).Where(x => x.Job.Completed == true);
            //}
            //if (User.IsInRole("Employer"))
            //{
            //    var userID = User.Identity.GetUserId();
            //    jobApplications = jobApplications.Where(e => e.Job.Employer.EmployerID.Equals(userID)).Include(w => w.Worker).Include(j => j.Job).Include(w => w.Worker).Include(e => e.Job.Employer).Where(f => f.Flagged == false).Where(x => x.Job.Completed == true);
            //}
            var jobs = db.JobApplications.Include(x => x.Job).Where(x => x.Job.Completed == true).OrderByDescending(x=>x.Job.EndDate);

            if (User.IsInRole("Employer"))
            {
                jobs = jobs.Where(x => x.Job.EmployerID == userId).Where(x=>x.Flagged==false).OrderByDescending(x => x.Job.EndDate);
            }
            else if(User.IsInRole("Worker"))
            {
                jobs = jobs.Where(x => x.WorkerID == userId).Where(x=>x.Response == Reply.Hired).OrderByDescending(x => x.Job.EndDate);
            }

            
            var incident = db.UserIncidents.Include(x => x.Incident).Include(x=>x.Incident.Job);

            ViewBag.Incidents = incident;


            return View(jobs);
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var incident = db.UserIncidents.Where(x => x.IncidentID == id).Include(x => x.Incident.Job);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.Incident = incident;
            return View();
        }

        // GET: Incidents/Create
        [Authorize(Roles = "Worker, Employer")]
        public ActionResult Create(int job, string userID)
        {
            var user = User.Identity.GetUserId();
            
            var jobs = db.Jobs.Include(s => s.Suburb).Where(j => j.JobID == job);
            var jobApps = db.JobApplications.Include(w => w.Worker).Where(w => w.Response == Reply.Hired).Where(j=>j.JobID == job);
            var incidents = db.UserIncidents.Include(x => x.Incident).Where(x => x.UserID == user);
            var result = false;

            foreach(var item in incidents)
            {
                if(item.Incident.OffenderID==userID && item.Incident.JobID==job)
                {
                    result = true;
                    break;
                }
            }

            if (result == true)
            {
                TempData["Incident"] = "You have already reported an incident for this job. Out Admin will contact you if there is any updates.";
            }

            Incident model = new Incident();
            model.JobID = job;
            model.OffenderID = userID;

            ViewBag.Job = job;
            ViewBag.Apps = jobApps;
            
            return View(model);
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude="ReportDate",Include = "IncidentID,JobID,Description,OffenderID,Progress")] Incident incident)
        {
            incident.ReportDate = DateTime.UtcNow;
            UserIncidents ui = new UserIncidents();
            ui.UserID = User.Identity.GetUserId();
            ui.IncidentID = incident.IncidentID;

            Notification notice = new Notification();
            var offender = db.Users.Find(incident.OffenderID);

            if (ModelState.IsValid)
            {
                notice.Message = "An incident has been logged against you. Our admin will be in contact with you soon.";
                notice.User = incident.OffenderID;
                notice.Updated = DateTime.UtcNow;
                notice.inc = incident.IncidentID;

                db.Notifications.Add(notice);
                db.Incidents.Add(incident);
                db.UserIncidents.Add(ui);
                db.SaveChanges();

                TempData["IncidentReport"] = "Incident reported successfully. Please allow a couple of days for admin to look into it.";

                await EmailIncidentReport(incident.OffenderID);

                return RedirectToRoute("Default", new { controller = "Incidents", action = "Index" });
            }

            //ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", incident.JobApplicationID);
            return View(incident);
        }



        public JsonResult Update(int incidentID, Prog progress)
        {
            Incident incident = db.Incidents.Find(incidentID);
            incident.Progress = progress;

            bool result = false;

            if (incident != null)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                result = true;
            }
            //ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", incident.JobApplicationID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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

        public async Task<ActionResult> EmailIncidentReport(string ID)
        {
            var user = db.Users.Find(ID);
            var FromEmail = "admin@exposure.com";
            var apiKey = ConfigurationManager.AppSettings["Exposure"];
            var client = new SendGridClient(apiKey);
            var body = "Dear " + user.FirstName + "&nbsp;" + user.LastName +
                ",<br/> Please be advised that an incident has been reported against you regarding a recent job that you have performed.<br/>" +
                "One of our assistants will contact you over the next couple of days with more details.";
            var subject = "Incident Reported";
            var from = new EmailAddress(FromEmail, "Admin(Exposure)");
            var to = new EmailAddress(user.Email);
            var plainTextContent = body;
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (client != null)
            {
                await client.SendEmailAsync(msg);

            }
            else
            {
                Trace.TraceError("Failed to create web transport");
                await Task.FromResult(0);

            }

            return RedirectToAction("Index");
        }
    }
}

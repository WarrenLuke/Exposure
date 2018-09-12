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
    public class IncidentsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Incidents
        public ActionResult Index(int page=1, int pageSize=10)
        {
            var incidents = db.Incidents.Include(x => x.UserIncidents).Include(x => x.Job);
            PagedList<Incident> model = new PagedList<Incident>(incidents, page, pageSize);
            ViewBag.Incident = model;

            return View(model);
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
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

        // GET: Incidents/Create
        [Authorize(Roles = "Worker, Employer")]
        public ActionResult Create(int job)
        {
            var userID = User.Identity.GetUserId();

            

            var jobs = db.Jobs.Include(s => s.Suburb).Where(j => j.JobID == job);
            var jobApps = db.JobApplications.Include(w => w.Worker).Where(w => w.Response == Reply.Hired).Where(j=>j.JobID == job);

            Incident model = new Incident();
            model.JobID = job;

            ViewBag.Job = job;
            ViewBag.Apps = jobApps;
            
            return View(model);
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude="ReportDate",Include = "IncidentID,JobID,Description,OffenderID,Progress")] Incident incident)
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
                notice.incident = incident.IncidentID;

                db.Notifications.Add(notice);
                db.Incidents.Add(incident);
                db.UserIncidents.Add(ui);
                db.SaveChanges();
                TempData["IncidentReport"] = "Incident reported successfully. Please allow a couple of days for admin to look into it.";
                return RedirectToRoute("Default", new { controller = "Jobs", action="Index" });
            }

            //ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", incident.JobApplicationID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
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
            //ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", incident.JobApplicationID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentID,JobApplicationID,Description,Reporter,Offender,Progress")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.JobApplicationID = new SelectList(db.JobApplications, "JobApplicationID", "Motivation", incident.JobApplicationID);
            return View(incident);
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
    }
}

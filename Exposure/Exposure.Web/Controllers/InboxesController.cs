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
    public class InboxesController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Inboxes
        public ActionResult Index(int page =1, int pageSize = 10)
        {
            var userID = User.Identity.GetUserId();
            var conversations = db.Inboxes.Include(x=>x.ApplicationUser).Where(x => x.Receiver == userID).Select(x => x.Sender).Distinct(); ;
            //PagedList<Inbox> model = new PagedList<Inbox>(conversations, page, pageSize);
            ViewBag.Conversations = conversations;

            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach(var item in conversations)
            {
                var appUser = db.Users.Find(item);
                users.Add(appUser);
            }

            var u = users;
            ViewBag.Users = u;
           
            
            return View(conversations);
        }

        public ActionResult Conversation(string id)
        {
            var user = User.Identity.GetUserId();
            ApplicationUser receiver = db.Users.Find(user);
            ApplicationUser sender = db.Users.Find(id);

            var messages = db.Inboxes.Where(x => x.Receiver == user).Where(x => x.Sender == id);

            ViewBag.CurrentUser = user;
            ViewBag.Receiver = receiver;
            ViewBag.Sender = sender;
            ViewBag.Messages = messages;

            return View(messages);
        }

        // GET: Inboxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inbox inbox = db.Inboxes.Find(id);
            if (inbox == null)
            {
                return HttpNotFound();
            }
            return View(inbox);
        }

        // GET: Inboxes/Create
        public JsonResult SendMessage(string receiver, string message)
        {
            Inbox inbox = new Inbox();
            inbox.Message = message;
            inbox.Sender = User.Identity.GetUserId();
            inbox.Receiver = receiver;
            inbox.Date = DateTime.UtcNow;
            var result = false;
            try
            {
                db.Inboxes.Add(inbox);
                db.SaveChanges();
                result = true;
            }
            catch { }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Inboxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InboxID,Sender,Receiver,Message,Date")] Inbox inbox)
        {
            if (ModelState.IsValid)
            {
                db.Inboxes.Add(inbox);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inbox);
        }

        // GET: Inboxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inbox inbox = db.Inboxes.Find(id);
            if (inbox == null)
            {
                return HttpNotFound();
            }
            return View(inbox);
        }

        // POST: Inboxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InboxID,Sender,Receiver,Message,Date")] Inbox inbox)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inbox).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inbox);
        }

        // GET: Inboxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inbox inbox = db.Inboxes.Find(id);
            if (inbox == null)
            {
                return HttpNotFound();
            }
            return View(inbox);
        }

        // POST: Inboxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inbox inbox = db.Inboxes.Find(id);
            db.Inboxes.Remove(inbox);
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

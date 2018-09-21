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
using PagedList;

namespace Exposure.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SkillsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Skills
        public ActionResult Index(int page =1, int pageSize = 10 )
        {
            var skills = db.Skills.OrderBy(x=>x.SkillDescription).ToList();
            PagedList<Skill> model = new PagedList<Skill>(skills, page, pageSize);

            ViewBag.Skills = model;

            return View(model);
        }

        // GET: Skills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // GET: Skills/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Skills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillID,SkillDescription,Recom_Rate")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(skill);
        }

        // GET: Skills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            return View(skill);
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillID,SkillDescription,Recom_Rate")] Skill skill)
        {
            Skill sk = db.Skills.Find(skill.SkillID);
            sk.Recom_Rate = skill.Recom_Rate;
            sk.SkillDescription = skill.SkillDescription;
            if (ModelState.IsValid)
            {
                db.Entry(sk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(skill);
        }
       
        

        // POST: Skills/Delete/5
        public JsonResult Delete(int id)
        {
            bool result = false;

            Skill skill = db.Skills.Find(id);
            if (skill!= null)
            {
                db.Skills.Remove(skill);
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

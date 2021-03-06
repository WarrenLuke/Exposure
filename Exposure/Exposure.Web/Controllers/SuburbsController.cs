﻿using System;
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
    public class SuburbsController : Controller
    {
        private IdentityDb db = new IdentityDb();

        // GET: Suburbs
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var suburbs = db.Suburbs.Include(s => s.City).OrderBy(x=>x.SubName);
            var suburbsList = suburbs.ToList();

            PagedList<Suburb> model = new PagedList<Suburb>(suburbsList, page, pageSize);

            ViewBag.Suburbs = model;

            return View(model);
        }

        // GET: Suburbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suburb suburb = db.Suburbs.Find(id);
            if (suburb == null)
            {
                return HttpNotFound();
            }
            return View(suburb);
        }

        // GET: Suburbs/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            return View();
        }

        // POST: Suburbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SuburbID,SubName,CityID")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {
                db.Suburbs.Add(suburb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", suburb.CityID);
            return View(suburb);
        }

        // GET: Suburbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suburb suburb = db.Suburbs.Find(id);
            if (suburb == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", suburb.CityID);
            return View(suburb);
        }

        // POST: Suburbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SuburbID,SubName,CityID")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suburb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", suburb.CityID);
            return View(suburb);
        }

       

        // POST: Suburbs/Delete/5        
        public JsonResult Delete(int id)
        {
            var result = false;
            Suburb suburb = db.Suburbs.Find(id);
            if(suburb !=null)
            {
                db.Suburbs.Remove(suburb);
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

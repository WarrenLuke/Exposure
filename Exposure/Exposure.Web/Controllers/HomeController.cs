using Exposure.Web.DataContexts;
using Exposure.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exposure.Web.Controllers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Exposure.Entities;

namespace Exposure.Web.Controllers
{
    public class HomeController : Controller
    {
        private IdentityDb db = new IdentityDb();


        public ActionResult Index()
        {

            ViewBag.Users = db.Users.Where(f => f.Status == null).Count();
            ViewBag.Jobs = db.Jobs.Where(c => c.Completed == false).Count();
            ViewBag.SJobs = db.Jobs.Where(c => c.Completed == true).Where(c => c.EndDate >= DbFunctions.AddDays(DateTime.UtcNow, -30)).Count();

            return View();
        }

        public ActionResult Dashboard()
        {
            #region jobs
            var jlist = db.Jobs.Include(x => x.Skill);
            List<int> repartitions = new List<int>();
            var skills = db.Skills.Select(x => x.SkillDescription).Distinct();
            var jCount = jlist.Count();

            foreach (var item in skills)
            {
                repartitions.Add(jlist.Count(x => x.Skill.SkillDescription == item));
            }
            var rep = repartitions;
            ViewBag.Skills = skills;
            ViewBag.Reps = rep;

            #endregion

            #region UserRoles
            IdentityRole wRole = db.Roles.First(r => r.Name == "Worker");
            int wCount = db.Set<IdentityUserRole>().Count(r => r.RoleId == wRole.Id);
            IdentityRole eRole = db.Roles.First(r => r.Name == "Employer");
            int eCount = db.Set<IdentityUserRole>().Count(r => r.RoleId == eRole.Id);
            List<int> uReps = new List<int>();

            uReps.Add(wCount);
            uReps.Add(eCount);
            ViewBag.WCount = wCount;
            ViewBag.ECount = eCount;

            var ureps = uReps;
            ViewBag.UserReps = ureps;
            #endregion

            #region JobApplication
            var appList = db.JobApplications;
            List<int> appRepartitions = new List<int>();
            List<Reply> replies = new List<Reply>();
            var aCount = appList.Count();

            foreach(var item in replies)
            {
                appRepartitions.Add(appList.Count(x => x.Response == item));
            }

            var appReps = appRepartitions;
            ViewBag.Replies = replies;
            ViewBag.AppReps = appReps;
            #endregion

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public FileContentResult ProfilePic(string Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Id == null)
                {
                    Id = User.Identity.GetUserId();
                }


                var bdUsers = HttpContext.GetOwinContext().Get<IdentityDb>();
                var userImage = bdUsers.Users.Where(x => x.Id == Id).FirstOrDefault();


                if (userImage.ProfilePic == null)
                {
                    var user = db.Users.Find(Id);
                    string fileName = null;

                    if (user.Gender == "Male")
                    {
                        fileName = HttpContext.Server.MapPath(@"~/Images/Male.jpg");
                    }
                    else
                    {
                        fileName = HttpContext.Server.MapPath(@"~/Images/Female.jpg");
                    }

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    return File(imageData, "image/jpeg");

                }
                return new FileContentResult(userImage.ProfilePic, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");
            }
        }
    }
}

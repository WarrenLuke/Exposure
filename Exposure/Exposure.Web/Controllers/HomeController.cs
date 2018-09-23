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
using System.Net;
using System.Net.Mail;
using Exposure.Web.Controllers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Exposure.Entities;
using Twilio.Jwt.AccessToken;
using System.Threading.Tasks;
using Exposure.Web.Models.ViewModels;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;

namespace Exposure.Web.Controllers
{
    public class HomeController : Controller
    {
        private IdentityDb db = new IdentityDb();

        public ActionResult Index()
        {
            IdentityRole adRole = db.Roles.First(r => r.Name == "Admin");
            int users = db.Set<IdentityUserRole>().Count(r => r.RoleId != adRole.Id);
            ViewBag.Users = users;
            ViewBag.Jobs = db.Jobs.Where(c => c.Completed == false).Count();
            ViewBag.SJobs = db.Jobs.Where(c => c.Completed == true).Where(c => c.EndDate >= DbFunctions.AddDays(DateTime.UtcNow, -30)).Count();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard(DateTime? toDate, DateTime? frmDate, string viewBy, DateTime? startDate, DateTime? endDate)
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
            ViewBag.jCount = jCount;

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
            replies.Add(Reply.Hired);
            replies.Add(Reply.Pending);
            replies.Add(Reply.Rejected);
            var aCount = appList.Count();

            foreach (var item in replies)
            {
                appRepartitions.Add(appList.Count(x => x.Response == item));
            }

            var appReps = appRepartitions;
            var reply = replies;
            ViewBag.Replies = reply;
            ViewBag.AppReps = appReps;
            ViewBag.ACount = aCount;
            #endregion

            #region BarChart
            var jobs = db.Jobs.Include(s => s.Skill);
            DateTime date = DateTime.UtcNow;

            if (toDate != null || frmDate != null)
            {
                jobs = jobs.Where(x => x.DateAdvertised >= frmDate && x.DateAdvertised <= toDate);
            }

            var dates = db.Jobs.Select(x => x.DateAdvertised).Distinct();
            List<int> barJobs = new List<int>();

            List<string> dateReps = new List<string>();
            foreach (var item in dates)
            {
                dateReps.Add(item.ToShortDateString());
            }

            foreach (var item in dates)
            {
                barJobs.Add(jobs.Count(x => x.DateAdvertised == item));
            }

            var jobList = barJobs;
            var dReps = dateReps;
            ViewBag.Dates = dReps;
            ViewBag.Count = jobList;
            #endregion

            #region SiteActivity  



            List<string> regDateReps = new List<string>();
            if (startDate == null || endDate == null)
            {
                startDate = DateTime.UtcNow.AddDays(-30);
                endDate = DateTime.UtcNow;
            }

            while (startDate <= endDate)
            {
                var dateString = startDate.Value.ToShortDateString();
                regDateReps.Add(dateString);
                startDate = startDate.Value.AddDays(1);
            }


            var Workers = db.Users.Include(x => x.Roles).Where(x => x.Roles.FirstOrDefault().RoleId == wRole.Id);
            List<int> workReg = new List<int>();
            foreach (var item in regDateReps)
            {
                var i = Convert.ToDateTime(item);
                workReg.Add(Workers.Count(x => x.RegDate == i));
            }

            var Employers = db.Users.Include(x => x.Roles).Where(x => x.Roles.FirstOrDefault().RoleId == eRole.Id);
            List<int> empReg = new List<int>();
            foreach (var item in regDateReps)
            {
                var d = Convert.ToDateTime(item);
                empReg.Add(Employers.Count(x => x.RegDate == d));
            }
            var emps = empReg;
            var works = workReg;
            var actReps = regDateReps;
            ViewBag.WorkAct = works;
            ViewBag.EmpAct = emps;
            ViewBag.ActDates = actReps;
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


        public ActionResult Email(string email)
        {
            
            EmailFormModel model = new EmailFormModel();

            model.ToEmail = email;            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Email(EmailFormModel model)
        {
            
            model.FromEmail = "admin@exposure.com";
            var apiKey = ConfigurationManager.AppSettings["Exposure"];
            var client = new SendGridClient(apiKey);
            var body = model.Message;
            var subject = model.Subject;
            var from = new EmailAddress(model.FromEmail, "Admin(Exposure)");
            var to = new EmailAddress(model.ToEmail);
            var plainTextContent = body;
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (client != null)
            {
                await client.SendEmailAsync(msg);
                return RedirectToAction("Index");
            }
            else
            {
                Trace.TraceError("Failed to create web transport");
                await Task.FromResult(0);
                return View(model);
            }
            
        }

        public ActionResult Help()
        {
            var faq = db.Helps;
            ViewBag.FAQ = faq;
            return View(faq);
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


        public FileContentResult Banner(int? Id)
        {

            var gb = db.GeneralBusinesses.FirstOrDefault();
            return new FileContentResult(gb.CompanyBanner, "image/jpeg");
        }

        public FileContentResult Logo(int? Id)
        {
            var gb = db.GeneralBusinesses.FirstOrDefault();
            return new FileContentResult(gb.Logo, "image/jpeg");
        }

        [HttpGet]
        public JsonResult GetNotifications()
        {
            List<Notice> notifications = new List<Notice>();
            var Id = User.Identity.GetUserId();
            var user = db.Users.Find(Id);
            var notice = db.Notifications.Where(x => x.User == Id).Where(x => x.Flagged == false).OrderByDescending(x => x.Updated);

            if (user != null)
            {
                notice = notice.Where(x => x.Updated > user.LastVisited).OrderByDescending(x => x.Updated);
            }

            foreach (var item in notice)
            {
                notifications.Add(new Notice() { msg = item.Message, updated = DateTime.Now.ToString("ss") });
            }

            return Json(notifications, JsonRequestBehavior.AllowGet);
        }


    }

    internal class Notice
    {
        public string msg;

        public string updated;
    }




}

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Exposure.Web.Models;
using System.Collections.Generic;
using Exposure.Entities;
using Exposure.Web.DataContexts;
using System.Data;
using System.Data.Entity;
using PagedList;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Exposure.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IdentityDb db = new IdentityDb();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();

            var model = new IndexViewModel
            {
                Email = UserManager.FindById(userId).Email,
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };

            if (User.IsInRole("Employer"))
            {
                model = new IndexViewModel
                {
                    Location = UserManager.FindById(userId).Employer.SuburbID,
                    WorkName = UserManager.FindById(userId).Employer.WorkName,
                    WorkAddressLine1 = UserManager.FindById(userId).Employer.WorkAddress1,
                    WorkAddressLine2 = UserManager.FindById(userId).Employer.WorkAddress2,
                    WorkNumber = UserManager.FindById(userId).Employer.WorkNumber,
                };
                var empSub = db.Employers.Include(m => m.Suburb).Where(m => m.SuburbID == model.Location);
                ViewBag.empSub = empSub;
            }
            ApplicationUser user = UserManager.FindById(userId);

            user = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                Email = user.Email,
                SuburbID = user.SuburbID,
                PhoneNumber = user.PhoneNumber,
                ProfilePic = user.ProfilePic
            };

            if (user.ProfilePic == null)
            {
                TempData["Pic"] = "Please add a picture to help other users identity you";
            }

            if (User.IsInRole("Worker"))
            {
                var skillsLst = new List<string>();

                var skillsQry = (from s in db.WorkerSkills
                                 where s.WorkerID == user.Id
                                 select s).ToList();

                var skills = db.WorkerSkills.Include(m => m.Skill).Include(m => m.Worker).OrderBy(m => m.Skill.SkillDescription).Where(m => m.WorkerID == userId);
                ViewBag.Skills = skills;
                ViewBag.JobApp = db.JobApplications.Where(j => j.Response.Value != Reply.Pending).Where(w => w.WorkerID == userId).Count();
            }

            var date = DateTime.UtcNow;
            if (User.IsInRole("Employer"))
            {
                ViewBag.JobApp = db.JobApplications.Where(j => j.Response.Value == Reply.Pending).Where(j => j.Flagged == false).Where(e => e.Job.EmployerID == userId).Where(x => x.Job.Employer.ApplicationUser.LastVisited < date).Count();
            }

            if (User.IsInRole("Worker"))
            {
                ViewBag.JobApp = db.JobApplications.Where(j => j.Response.Value == Reply.Hired || j.Response.Value == Reply.Rejected).Where(j => j.Flagged == false).Where(e => e.WorkerID == userId).Where(x => x.Worker.ApplicationUser.LastVisited < date).Count();
            }

            var uSub = user.SuburbID;
            var userSub = db.Users.Include(m => m.Suburb).Where(m => m.SuburbID == user.SuburbID).Where(u => u.Id.Equals(userId));
            var reviews = db.UserReviews.Where(u => u.Review.Reviewee == userId).Include(u => u.Review).Include(u => u.ApplicationUser);

            ViewBag.Reviews = reviews;
            ViewBag.userSub = userSub;
            ViewBag.UserID = userId;
            ViewData["user"] = user;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllUsers(string search, string userType, int page = 1, int pageSize = 10)
        {
            var userID = User.Identity.GetUserId();
            IdentityRole role;
            string RID = "00as";
            var users = db.Users.Include(w => w.Worker).Include(e => e.Employer).Where(j => j.Id != userID).OrderBy(x => x.FirstName + x.LastName).ToList();
            PagedList<ApplicationUser> model;

            if (!String.IsNullOrWhiteSpace(userType))
            {
                role = db.Roles.First(r => r.Name == userType);
                RID = role.Id;
                users = users.Where(x => x.Roles.FirstOrDefault().RoleId == RID).ToList();
            }
            

            if (search != null)
            {
                users = users.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search)).ToList();
            }

            model = new PagedList<ApplicationUser>(users, page, pageSize);
            ViewBag.Users = model;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GeneralBusiness()
        {
            var gb = db.GeneralBusinesses.Count();
            var gbID = db.GeneralBusinesses.FirstOrDefault();

            if (gb == 0)
            {
                return RedirectToRoute("Default", new { controller = "GeneralBusinesses", action = "Create" });
            }
            else
            {
                return RedirectToRoute("Default", new { controller = "GeneralBusinesses", action = "Details", id = gbID.ID });
            }
        }

        //GET: /Cities/Index
        public ActionResult Cities()
        {
            return RedirectToRoute("Defaults", new { controller = "Cities", action = "Index" });
        }

        public ActionResult SuburbsCities()
        {
            return View();
        }

        //GET: /Incidents/Index
        public ActionResult Incidents()
        {
            return RedirectToRoute("Default", new { controller = "Incidents", action = "Index" });
        }

        //GET: /Suburbs/Index
        public ActionResult Suburbs()
        {
            return RedirectToRoute("Default", new { controller = "Suburbs", action = "Index" });
        }

        //GET: /Jobs/Index
        public ActionResult Jobs()
        {
            return RedirectToRoute("Default", new { controller = "Jobs", action = "Index" });
        }


        public ActionResult ManageJobs()
        {
            return RedirectToRoute("Default", new { controller = "Jobs", action = "Index" });
        }

        public ActionResult ReportIncident()
        {
            return RedirectToRoute("Default", new { controller = "Incidents", action = "Create" });
        }

        public ActionResult UpdateWorkDetails()
        {
            var userID = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(userID);
            return RedirectToRoute("Default", new { controller = "Employers", action = "Edit", id = userID });
        }

        public ActionResult AddSkill()
        {
            var userID = User.Identity.GetUserId();
            return RedirectToRoute("Default", new { controller = "WorkerSkills", action = "Create", id = userID });
        }

        public ActionResult EditSkill(string id, int skill)
        {
            return RedirectToRoute("Default", new { controller = "WorkerSkills", action = "Edit", id = id, skill = skill });
        }

        public ActionResult EditProfile()
        {
            var userId = User.Identity.GetUserId();
            return RedirectToRoute("Default", new { controller = "Account", action = "EditProfile", id = userId });

        }

        public ActionResult JobApplications()
        {
            return RedirectToRoute("Default", new { controller = "JobApplications", action = "Index" });

        }


        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }


        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword       
        public ActionResult ChangePassword()
        {
            return View();
        }


        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}
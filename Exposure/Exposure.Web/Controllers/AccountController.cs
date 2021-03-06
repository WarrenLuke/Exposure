﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Exposure.Web.Models;
using Exposure.Web.DataContexts;
using Microsoft.AspNet.Identity.EntityFramework;
using Exposure.Entities;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Net;
using Exposure.Web.Models.ViewModels;

namespace Exposure.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IdentityDb db = new IdentityDb();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);

            //if(user != null)
            //{
            //    if(!await UserManager.IsEmailConfirmedAsync(user.Id))
            //    {
            //        string callBackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account- resend");
            //        TempData["errorMessage"] = "You must have a confirmed email to log on. The confirmation token has been resent to your email account.";
            //        return View(model);
            //    }
            //}


            if (user == null)
            {
                ModelState.AddModelError("", "Incorrect Email or/and Password");
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Incorrect Email or/and Password");
                    return View(model);
            }
        }

        [Authorize]
        public ActionResult AccountStatus(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountStatus([Bind(Include = "Id")]ApplicationUser user)
        {
            ApplicationUser u = await UserManager.FindByIdAsync(user.Id);

            if (User.IsInRole("Employer,Worker"))
            {
                user.Status = Models.Enums.Status.UserRemove;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("LogOff");

            }
            else
            {
                user.Status = Models.Enums.Status.AdminBlock;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute("User", "Manage");
            }


        }
        //GET: /Account/UpdatePicture
        public ActionResult UpdatePicture(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);

        }

        //POST: /Account/UpdatePicture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePicture([Bind(Exclude = "ProfilePic")]ApplicationUser user)
        {
            var userId = User.Identity.GetUserId();

            ApplicationUser u = await UserManager.FindByIdAsync(userId);

            byte[] imageData = null;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["ProfilePic"];
                var image = poImgFile;
                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            u.ProfilePic = imageData;
            var result = await UserManager.UpdateAsync(u);

            if (result.Succeeded)
            {
                try
                {
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["PicUpdate"] = "Profile Picture Changed Successfully";
                    return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });
                }
                catch
                {
                    db.SaveChanges();
                    TempData["PicUpdate"] = "Profile Picture Changed Successfully";
                    return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });
                }

            }
            return View(user);
        }

        public ActionResult RemovePicture()
        {
            var userId = User.Identity.GetUserId();

            ApplicationUser user = UserManager.FindById(userId);

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePicture(ApplicationUser user)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser u = await UserManager.FindByIdAsync(userId);

            u.ProfilePic = null;

            var result = await UserManager.UpdateAsync(u);

            if (result.Succeeded)
            {
                try
                {
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["PicUpdate"] = "Profile Picture Removed Successfully!";
                    return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });

                }
                catch
                {
                    db.SaveChanges();
                    TempData["PicUpdate"] = "Profile Picture Removed Successfully";
                    return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });
                }

            }

            return RedirectToRoute("Default", new { controler = "Account", action = "UpdatePicture", id = user.Id });
        }

        //GET: /Account/EditProfile
        public ActionResult EditProfile(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = UserManager.FindById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            //var user = db.Users.Include(m => m.Suburb).Where(m => m.Id == userId);
            ViewBag.Suburbs = new SelectList(db.Suburbs, "SuburbID", "SubName", user.SuburbID);


            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile([Bind(Exclude = "ProfilePic, PasswordHash, UserName, Email")]ApplicationUser model)
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = await UserManager.FindByIdAsync(userId);


            user.AddressLine1 = model.AddressLine1;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.AddressLine2 = model.AddressLine2;
            user.SuburbID = model.SuburbID;

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                try
                {
                    db.Entry(user).State = user.Id == null ? EntityState.Added : EntityState.Modified;
                }
                catch
                {
                    db.SaveChanges();
                }

                TempData["EditProfile"] = "Personal Details Successfully changed";
                return RedirectToRoute("Default", new { controller = "Manage", action = "Index" });
            }


            ViewBag.Suburbs = new SelectList(db.Suburbs, "SuburbID", "SubName", user.SuburbID);

            ViewBag.user = user;
            TempData["EditProfile"] = "Failed updating Details. Please Try Again";
            return View(model);

        }

        public ActionResult UserDetails(string Id)
        {

            var userDetails = db.Users.Include(e => e.Employer.Suburb).Include(w => w.Worker).Include(s => s.Suburb).Where(u => u.Id == Id);
            var reviews = db.UserReviews.Where(u => u.Review.Reviewee == Id).Include(u => u.Review).Include(u => u.ApplicationUser);
            var userRole = UserManager.GetRoles(Id).Single();

            if (userRole == "Worker")
            {
                ViewBag.Skills = db.WorkerSkills.Include(w => w.Worker).Include(ws => ws.Worker.WorkerSkills).Where(w => w.WorkerID == Id);
            }
            else if (userRole == "Employer")
            {
                ViewBag.Employer = db.Employers.Include(x => x.Suburb).Where(x => x.EmployerID == Id);
            }

            ViewBag.User = userDetails;
            ViewBag.Role = userRole;
            ViewBag.Reviews = reviews;
            return View();
        }


        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(x => x.SubName), "SuburbID", "SubName");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "ProfilePic")]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(x => x.SubName), "SuburbID", "SubName");

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Gender = model.Gender,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    PhoneNumber = model.PhoneNumber,
                    SuburbID = model.SuburbID,
                    RegDate = DateTime.Now,
                    LastVisited = DateTime.Now
                };

                var role = model.Role;
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Binding user to selected role by using the UserManager class
                    UserManager.AddToRole(user.Id, role);

                    //If statement used to determine which table user should be added to
                    if (role == "Employer")
                    {
                        var employer = new Employer { EmployerID = user.Id };
                        db.Employers.Add(employer);
                        db.SaveChanges();
                    }
                    else if (role == "Worker")
                    {
                        var worker = new Worker { WorkerID = user.Id };
                        db.Workers.Add(worker);
                        db.SaveChanges();
                    }

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    var callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm Your Account");

                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                     + "before you can log in.";

                    return View("VerifyEmail");

                }
                AddErrors(result);
            }
            ViewBag.SuburbID = new SelectList(db.Suburbs.OrderBy(x => x.SubName), "SuburbID", "SubName");

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser u = db.Users.Find(id);
            if (u != null)
            {
                u.LastVisited = DateTime.UtcNow;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
                "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }

        public async Task<ActionResult> EmailIncident(EmailFormModel model)
        {
            var user = UserManager.FindByEmail(model.ToEmail);
            var callbackUrl = Url.Action("Index", "Incidents");
            await UserManager.SendEmailAsync(user.Id, model.Subject, model.Message);
            TempData["EmailSent"] = "Email Successfully sent";

             return RedirectToRoute("Default", new { controller = "Incidents", action = "Index", id = user.Id });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
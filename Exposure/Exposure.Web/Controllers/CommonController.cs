using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exposure.Entities;
using Exposure.Web.DataContexts;
using Exposure.Web.Models.ViewModels;

namespace Exposure.Web.Controllers
{
    public class CommonController : Controller
    {
        private IdentityDb db = new IdentityDb();
        public ActionResult General()
        {
            GeneralBusiness gb = db.GeneralBusinesses.FirstOrDefault();

            GeneralBusinessView view = new GeneralBusinessView();

            view.CompanyBanner = gb.CompanyBanner;
            view.CompanyName = gb.CompanyName;
            view.Description = gb.Description;
            view.EmailAddress = gb.EmailAddress;
            view.FaxNo = gb.FaxNo;
            view.TelNo = gb.TelNo;
            view.Logo = gb.Logo;
            view.Suburb = gb.Suburb.SubName;



            return View(gb);
            
        }
    }
}
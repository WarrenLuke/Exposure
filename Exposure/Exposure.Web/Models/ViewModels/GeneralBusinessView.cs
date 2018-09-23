using Exposure.Entities;
using Exposure.Web.DataContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exposure.Web.Models.ViewModels
{
    public class GeneralBusinessView
    {
        private IdentityDb db = new IdentityDb();

        public GeneralBusinessView()
        {
            GeneralBusiness gb = db.GeneralBusinesses.FirstOrDefault();

            CompanyName = gb.CompanyName;
            Logo = gb.Logo;
            Slogan = gb.Slogan;
            Suburb = gb.Suburb.SubName;
            Description = gb.Description;
            FaxNo = gb.FaxNo;
            TelNo = gb.TelNo;
            EmailAddress = gb.EmailAddress;
        }

        [DisplayName("Company Banner")]
        [Required]
        public byte[] CompanyBanner { get; set; }

        [DisplayName("Logo")]
        [Required]
        public byte[] Logo { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [Required]
        public string Slogan { get; set; }

        [Required]
        public string Suburb { get; set; }

        [Required]
        public string Description { get; set; }

        [DisplayName("Fax No.")]
        [Required]
        public string FaxNo { get; set; }

        [DisplayName("Telephone No.")]
        [Required]
        public string TelNo { get; set; }

        [DisplayName("Email")]
        [EmailAddress]
        public string EmailAddress { get; set; }


    }




}
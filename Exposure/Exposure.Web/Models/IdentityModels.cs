using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Exposure.Entities;
using Exposure.Web.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Exposure.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Profile Picture")]
        [Column(TypeName = "image")]
        public byte[] ProfilePic { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(1024)]
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }

        
        [DisplayName("Suburb")]
        public int SuburbID { get; set; }

        [ForeignKey("SuburbID")]
        public virtual Suburb Suburb { get; set; }

        [ScaffoldColumn(false)]
        public Status? Status { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<UserReviews> UserReviews { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; }    
        public virtual ICollection<UserIncidents> UserIncidents { get; set; }        

        public virtual Employer Employer { get; set; }

        public virtual Worker Worker { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RegDate { get; set; }

        public DateTime LastVisited { get; set; }

        public virtual ICollection<Notification> notifications { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemPendukungKeputusan.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemPendukungKeputusan.Models.Security
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class SistemPendukungKeputusanApplicationUser : IdentityUser, IUser
    {
        public void Delete(SPKContext context)
        {
            try
            {
                context.SistemPendukungKeputusanApplicationUsers.Remove(this);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SistemPendukungKeputusanApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool Active { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }
        [UIHint("ForeignObject")]
        [Display(Name = "Application Role")]
        public int SistemPendukungKeputusanApplicationRoleId { get; set; }

        public virtual SistemPendukungKeputusanApplicationRole SistemPendukungKeputusanApplicationRole { get;set;}
        
        public SistemPendukungKeputusanApplicationUser() : base()
        {
            Active = true;
        }
    }
}
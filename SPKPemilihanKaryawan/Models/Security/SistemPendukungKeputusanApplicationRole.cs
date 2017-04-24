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
    public class SistemPendukungKeputusanApplicationRole
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Active { get; set; }

        public SistemPendukungKeputusanApplicationRole() : base()
        {
            Active = true;
        }

        #region Inquiry
        [Display(Name = "Create")]
        public bool Report_Create { get; set; }

        [Display(Name = "Delete")]
        public bool Report_Delete { get; set; }

        [Display(Name = "Edit")]
        public bool Report_Edit { get; set; }

        [Display(Name = "Navigate")]
        public bool Report_Navigate { get; set; }

        [Display(Name = "Read")]
        public bool Report_Read { get; set; }

        #endregion

        #region Master


        [Display(Name = "Create")]
        public bool Vacancy_Create { get; set; }

        [Display(Name = "Delete")]
        public bool Vacancy_Delete { get; set; }

        [Display(Name = "Edit")]
        public bool Vacancy_Edit { get; set; }

        [Display(Name = "Navigate")]
        public bool Vacancy_Navigate { get; set; }

        [Display(Name = "Read")]
        public bool Vacancy_Read { get; set; }

        [Display(Name = "Create")]
        public bool EmployeeCandidate_Create { get; set; }

        [Display(Name = "Delete")]
        public bool EmployeeCandidate_Delete { get; set; }

        [Display(Name = "Edit")]
        public bool EmployeeCandidate_Edit { get; set; }

        [Display(Name = "Navigate")]
        public bool EmployeeCandidate_Navigate { get; set; }

        [Display(Name = "Read")]
        public bool EmployeeCandidate_Read { get; set; }
        #endregion

        #region System
        [Display(Name = "Create")]
        public bool ApplicationRole_Create { get; set; }

        [Display(Name = "Delete")]
        public bool ApplicationRole_Delete { get; set; }

        [Display(Name = "Edit")]
        public bool ApplicationRole_Edit { get; set; }

        [Display(Name = "Navigate")]
        public bool ApplicationRole_Navigate { get; set; }

        [Display(Name = "Read")]
        public bool ApplicationRole_Read { get; set; }


        [Display(Name = "Create")]
        public bool ApplicationUser_Create { get; set; }

        [Display(Name = "Delete")]
        public bool ApplicationUser_Delete { get; set; }

        [Display(Name = "Edit")]
        public bool ApplicationUser_Edit { get; set; }

        [Display(Name = "Navigate")]
        public bool ApplicationUser_Navigate { get; set; }

        [Display(Name = "Read")]
        public bool ApplicationUser_Read { get; set; }

        [Display(Name = "ResetPassword")]
        public bool ApplicationUser_ResetPassword { get; set; }
        #endregion

        [Display(Name = "Is Super Admin")]
        public bool IsSuperAdmin { get; set; }

        public void Delete(SPKContext context)
        {
            try
            {
                context.SistemPendukungKeputusanApplicationRoles.Remove(this);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public List<string> GetRoles()
        {
            List<string> lst = new List<string>();
            Type type = typeof(SistemPendukungKeputusanApplicationRole);
            var properties = type.GetProperties();
            string[] excludedProperties = { "Id", "Code", "Name", "IsSuperAdmin", "Active"};

            if (!IsSuperAdmin)
            {
                foreach (var property in properties)
                    if (!excludedProperties.Contains(property.Name) && (bool)property.GetValue(this))
                        lst.Add(property.Name);
            }
            else
            {
                foreach (var property in properties)
                    lst.Add(property.Name);
            }
            return lst;
        }
    }
}

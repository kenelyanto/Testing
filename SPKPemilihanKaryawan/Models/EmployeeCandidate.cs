using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Models.Base;
using SistemPendukungKeputusan.Models.Enum;
using SPKPemilihanKaryawan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemPendukungKeputusan.Models
{
    public class EmployeeCandidate : PrioritasHierarki
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string Fullname { get; set; }

        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Identification Number")]
        public string IdentificationNumber { get; set; }
        public decimal TotalWeight { get; set; }

        [Required]
        [UIHint("ForeignObject")]
        [Display(Name = "Vacancy")]
        public int VacancyId { get; set; }

        
        public virtual Vacancy Vacancy { get; set; }
        public virtual ICollection<AHPCalculationTable> AHPCalculationTables { get; set; }


        public EmployeeCandidate()
        {
            AHPCalculationTables = new List<AHPCalculationTable>();
        }


        public void Delete(SPKContext context)
        {
            try
            {
                //if (context.SalesOrders.Where(c => c.OfficeId == Id).ToList().Count > 0)
                //    throw new InvalidOperationException("Office still being used in SalesOrders, untick the active rather than delete.");
                context.AHPCalculationTables.RemoveRange(AHPCalculationTables);
                context.EmployeeCandidates.Remove(this);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
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
    public class AHPCalculationTable 
    {
        public int Id { get; set; }

        [Required]
        [UIHint("ForeignObject")]
        [Display(Name = "Employee Candidate")]
        public int EmployeeCandidateId { get; set; }

        public string Type { get; set; }
        public Prioritas Priority { get; set; }

        public decimal VacancyWeight { get; set; }
        public decimal EmployeCandidateWeight { get; set; }
        public decimal PriorityTotalWeight { get; set; }

        public virtual EmployeeCandidate EmployeeCandidate { get; set; }

        public void DeleteById(int id)
        {
            try
            {
                SPKContext context = new SPKContext();
                var data = context.AHPCalculationTables.Find(id);
                context.AHPCalculationTables.Remove(data);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}
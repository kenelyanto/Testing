using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Models.Base;
using SPKPemilihanKaryawan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemPendukungKeputusan.Models
{
    public class Vacancy : PrioritasHierarki
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public decimal AgeWeightPriority { get; set; }
        public decimal StudyWeightPriority { get; set; }
        public decimal ExperienceWeightPriority { get; set; }
        public decimal CertificateWeightPriority { get; set; }
        public bool NeedProcess { get; set; }
        public virtual ICollection<EmployeeCandidate> EmployeeCandidates { get; set; }

        public Vacancy()
        {
            Active = true;
            EmployeeCandidates = new List<EmployeeCandidate>();
        }

        public void Delete(SPKContext context)
        {
            try
            {
                //if (context.SalesOrders.Where(c => c.OfficeId == Id).ToList().Count > 0)
                //    throw new InvalidOperationException("Office still being used in SalesOrders, untick the active rather than delete.");
                foreach(EmployeeCandidate employeeCandidate in EmployeeCandidates)
                {
                    context.AHPCalculationTables.RemoveRange(employeeCandidate.AHPCalculationTables);
                }
                context.EmployeeCandidates.RemoveRange(EmployeeCandidates);
                context.Vacancies.Remove(this);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}
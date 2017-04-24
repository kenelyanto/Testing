namespace SPKPemilihanKaryawan.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using SistemPendukungKeputusan.Models;
    using SistemPendukungKeputusan.Models.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SistemPendukungKeputusan.DAL.SPKContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SistemPendukungKeputusan.DAL.SPKContext context)

        {
            DbContextTransaction trans = context.Database.BeginTransaction();
            Report report = new Report()
            {
                Name = "SPK Report",
                FileName = "SPK.rpt",
                Query = "SELECT VC.NeedProcess, VC.Name, AHP.VacancyWeight AS BobotPrioritasVacancy, VC.StudyWeightPriority AS BobotPrioritasPendidikan, VC.ExperienceWeightPriority AS BobotPrioritasPengalaman," + Environment.NewLine + 
                        "VC.CertificateWeightPriority AS BobotPrioritasSertifikasi, VC.AgeWeightPriority AS BobotPrioritasUsia," + Environment.NewLine +
                        "EC.Fullname, AHP.EmployeCandidateWeight AS BobotPrioritasEmployeeCandidate, EC.TotalWeight AS TotalPrioritasAkhir, AHP.PriorityTotalWeight AS BobotAkhir, AHP.Type AS Tipe, EC.PHONENUMBER, EC.IdentificationNumber AS KTPNumber" + Environment.NewLine + 
                        "FROM Vacancies VC" + Environment.NewLine +
                        "INNER JOIN EmployeeCandidates EC ON EC.VacancyId = VC.Id" + Environment.NewLine +
                        "INNER JOIN AHPCalculationTables AHP ON AHP.EmployeeCandidateId = EC.Id" + Environment.NewLine +
                        "WHERE VC.Id = '[[Vacancy]]'"

            };
            if (context.Reports.FirstOrDefault(m => m.Name == report.Name) == null)
                context.Reports.Add(report);

            string email = "admin@admin.co.id";
            string userName = "admin@admin.co.id";
            string password = "wfr123321";
            string name = "Admin";

            var userStore = new UserStore<SistemPendukungKeputusanApplicationUser>(context);
            var manager = new SistemPendukungKeputusanUserManager(userStore);
            
            SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole = context.SistemPendukungKeputusanApplicationRoles.Where(m => m.Name == "Super Admin").FirstOrDefault();
            if (sistemPendukungKeputusanApplicationRole == null)
            {
                SistemPendukungKeputusanApplicationRole applicationRole = new SistemPendukungKeputusanApplicationRole() { Name = "Super Admin", IsSuperAdmin = true };
                context.SistemPendukungKeputusanApplicationRoles.Add(applicationRole);
                var userToInsert = new SistemPendukungKeputusanApplicationUser { Name = name, UserName = userName, Email = email, EmailConfirmed = true, SistemPendukungKeputusanApplicationRoleId = 1 };
                IdentityResult result = manager.Create(userToInsert, password);
            }
            trans.Commit();
        }
    }
}

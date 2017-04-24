using Microsoft.AspNet.Identity.EntityFramework;
using SistemPendukungKeputusan.Models;
using SistemPendukungKeputusan.Models.Base;
using SistemPendukungKeputusan.Models.Security;
using SPKPemilihanKaryawan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPendukungKeputusan.DAL
{
    public partial class SPKContext : DbContext
    {
        public SPKContext() : base("SPKContext")
        {

        }

        public SPKContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public virtual DbSet<EmployeeCandidate> EmployeeCandidates { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<AHPCalculationTable> AHPCalculationTables { get; set; }

        public virtual DbSet<IdentityUser> Users { get; set; }
        public virtual DbSet<SistemPendukungKeputusanApplicationRole> SistemPendukungKeputusanApplicationRoles { get; set; }

        public virtual DbSet<SistemPendukungKeputusanApplicationUser> SistemPendukungKeputusanApplicationUsers { get; set; }

        


        public string CurrentUserName { get; set; }

        public static SPKContext Create()
        {
            return new SPKContext();
        }

        public override int SaveChanges()
        {
            Audit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            Audit();
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Vacancy>().Property(c => c.StudyWeightPriority).HasPrecision(18, 10);
            modelBuilder.Entity<Vacancy>().Property(c => c.ExperienceWeightPriority).HasPrecision(18, 10);
            modelBuilder.Entity<Vacancy>().Property(c => c.CertificateWeightPriority).HasPrecision(18, 10);
            modelBuilder.Entity<Vacancy>().Property(c => c.AgeWeightPriority).HasPrecision(18, 10);

            modelBuilder.Entity<EmployeeCandidate>().Property(c => c.TotalWeight).HasPrecision(18, 10);

            modelBuilder.Entity<AHPCalculationTable>().Property(c => c.EmployeCandidateWeight).HasPrecision(18, 10);
            modelBuilder.Entity<AHPCalculationTable>().Property(c => c.VacancyWeight).HasPrecision(18, 10);
            modelBuilder.Entity<AHPCalculationTable>().Property(c => c.PriorityTotalWeight).HasPrecision(18, 10);

            var user = modelBuilder.Entity<IdentityUser>()
                     .ToTable("Users");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = false }));

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("UserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogins");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaims");

            var role = modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = false }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<SistemPendukungKeputusanApplicationUser>().ToTable("SistemPendukungKeputusanApplicationUsers").Property(r => r.Name).IsRequired();
        }

        private void Audit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                if (entry.Entity.GetType().IsSubclassOf(typeof(AuditEntityBase)))
                {
                    entry.Property("CreatedBy").CurrentValue = CurrentUserName;
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
                if (entry.Entity.GetType().IsSubclassOf(typeof(AuditEntityBase)))
                {
                    entry.Property("CreatedBy").CurrentValue = entry.Property("CreatedBy").CurrentValue;
                    entry.Property("CreatedAt").CurrentValue = entry.Property("CreatedAt").CurrentValue;
                    entry.Property("UpdatedBy").CurrentValue = CurrentUserName;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
        }

    }
}

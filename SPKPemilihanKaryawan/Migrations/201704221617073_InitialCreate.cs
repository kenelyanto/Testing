namespace SPKPemilihanKaryawan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AHPCalculationTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeCandidateId = c.Int(nullable: false),
                        Type = c.String(),
                        Priority = c.Int(nullable: false),
                        VacancyWeight = c.Decimal(nullable: false, precision: 18, scale: 10),
                        EmployeCandidateWeight = c.Decimal(nullable: false, precision: 18, scale: 10),
                        PriorityTotalWeight = c.Decimal(nullable: false, precision: 18, scale: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeCandidates", t => t.EmployeeCandidateId)
                .Index(t => t.EmployeeCandidateId);
            
            CreateTable(
                "dbo.EmployeeCandidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fullname = c.String(),
                        PhoneNumber = c.String(),
                        IdentificationNumber = c.String(),
                        TotalWeight = c.Decimal(nullable: false, precision: 18, scale: 10),
                        VacancyId = c.Int(nullable: false),
                        Study = c.Int(nullable: false),
                        Experience = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Certificate = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vacancies", t => t.VacancyId)
                .Index(t => t.VacancyId);
            
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        AgeWeightPriority = c.Decimal(nullable: false, precision: 18, scale: 10),
                        StudyWeightPriority = c.Decimal(nullable: false, precision: 18, scale: 10),
                        ExperienceWeightPriority = c.Decimal(nullable: false, precision: 18, scale: 10),
                        CertificateWeightPriority = c.Decimal(nullable: false, precision: 18, scale: 10),
                        NeedProcess = c.Boolean(nullable: false),
                        Study = c.Int(nullable: false),
                        Experience = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Certificate = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                        Query = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SistemPendukungKeputusanApplicationRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Report_Create = c.Boolean(nullable: false),
                        Report_Delete = c.Boolean(nullable: false),
                        Report_Edit = c.Boolean(nullable: false),
                        Report_Navigate = c.Boolean(nullable: false),
                        Report_Read = c.Boolean(nullable: false),
                        Vacancy_Create = c.Boolean(nullable: false),
                        Vacancy_Delete = c.Boolean(nullable: false),
                        Vacancy_Edit = c.Boolean(nullable: false),
                        Vacancy_Navigate = c.Boolean(nullable: false),
                        Vacancy_Read = c.Boolean(nullable: false),
                        EmployeeCandidate_Create = c.Boolean(nullable: false),
                        EmployeeCandidate_Delete = c.Boolean(nullable: false),
                        EmployeeCandidate_Edit = c.Boolean(nullable: false),
                        EmployeeCandidate_Navigate = c.Boolean(nullable: false),
                        EmployeeCandidate_Read = c.Boolean(nullable: false),
                        ApplicationRole_Create = c.Boolean(nullable: false),
                        ApplicationRole_Delete = c.Boolean(nullable: false),
                        ApplicationRole_Edit = c.Boolean(nullable: false),
                        ApplicationRole_Navigate = c.Boolean(nullable: false),
                        ApplicationRole_Read = c.Boolean(nullable: false),
                        ApplicationUser_Create = c.Boolean(nullable: false),
                        ApplicationUser_Delete = c.Boolean(nullable: false),
                        ApplicationUser_Edit = c.Boolean(nullable: false),
                        ApplicationUser_Navigate = c.Boolean(nullable: false),
                        ApplicationUser_Read = c.Boolean(nullable: false),
                        ApplicationUser_ResetPassword = c.Boolean(nullable: false),
                        IsSuperAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SistemPendukungKeputusanApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Active = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        SistemPendukungKeputusanApplicationRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .ForeignKey("dbo.SistemPendukungKeputusanApplicationRoles", t => t.SistemPendukungKeputusanApplicationRoleId)
                .Index(t => t.Id)
                .Index(t => t.SistemPendukungKeputusanApplicationRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SistemPendukungKeputusanApplicationUsers", "SistemPendukungKeputusanApplicationRoleId", "dbo.SistemPendukungKeputusanApplicationRoles");
            DropForeignKey("dbo.SistemPendukungKeputusanApplicationUsers", "Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.EmployeeCandidates", "VacancyId", "dbo.Vacancies");
            DropForeignKey("dbo.AHPCalculationTables", "EmployeeCandidateId", "dbo.EmployeeCandidates");
            DropIndex("dbo.SistemPendukungKeputusanApplicationUsers", new[] { "SistemPendukungKeputusanApplicationRoleId" });
            DropIndex("dbo.SistemPendukungKeputusanApplicationUsers", new[] { "Id" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.EmployeeCandidates", new[] { "VacancyId" });
            DropIndex("dbo.AHPCalculationTables", new[] { "EmployeeCandidateId" });
            DropTable("dbo.SistemPendukungKeputusanApplicationUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.SistemPendukungKeputusanApplicationRoles");
            DropTable("dbo.Reports");
            DropTable("dbo.Vacancies");
            DropTable("dbo.EmployeeCandidates");
            DropTable("dbo.AHPCalculationTables");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Dynamic;
using SistemPendukungKeputusan.Models;
using SistemPendukungKeputusan.Helper;
using SistemPendukungKeputusan.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using SistemPendukungKeputusan.Models.Security;
using SPKPemilihanKaryawan.Models;

namespace SPKPemilihanKaryawan.Controllers
{
    public class ApplicationRolesController : BaseController
    {
        public const string MODULE_NAME = "ApplicationRole";
        #region Methods

        public static MetronicDatatableSetting<SistemPendukungKeputusanApplicationRole> IndexListDatatableSetting()
        {
            SPKContext SPKContext = new SPKContext();
            MetronicDatatableSetting<SistemPendukungKeputusanApplicationRole> datatableSetting = new MetronicDatatableSetting<SistemPendukungKeputusanApplicationRole>();
            datatableSetting.BaseCollection = SPKContext.SistemPendukungKeputusanApplicationRoles;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationRole>()
            {
                Name = "Name",
                Caption = "Name",
                ColumnWidth = 40,
                Type = FieldType.Text,
                GetValueMethod = model => model.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationRole>()
            {
                Name = "Active",
                Caption = "Active",
                ColumnWidth = 9,
                Type = FieldType.Boolean,
                GetValueMethod = model => model.Active ? "Yes" : "No"
            });

            return datatableSetting;
        }

        public static MetronicDatatableSetting<ApplicationRoleDetailViewModel> IndexListDetailDatatableSetting(int prmId)
        {
            SPKContext SPKContext = new SPKContext();
            MetronicDatatableSetting<ApplicationRoleDetailViewModel> datatableSetting = new MetronicDatatableSetting<ApplicationRoleDetailViewModel>() { DisableDeleteButton = true, DisableDetailsButton = true, DisableEditButton = true };

            SistemPendukungKeputusanApplicationRole objAppRole = SPKContext.SistemPendukungKeputusanApplicationRoles.FirstOrDefault(c => c.Id == prmId);
            List<ApplicationRoleDetailViewModel> lstAppRole = new List<ApplicationRoleDetailViewModel>();

            Type type = objAppRole.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(bool) &&
                    property.Name != "Active" &&
                    property.Name != "IsSuperAdmin")
                {
                    if (!lstAppRole.Exists(c => c.RoleDescription == property.Name.Split('_')[0]))
                    {
                        lstAppRole.Add(new ApplicationRoleDetailViewModel
                        {
                            Id = objAppRole.Id,
                            Name = objAppRole.Name,
                            Active = objAppRole.Active,
                            RoleDescription = property.Name.Split('_')[0]
                        });
                    }

                    if (Convert.ToBoolean(property.GetValue(objAppRole, null)))
                    {
                        var objRole = lstAppRole.Where(d => d.RoleDescription == property.Name.Split('_')[0]).FirstOrDefault();
                        if (objRole != null) { objRole.Permission += property.Name.Split('_').Count() > 1 ? property.Name.Split('_')[1] + "; " : property.Name.Split('_')[0] + "; "; }
                    }
                }
            }

            datatableSetting.BaseCollection = lstAppRole.AsQueryable().Where(c => !string.IsNullOrEmpty(c.Permission));

            datatableSetting.ColumnSettings.Add(new ColumnSetting<ApplicationRoleDetailViewModel>()
            {
                Name = "RoleDescription",
                Caption = "Role Description",
                ColumnWidth = 40,
                Type = FieldType.Text,
                GetValueMethod = model => model.RoleDescription
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<ApplicationRoleDetailViewModel>()
            {
                Name = "Permission",
                Caption = "Permission",
                ColumnWidth = 9,
                Type = FieldType.Text,
                GetValueMethod = model => model.Permission
            });

            return datatableSetting;
        }

        // GET: ApplicationRoles/Create
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public ActionResult Create()
        {
            return View(new SistemPendukungKeputusanApplicationRole());
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Create")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Active," +
                                                                "Vacancy_Create,"+
                                                                "Vacancy_Delete,"+
                                                                "Vacancy_Edit,"+
                                                                "Vacancy_Navigate,"+
                                                                "Vacancy_Read,"+
                                                                "EmployeeCandidate_Create,"+
                                                                "EmployeeCandidate_Delete,"+
                                                                "EmployeeCandidate_Edit,"+
                                                                "EmployeeCandidate_Navigate,"+
                                                                "EmployeeCandidate_Read,"+
                                                                "Report_Create," +
                                                                "Report_Delete," +
                                                                "Report_Edit," +
                                                                "Report_Navigate," +
                                                                "Report_Read," +
                                                                "ApplicationRole_Create," +
                                                                "ApplicationRole_Delete," +
                                                                "ApplicationRole_Edit," +
                                                                "ApplicationRole_Navigate," +
                                                                "ApplicationRole_Read," +
                                                                "ApplicationUser_Create," +
                                                                "ApplicationUser_Delete," +
                                                                "ApplicationUser_Edit," +
                                                                "ApplicationUser_Navigate," +
                                                                "ApplicationUser_Read," +
                                                                "IsSuperAdmin")] SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole)
        {
            if (ModelState.IsValid)
            {
                db.SistemPendukungKeputusanApplicationRoles.Add(sistemPendukungKeputusanApplicationRole);
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = sistemPendukungKeputusanApplicationRole.Id });

                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return View(sistemPendukungKeputusanApplicationRole);
        }

        // GET: ApplicationRoles/Delete/5
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole = await db.SistemPendukungKeputusanApplicationRoles.FindAsync(id);
            if (sistemPendukungKeputusanApplicationRole == null)
            {
                TempData["ErrorNote"] = "The application role can not be found";
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole = await db.SistemPendukungKeputusanApplicationRoles.FindAsync(id);
            SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = db.SistemPendukungKeputusanApplicationUsers.Where(c => c.SistemPendukungKeputusanApplicationRoleId == sistemPendukungKeputusanApplicationRole.Id).FirstOrDefault();
            if(sistemPendukungKeputusanApplicationUser != null)
            {
                TempData["ErrorNote"] = "This roll still being used";
                return RedirectToAction("Index");
            }
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    sistemPendukungKeputusanApplicationRole.Delete(db);
                    transaction.Commit();
                    TempData["InfoNote"] = "Data deleted successfully";
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    TempData["InfoNote"] = ex.Message;
                }
            }
            return RedirectToAction("Index");
        }

        // GET: ApplicationRoles/Details/5
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole = await db.SistemPendukungKeputusanApplicationRoles.FindAsync(id);
            if (sistemPendukungKeputusanApplicationRole == null)
            {
                TempData["ErrorNote"] = "The application role can not be found";
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationRole);
        }

        // GET: ApplicationRoles/Edit/5
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole = await db.SistemPendukungKeputusanApplicationRoles.FindAsync(id);
            if (sistemPendukungKeputusanApplicationRole == null)
            {
                TempData["ErrorNote"] = "The application role can not be found";
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationRole);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Active," +
                                                                "Vacancy_Create,"+
                                                                "Vacancy_Delete,"+
                                                                "Vacancy_Edit,"+
                                                                "Vacancy_Navigate,"+
                                                                "Vacancy_Read,"+
                                                                "EmployeeCandidate_Create,"+
                                                                "EmployeeCandidate_Delete,"+
                                                                "EmployeeCandidate_Edit,"+
                                                                "EmployeeCandidate_Navigate,"+
                                                                "EmployeeCandidate_Read,"+
                                                                "Report_Create," +
                                                                "Report_Delete," +
                                                                "Report_Edit," +
                                                                "Report_Navigate," +
                                                                "Report_Read," +
                                                                "ApplicationRole_Create," +
                                                                "ApplicationRole_Delete," +
                                                                "ApplicationRole_Edit," +
                                                                "ApplicationRole_Navigate," +
                                                                "ApplicationRole_Read," +
                                                                "ApplicationUser_Create," +
                                                                "ApplicationUser_Delete," +
                                                                "ApplicationUser_Edit," +
                                                                "ApplicationUser_Navigate," +
                                                                "ApplicationUser_Read," +
                                                                "IsSuperAdmin")] SistemPendukungKeputusanApplicationRole sistemPendukungKeputusanApplicationRole)
        {
            if (ModelState.IsValid)
            {
                SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = db.SistemPendukungKeputusanApplicationUsers.Where(c => c.SistemPendukungKeputusanApplicationRoleId == sistemPendukungKeputusanApplicationRole.Id && c.Active).FirstOrDefault();
                if (sistemPendukungKeputusanApplicationUser != null)
                {
                    TempData["ErrorNote"] = "This roll still being used";
                    return RedirectToAction("Index");
                }
                db.Entry(sistemPendukungKeputusanApplicationRole).State = EntityState.Modified;
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = sistemPendukungKeputusanApplicationRole.Id });

                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationRole);
        }

        // GET: ApplicationRoles
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() => View());
        }

       
        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<SistemPendukungKeputusanApplicationRole> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
        }

        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> ShowIndexDetailList(int ApplicationRoleId)
        {
            MetronicDatatableSetting<ApplicationRoleDetailViewModel> dtDetailSetting = IndexListDetailDatatableSetting(ApplicationRoleId);
            dtDetailSetting.BaseCollection = dtDetailSetting.BaseCollection.Where(m => m.Id == ApplicationRoleId);
            return Content(await dtDetailSetting.GetJsonContent(this));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Methods
    }
}

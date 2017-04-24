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

namespace SPKPemilihanKaryawan.Controllers
{
    public class EmployeeCandidatesController : BaseController
    {
        public const string MODULE_NAME = "EmployeeCandidate";
        string redirectTabName = "#tab_employeeCandidate";
        public async Task<ActionResult> Index()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "EmployeeCandidates"), Name = "Employee Candidates" });
            ViewBag.Breadcrumbs = Breadcrumbs;
            return await Task.Run(() => View());
        }
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public static MetronicDatatableSetting<EmployeeCandidate> IndexListDatatableSetting()
        {
            SPKContext spkContext = new SPKContext();

            MetronicDatatableSetting<EmployeeCandidate> datatableSetting = new MetronicDatatableSetting<EmployeeCandidate>();
            datatableSetting.BaseCollection = spkContext.EmployeeCandidates;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<EmployeeCandidate>()
            {
                Name = "Fullname",
                Caption = "Full name",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Fullname
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<EmployeeCandidate>()
            {
                Name = "IdentificationNumber",
                Caption = "Identification Number",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.IdentificationNumber
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<EmployeeCandidate>()
            {
                Name = "PhoneNumber",
                Caption = "Phone Number",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.PhoneNumber
            });
            return datatableSetting;

        }
        
        public async Task<ActionResult> ShowIndexList(int vacancyId)
        {
            MetronicDatatableSetting<EmployeeCandidate> dtSetting = IndexListDatatableSetting();
            dtSetting.BaseCollection = dtSetting.BaseCollection.Where(m => m.VacancyId == vacancyId);
            return Content(await dtSetting.GetJsonContent(this));
        }

        [Authorize(Roles = MODULE_NAME + "_Create")]
        public ActionResult Create(int id)
        {
            Vacancy vacancy = db.Vacancies.Find(id);
            if (vacancy == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "EmployeeCandidates"), Name = "Employee Candidate" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "EmployeeCandidates"), Name = "Create Employee Candidate" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(new EmployeeCandidate() {Vacancy = vacancy, VacancyId = vacancy.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PhoneNumber,IdentificationNumber,Fullname,VacancyId,Study,Experience,Age,Certificate")] EmployeeCandidate employeeCandidate)
        {
            Vacancy vacancy = await db.Vacancies.FindAsync(employeeCandidate.VacancyId);
            
            if (vacancy == null)
                return HttpNotFound("Vacancy not found");

            if (ModelState.IsValid)
            {
                vacancy.NeedProcess = true;
                employeeCandidate.Vacancy = vacancy;
                db.EmployeeCandidates.Add(employeeCandidate);
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = employeeCandidate.Id });
                    case "btnSaveAndClose":
                        return new RedirectResult(Url.Action("Details", "Vacancies", new { id = employeeCandidate.VacancyId }) + redirectTabName);
                }
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "EmployeeCandidates"), Name = "Employee Candidate" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "EmployeeCandidates"), Name = "Create Employee Candidate" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(employeeCandidate);
        }
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeCandidate employeeCandidate = await db.EmployeeCandidates.FindAsync(id);
            if (employeeCandidate == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "EmployeeCandidates"), Name = "Employee Candidates" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "EmployeeCandidates", new { Id = employeeCandidate.Id }), Name = "Edit Employee Candidate #" + employeeCandidate.Fullname });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(employeeCandidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PhoneNumber,IdentificationNumber,Fullname,VacancyId,Study,Experience,Age,Certificate")] EmployeeCandidate employeeCandidate)
        {
            if (ModelState.IsValid)
            {
                Vacancy vacancy = await db.Vacancies.FindAsync(employeeCandidate.VacancyId);
                vacancy.NeedProcess = true;
                db.Entry(employeeCandidate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = employeeCandidate.Id });
                    case "btnSaveAndClose":
                        return new RedirectResult(Url.Action("Details", "Vacancies", new { id = employeeCandidate.VacancyId}) + redirectTabName);
                }
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "EmployeeCandidates"), Name = "Vacancy" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "EmployeeCandidates", new { Id = employeeCandidate.Id }), Name = "Edit Vacancy #" + employeeCandidate.Fullname });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(employeeCandidate);
        }

        // GET: Offices/Delete/5
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeCandidate employeeCandidate = await db.EmployeeCandidates.FindAsync(id);
            if (employeeCandidate == null)
            {
                return HttpNotFound();
            }
            return View(employeeCandidate);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int VacancyId = 0;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    EmployeeCandidate employeeCandidate = await db.EmployeeCandidates.FindAsync(id);
                    Vacancy vacancy = await db.Vacancies.FindAsync(employeeCandidate.VacancyId);
                    vacancy.NeedProcess = true;
                    db.SaveChanges();
                    VacancyId = employeeCandidate.VacancyId;
                    employeeCandidate.Delete(db);
                    transaction.Commit();
                    TempData["InfoNote"] = "Data deleted successfully";
                }
                catch (InvalidOperationException ex)
                {
                    transaction.Rollback();
                    TempData["InfoNote"] = ex.Message;
                }
            }
            return new RedirectResult(Url.Action("Details", "Vacancies", new { id = VacancyId }) + redirectTabName);
        }
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeCandidate employeeCandidate = await db.EmployeeCandidates.FindAsync(id);

            if (employeeCandidate == null)
            {
                return HttpNotFound();
            }
            return View(employeeCandidate);
        }
    }
}

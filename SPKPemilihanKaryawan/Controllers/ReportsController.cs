using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Helper;
using SistemPendukungKeputusan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SPKPemilihanKaryawan.Controllers
{
    public class ReportsController : BaseController
    {
        #region Methods
        public const string MODULE_NAME = "Report";

        public static MetronicDatatableSetting<Report> IndexListDatatableSetting()
        {
            SPKContext spkContext = new SPKContext();

            MetronicDatatableSetting<Report> datatableSetting = new MetronicDatatableSetting<Report>();
            datatableSetting.BaseCollection = spkContext.Reports;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<Report>()
            {
                Name = "Name",
                Caption = "Name",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Report>()
            {
                Name = "FileName",
                Caption = "File Name",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.FileName
            });
            return datatableSetting;
        }

        // GET: Sectors
        public async Task<ActionResult> Index()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Reports"), Name = "Reports" });
            ViewBag.Breadcrumbs = Breadcrumbs;
            return await Task.Run(() => View());
        }

        public ActionResult List()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("List", "Reports"), Name = "Reports" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            //return View(results);

            return RedirectToAction("ShowReport");
        }

        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<Report> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
        }

        [Authorize(Roles = MODULE_NAME + "_Read")]
        public ActionResult ShowReport()
        {
            ViewBag.ReportId = new SelectList(db.Reports, "Id", "Name");
            ViewBag.VacancyId = new SelectList(db.Vacancies.Where(m => m.Active == true && !m.NeedProcess), "Id", "Name");
            
            ShowReportParameters model = new ShowReportParameters();

            return View(model);
        }

        [HttpPost]
        public ActionResult ShowReport([Bind(Include = "ReportId,VacancyId")] ShowReportParameters showReportParameters)
        {
            if (ModelState.IsValid)
            {
                Report report = db.Reports.FirstOrDefault(m => m.Id == showReportParameters.ReportId);
                if (report == null)
                    return HttpNotFound();

                Session.Add("Report", report);
                Session.Add("ReportParameter", showReportParameters);
                return Redirect(Url.Action("ReportViewer.aspx", "WebForms"));
            }
            ViewBag.ReportId = new SelectList(db.Reports, "Id", "Name");
            ViewBag.VacancyId = new SelectList(db.Vacancies.Where(m => m.Active == true), "Id", "Name");

            return View();
        }

        // GET: Sectors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        [Authorize(Roles = MODULE_NAME + "_Create")]
        public ActionResult Create()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Reports"), Name = "Report" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Reports"), Name = "Create Report" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(new Report());
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FileName,Name,Query")] Report Report)
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Reports"), Name = "Report" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Reports"), Name = "Create Report" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            if (ModelState.IsValid)
            {
                db.Reports.Add(Report);
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = Report.Id });

                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return View(Report);
        }

        // GET: Reports/Edit/5
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report Report = await db.Reports.FindAsync(id);
            if (Report == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Reports"), Name = "Report" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Reports", new { Id = Report.Id }), Name = "Edit Report #" + Report.Name });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(Report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,FileName,Query")] Report Report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Report).State = EntityState.Modified;
                await db.SaveChangesAsync();
                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = Report.Id });

                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Reports"), Name = "Report" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Reports", new { Id = Report.Id }), Name = "Edit Report #" + Report.Name });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(Report);
        }

        // GET: Reports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report Report = await db.Reports.FindAsync(id);
            if (Report == null)
            {
                return HttpNotFound();
            }
            return View(Report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Report Report = await db.Reports.FindAsync(id);
                
                    db.Reports.Remove(Report);
                    await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
        }

        #endregion Methods
    }
}
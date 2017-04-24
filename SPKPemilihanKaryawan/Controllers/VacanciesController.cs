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
using SistemPendukungKeputusan.Models.Base;
using SistemPendukungKeputusan.Models.Enum;
using System.Drawing.Drawing2D;

namespace SPKPemilihanKaryawan.Controllers
{
    public class VacanciesController : BaseController
    {
        public const string MODULE_NAME = "Vacancy";
        // GET: Offices
        public async Task<ActionResult> Index()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Vacancies"), Name = "Vacancies" });
            ViewBag.Breadcrumbs = Breadcrumbs;
            return await Task.Run(() => View());
        }
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public static MetronicDatatableSetting<Vacancy> IndexListDatatableSetting()
        {
            SPKContext spkContext = new SPKContext();

            MetronicDatatableSetting<Vacancy> datatableSetting = new MetronicDatatableSetting<Vacancy>();
            datatableSetting.BaseCollection = spkContext.Vacancies;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<Vacancy>()
            {
                Name = "Name",
                Caption = "Name",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Vacancy>()
            {
                Name = "Active",
                Caption = "Active",
                ColumnWidth = 5,
                Type = FieldType.Boolean,
                GetValueMethod = model => model.Active ? "Yes" : "No"
            });
            return datatableSetting;

        }

        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<Vacancy> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
        }

        [Authorize(Roles = MODULE_NAME + "_Create")]
        public ActionResult Create()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Vacancies"), Name = "Vacancy" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Vacancies"), Name = "Create Vacancy" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(new Vacancy());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Active,Study,Experience,Age,Certificate")] Vacancy vacancy)
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Vacancies"), Name = "Vacancy" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Vacancies"), Name = "Create Vacancy" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            if (ModelState.IsValid)
            {
                vacancy.NeedProcess = true;
                db.Vacancies.Add(vacancy);
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = vacancy.Id });
                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return View(vacancy);
        }
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Vacancies"), Name = "Vacancies" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Vacancies", new { Id = vacancy.Id }), Name = "Edit Vacancy #" + vacancy.Name });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Active,Study,Experience,Age,Certificate")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                vacancy.NeedProcess = true;
                db.Entry(vacancy).State = EntityState.Modified;
                await db.SaveChangesAsync();
                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = vacancy.Id });
                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Vacancies"), Name = "Vacancy" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Vacancies", new { Id = vacancy.Id }), Name = "Edit Vacancy #" + vacancy.Name });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(vacancy);
        }

        // GET: Offices/Delete/5
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Vacancy vacancy = await db.Vacancies.FindAsync(id);
                    vacancy.Delete(db);
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
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Process(int? id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            Vacancy vacancy = await db.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                TempData["ErrorNote"] = "The vacancy can not be found";
                return RedirectToAction("Index");
            }
            AHPVacanciesCalculation(vacancy);
            AHPCalculationTable(vacancy);
            AHPEmployeeCandidateCalculation(vacancy);

            vacancy.NeedProcess = false;
            db.SaveChanges();

            TempData["InfoNote"] = "Process Finished";
            return RedirectToAction("Details", new { Id = vacancy.Id });
        }

        public void AHPVacanciesCalculation(Vacancy vacancy)
        {
            decimal pendiPenga = numberComparision(vacancy.Study - vacancy.Experience);
            decimal pendiUsia = numberComparision(vacancy.Study - vacancy.Age);
            decimal pendiSertifikasi = numberComparision(vacancy.Study - vacancy.Certificate);
            decimal pengaUsia = numberComparision(vacancy.Experience - vacancy.Age);
            decimal pengaSertifikasi = numberComparision(vacancy.Experience - vacancy.Certificate);
            decimal usiaSertifikasi = numberComparision(vacancy.Age - vacancy.Certificate);

            decimal matriksX1Y1 = ((1 * 1) + (pendiPenga * (1 / pendiPenga)) + (pendiUsia * (1 / pendiUsia)) + (pendiSertifikasi * (1 / pendiSertifikasi)));
            decimal matriksX2Y1 = (pendiPenga + pendiPenga + (pendiUsia * (1 / pengaUsia)) + (pendiSertifikasi * (1 / pengaSertifikasi)));
            decimal matriksX3Y1 = (pendiUsia + (pendiPenga * pengaUsia) + pendiUsia + (pendiSertifikasi * (1 / usiaSertifikasi)));
            decimal matriksX4Y1 = (pendiSertifikasi + (pendiPenga * pengaSertifikasi) + (pendiUsia * usiaSertifikasi) + pendiSertifikasi);
            decimal totalRow1 = matriksX1Y1 + matriksX2Y1 + matriksX3Y1 + matriksX4Y1;

            decimal matriksX1Y2 = (((1 / pendiPenga) * 1) + (1 * (1 / pendiPenga)) + (pengaUsia * (1 / pendiUsia)) + (pengaSertifikasi * (1 / pendiSertifikasi)));
            decimal matriksX2Y2 = (((1 / pendiPenga) * pendiPenga) + (1 * 1) + (pengaUsia * (1 / pengaUsia)) + (pengaSertifikasi * (1 / pengaSertifikasi)));
            decimal matriksX3Y2 = (((1 / pendiPenga) * pendiUsia) + (1 * pengaUsia) + (pengaUsia * 1) + (pengaSertifikasi * (1 / usiaSertifikasi)));
            decimal matriksX4Y2 = (((1 / pendiPenga) * pendiSertifikasi) + (1 * pengaSertifikasi) + (pengaUsia * usiaSertifikasi) + pengaSertifikasi);
            decimal totalRow2 = matriksX1Y2 + matriksX2Y2 + matriksX3Y2 + matriksX4Y2;

            decimal matriksX1Y3 = (((1 / pendiUsia) * 1) + ((1 / pengaUsia) * (1 / pendiPenga)) + (1 * (1 / pendiUsia)) + (usiaSertifikasi * (1 / pendiSertifikasi)));
            decimal matriksX2Y3 = (((1 / pendiUsia) * pendiPenga) + ((1 / pengaUsia) * 1) + (1 * (1 / pengaUsia)) + (usiaSertifikasi * (1 / pengaSertifikasi)));
            decimal matriksX3Y3 = (((1 / pendiUsia) * pendiUsia) + ((1 / pengaUsia) * pengaUsia) + (1 * 1) + (usiaSertifikasi * (1 / usiaSertifikasi)));
            decimal matriksX4Y3 = (((1 / pendiUsia) * pendiSertifikasi) + ((1 / pengaUsia) * pengaSertifikasi) + (1 * usiaSertifikasi) + usiaSertifikasi);
            decimal totalRow3 = matriksX1Y3 + matriksX2Y3 + matriksX3Y3 + matriksX4Y3;

            decimal matriksX1Y4 = (((1 / pendiSertifikasi) * 1) + ((1 / pengaSertifikasi) * (1 / pendiPenga)) + ((1 / usiaSertifikasi) * (1 / pendiUsia)) + (1 * (1 / pendiSertifikasi)));
            decimal matriksX2Y4 = (((1 / pendiSertifikasi) * pendiPenga) + ((1 / pengaSertifikasi) * 1) + ((1 / usiaSertifikasi) * (1 / pengaUsia)) + (1 * (1 / pengaSertifikasi)));
            decimal matriksX3Y4 = (((1 / pendiSertifikasi) * pendiUsia) + ((1 / pengaSertifikasi) * pengaUsia) + ((1 / usiaSertifikasi) * 1) + (1 * (1 / usiaSertifikasi)));
            decimal matriksX4Y4 = (((1 / pendiSertifikasi) * pendiSertifikasi) + ((1 / pengaSertifikasi) * pengaSertifikasi) + ((1 / usiaSertifikasi) * usiaSertifikasi) + 1);
            decimal totalRow4 = matriksX1Y4 + matriksX2Y4 + matriksX3Y4 + matriksX4Y4;

            decimal totalMatriks = totalRow1 + totalRow2 + totalRow3 + totalRow4;

            vacancy.StudyWeightPriority = totalRow1 / totalMatriks;
            vacancy.ExperienceWeightPriority = totalRow2 / totalMatriks;
            vacancy.AgeWeightPriority = totalRow3 / totalMatriks;
            vacancy.CertificateWeightPriority = totalRow4 / totalMatriks;

            db.SaveChanges();
        }

        public void AHPEmployeeCandidateCalculation(Vacancy vacancy)
        {
            List<AHPCalculationTable> ListPendidikanAHPCalculationTable = db.AHPCalculationTables.Where(c => c.Type == "Study" && c.EmployeeCandidate.VacancyId == vacancy.Id).ToList();
            List<AHPCalculationTable> ListPengalamanAHPCalculationTable = db.AHPCalculationTables.Where(c => c.Type == "Experience" && c.EmployeeCandidate.VacancyId == vacancy.Id).ToList();
            List<AHPCalculationTable> ListUsiaAHPCalculationTable = db.AHPCalculationTables.Where(c => c.Type == "Age" && c.EmployeeCandidate.VacancyId == vacancy.Id).ToList();
            List<AHPCalculationTable> ListSertifikasiAHPCalculationTable = db.AHPCalculationTables.Where(c => c.Type == "Certificate" && c.EmployeeCandidate.VacancyId == vacancy.Id).ToList();

            decimal[,] PendidikanArray = new decimal[ListPendidikanAHPCalculationTable.Count(), ListPendidikanAHPCalculationTable.Count()];
            decimal[,] PengalamanArray = new decimal[ListPengalamanAHPCalculationTable.Count(), ListPengalamanAHPCalculationTable.Count()];
            decimal[,] UsiaArray = new decimal[ListUsiaAHPCalculationTable.Count(), ListUsiaAHPCalculationTable.Count()];
            decimal[,] SertifikasiArray = new decimal[ListSertifikasiAHPCalculationTable.Count(), ListUsiaAHPCalculationTable.Count()];

            for (int x = 0; x < ListPendidikanAHPCalculationTable.Count(); x++)
            {
                for (int y = 0; y < ListPendidikanAHPCalculationTable.Count(); y++)
                {
                    PendidikanArray[x, y] = numberComparision(ListPendidikanAHPCalculationTable[x].Priority - ListPendidikanAHPCalculationTable[y].Priority);
                    PengalamanArray[x, y] = numberComparision(ListPengalamanAHPCalculationTable[x].Priority - ListPengalamanAHPCalculationTable[y].Priority);
                    UsiaArray[x, y] = numberComparision(ListUsiaAHPCalculationTable[x].Priority - ListUsiaAHPCalculationTable[y].Priority);
                    SertifikasiArray[x, y] = numberComparision(ListSertifikasiAHPCalculationTable[x].Priority - ListSertifikasiAHPCalculationTable[y].Priority);
                }
            }
            decimal[,] PendidikanArray2 = MultiplyMatrix(PendidikanArray, PendidikanArray);
            decimal[,] PengalamanArray2 = MultiplyMatrix(PengalamanArray, PengalamanArray);
            decimal[,] UsiaArray2 = MultiplyMatrix(UsiaArray, UsiaArray);
            decimal[,] SertifikasiArray2 = MultiplyMatrix(SertifikasiArray, SertifikasiArray);

            decimal[] PendidikanArray2Percentage = Percentage(PendidikanArray2);
            decimal[] PengalamanArray2Percentage = Percentage(PengalamanArray2);
            decimal[] UsiaArray2Percentage = Percentage(UsiaArray2);
            decimal[] Sertifikasi2Percentage = Percentage(SertifikasiArray2);

            for (int z = 0; z < Sertifikasi2Percentage.Count(); z++)
            {
                int candidateId = ListPendidikanAHPCalculationTable[z].EmployeeCandidateId;
                EmployeeCandidate employeeCandidate = db.EmployeeCandidates.Where(c => c.Id == candidateId).FirstOrDefault();
                employeeCandidate.TotalWeight = 0;

                ListPendidikanAHPCalculationTable[z].EmployeCandidateWeight = PendidikanArray2Percentage[z];
                ListPendidikanAHPCalculationTable[z].PriorityTotalWeight = PendidikanArray2Percentage[z] * ListPendidikanAHPCalculationTable[z].VacancyWeight;

                ListPengalamanAHPCalculationTable[z].EmployeCandidateWeight = PengalamanArray2Percentage[z];
                ListPengalamanAHPCalculationTable[z].PriorityTotalWeight = PengalamanArray2Percentage[z] * ListPengalamanAHPCalculationTable[z].VacancyWeight;

                ListUsiaAHPCalculationTable[z].EmployeCandidateWeight = UsiaArray2Percentage[z];
                ListUsiaAHPCalculationTable[z].PriorityTotalWeight = UsiaArray2Percentage[z] * ListUsiaAHPCalculationTable[z].VacancyWeight;

                ListSertifikasiAHPCalculationTable[z].EmployeCandidateWeight = Sertifikasi2Percentage[z];
                ListSertifikasiAHPCalculationTable[z].PriorityTotalWeight = Sertifikasi2Percentage[z] * ListSertifikasiAHPCalculationTable[z].VacancyWeight;

                employeeCandidate.TotalWeight = ListPendidikanAHPCalculationTable[z].PriorityTotalWeight + ListPengalamanAHPCalculationTable[z].PriorityTotalWeight + ListUsiaAHPCalculationTable[z].PriorityTotalWeight + ListSertifikasiAHPCalculationTable[z].PriorityTotalWeight;
            }
            db.SaveChanges();
       
        }

        public decimal[] Percentage(decimal[,] A)
        {
            decimal sum = A.Cast<decimal>().Sum();
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            decimal temp = 0;
            decimal[] kHasil = new decimal[rA];


            for (int i = 0; i < rA; i++)
            {
                temp = 0;
                for (int j = 0; j < cA; j++)
                {
                    temp += A[i, j];
                }
                temp = temp / sum;
                kHasil[i] = temp;
            }
            return kHasil;
        }

        public decimal[,] MultiplyMatrix(decimal[,] A, decimal[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            decimal temp = 0;
            decimal[,] kHasil = new decimal[rA, cB];
            if (cA != rB)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }
                return kHasil;
            }
            return kHasil;
        }

        public void AHPCalculationTable(Vacancy vacancy)
        {

            foreach (EmployeeCandidate employeecandidate in vacancy.EmployeeCandidates)
            {
                foreach (AHPCalculationTable ahpcalculationTable in employeecandidate.AHPCalculationTables)
                {
                    ahpcalculationTable.DeleteById(ahpcalculationTable.Id);
                }
                db.SaveChanges();
                for (int x = 0; x < 4; x++)
                {
                    AHPCalculationTable ahpCalculationTable = new AHPCalculationTable();
                    string type = "";
                    switch (x)
                    {
                        case 0:
                            type = "Study";
                            ahpCalculationTable.Type = type;
                            ahpCalculationTable.EmployeeCandidateId = employeecandidate.Id;
                            ahpCalculationTable.EmployeeCandidate = employeecandidate;
                            ahpCalculationTable.VacancyWeight = vacancy.StudyWeightPriority;
                            ahpCalculationTable.Priority = employeecandidate.Study;
                            break;
                        case 1:
                            type = "Experience";
                            ahpCalculationTable.Type = type;
                            ahpCalculationTable.EmployeeCandidateId = employeecandidate.Id;
                            ahpCalculationTable.EmployeeCandidate = employeecandidate;
                            ahpCalculationTable.VacancyWeight = vacancy.ExperienceWeightPriority;
                            ahpCalculationTable.Priority = employeecandidate.Experience;
                            break;
                        case 2:
                            type = "Certificate";
                            ahpCalculationTable.Type = type;
                            ahpCalculationTable.EmployeeCandidateId = employeecandidate.Id;
                            ahpCalculationTable.EmployeeCandidate = employeecandidate;
                            ahpCalculationTable.VacancyWeight = vacancy.CertificateWeightPriority;
                            ahpCalculationTable.Priority = employeecandidate.Certificate;
                            break;
                        case 3:
                            type = "Age";
                            ahpCalculationTable.Type = type;
                            ahpCalculationTable.EmployeeCandidateId = employeecandidate.Id;
                            ahpCalculationTable.EmployeeCandidate = employeecandidate;
                            ahpCalculationTable.VacancyWeight = vacancy.AgeWeightPriority;
                            ahpCalculationTable.Priority = employeecandidate.Age;
                            break;
                    }
                    db.AHPCalculationTables.Add(ahpCalculationTable);
                }
                db.SaveChanges();
            }
        }

        public decimal numberComparision(int x)
        {
            if (x == 0)
            {
                return 1;
            }
            else if (x == 1)
            {
                return 3;
            }
            else if (x == 2)
            {
                return 5;
            }
            else if (x == 3)
            {
                return 7;
            }
            else if (x == 4)
            {
                return 9;
            }
            else if (x == -1)
            {
                return (decimal)1 / (decimal)3;
            }
            else if (x == -2)
            {
                return (decimal)1 / (decimal)5;
            }
            else if (x == -3)
            {
                return (decimal)1 / (decimal)7;
            }
            else if (x == -4)
            {
                return (decimal)1 / (decimal)9;
            }

            return x;
        }
    }
}

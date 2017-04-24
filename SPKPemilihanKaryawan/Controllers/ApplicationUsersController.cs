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
using Microsoft.AspNet.Identity.Owin;
using SistemPendukungKeputusan.Models.Security;
using System.Data.Entity.Validation;
using SPKPemilihanKaryawan.Models;

namespace SPKPemilihanKaryawan.Controllers
{
    public class ApplicationUsersController : BaseController
    {
        public const string MODULE_NAME = "ApplicationUser";
        private SistemPendukungKeputusanSignInManager _signInManager;
        private SistemPendukungKeputusanUserManager _userManager;

        public ApplicationUsersController()
        {
        }

        public ApplicationUsersController(SistemPendukungKeputusanUserManager userManager, SistemPendukungKeputusanSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public SistemPendukungKeputusanSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SistemPendukungKeputusanSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public SistemPendukungKeputusanUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<SistemPendukungKeputusanUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        // GET: ApplicationUsers/Details/5
        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = await db.SistemPendukungKeputusanApplicationUsers.FindAsync(id);
            if (sistemPendukungKeputusanApplicationUser == null)
            {
                TempData["ErrorNote"] = "The application user can not be found";
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationUser);
        }

        // GET: ApplicationUsers/Create
        [Authorize(Roles = MODULE_NAME + "_Create")]
        public ActionResult Create()
        {
            ViewBag.SistemPendukungKeputusanApplicationRoleId = new SelectList(db.SistemPendukungKeputusanApplicationRoles.Where(role => role.Active == true), "Id", "Name");
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Create")]
        public async Task<ActionResult> Create([Bind(Include = "Email,UserName,Name,Active,Password,ConfirmPassword,SistemPendukungKeputusanApplicationRoleId")] RegisterViewModel model)
        {
            DbContextTransaction dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        TempData["ErrorNote"] = "Password Confirm is wrong";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string email = model.Email;
                        string userName = model.Name;
                        string password = model.Password;
                        string name = model.Name;

                        SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanUser = db.SistemPendukungKeputusanApplicationUsers.Where(c => c.Id == email).FirstOrDefault();
                        if(sistemPendukungKeputusanUser != null){
                            TempData["ErrorNote"] = "Email Already Used";
                            return RedirectToAction("Index");
                        }

                        var userStore = new UserStore<SistemPendukungKeputusanApplicationUser>(db);
                        var manager = new SistemPendukungKeputusanUserManager(userStore);

                        var userToInsert = new SistemPendukungKeputusanApplicationUser { Name = name, UserName = userName, Email = email, EmailConfirmed = true, SistemPendukungKeputusanApplicationRoleId = model.SistemPendukungKeputusanApplicationRoleId };
                        IdentityResult result = await UserManager.CreateAsync(userToInsert, password);


                        switch (Request.Form["submit"])
                        {
                            case "btnSave":
                                return RedirectToAction("Details", new { Id = userToInsert.Id });
                            case "btnSaveAndClose":
                                return RedirectToAction("Index");
                        }
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (InvalidOperationException ex)
            {
                dbContextTransaction.Rollback();
                TempData["ErrorNote"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.SistemPendukungKeputusanApplicationRoleId = new SelectList(db.SistemPendukungKeputusanApplicationRoles.Where(role => role.Active == true), "Id", "Name", model.SistemPendukungKeputusanApplicationRoleId);
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = await db.SistemPendukungKeputusanApplicationUsers.FindAsync(id);
            if (sistemPendukungKeputusanApplicationUser == null)
            {
                TempData["ErrorNote"] = "The application user can not be found";
                return RedirectToAction("Index");
            }
            ViewBag.SistemPendukungKeputusanApplicationRoleId = new SelectList(db.SistemPendukungKeputusanApplicationRoles.Where(role => role.Active == true), "Id", "Name", sistemPendukungKeputusanApplicationUser.SistemPendukungKeputusanApplicationRoleId);
            return View(sistemPendukungKeputusanApplicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Edit")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Active,Name,SistemPendukungKeputusanApplicationRoleId")] SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser)
        {
            try
            {
                SistemPendukungKeputusanApplicationUser objApplicationUser = await db.SistemPendukungKeputusanApplicationUsers.FindAsync(sistemPendukungKeputusanApplicationUser.Id);
                if (ModelState.IsValid)
                {
                    objApplicationUser.Active = sistemPendukungKeputusanApplicationUser.Active;
                    objApplicationUser.Name = sistemPendukungKeputusanApplicationUser.Name;
                    objApplicationUser.SistemPendukungKeputusanApplicationRoleId = sistemPendukungKeputusanApplicationUser.SistemPendukungKeputusanApplicationRoleId;
                    await db.SaveChangesAsync();

                    switch (Request.Form["submit"])
                    {
                        case "btnSave":
                            return RedirectToAction("Details", new { Id = objApplicationUser.Id });
                        case "btnSaveAndClose":
                            return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.SistemPendukungKeputusanApplicationRoleId = new SelectList(db.SistemPendukungKeputusanApplicationRoles.Where(role => role.Active == true), "Id", "Name", sistemPendukungKeputusanApplicationUser.SistemPendukungKeputusanApplicationRoleId);
                return View(sistemPendukungKeputusanApplicationUser);
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = await db.SistemPendukungKeputusanApplicationUsers.FindAsync(id);
            if (sistemPendukungKeputusanApplicationUser == null)
            {
                TempData["ErrorNote"] = "The application user can not be found";
                return RedirectToAction("Index");
            }
            return View(sistemPendukungKeputusanApplicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            List<SistemPendukungKeputusanApplicationUser> ListsistemPendukungKeputusanUser = db.SistemPendukungKeputusanApplicationUsers.ToList();
            if(ListsistemPendukungKeputusanUser.Count() == 1)
            {
                TempData["ErrorNote"] = "The last user remaining";
                return RedirectToAction("Index");
            }
            SistemPendukungKeputusanApplicationUser sistemPendukungKeputusanApplicationUser = await db.SistemPendukungKeputusanApplicationUsers.FindAsync(id);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    sistemPendukungKeputusanApplicationUser.Delete(db);
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

        public static MetronicDatatableSetting<SistemPendukungKeputusanApplicationUser> IndexListDatatableSetting()
        {
            SPKContext SPKContext = new SPKContext();
            var batamFastApplicationRoles = from objBatamFastApplicationRole in SPKContext.SistemPendukungKeputusanApplicationRoles
                                            where objBatamFastApplicationRole.Active == true
                                            select new { Id = objBatamFastApplicationRole.Id, Name = objBatamFastApplicationRole.Name };

            MetronicDatatableSetting<SistemPendukungKeputusanApplicationUser> datatableSetting = new MetronicDatatableSetting<SistemPendukungKeputusanApplicationUser>();
            datatableSetting.BaseCollection = SPKContext.SistemPendukungKeputusanApplicationUsers;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationUser>()
            {
                Name = "SistemPendukungKeputusanApplicationRoleId",
                Caption = "Role",
                Type = FieldType.ForeignObject,
                ColumnWidth = 30,
                SelectList = new SelectList(batamFastApplicationRoles, "Id", "Name"),
                GetValueMethod = model => model.SistemPendukungKeputusanApplicationRole.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationUser>()
            {
                Name = "Name",
                Caption = "Name",
                ColumnWidth = 40,
                Type = FieldType.Text,
                GetValueMethod = model => model.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationUser>()
            {
                Name = "UserName",
                Caption = "Username",
                ColumnWidth = 40,
                Type = FieldType.Text,
                GetValueMethod = model => model.UserName
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationUser>()
            {
                Name = "Email",
                Caption = "Email",
                ColumnWidth = 40,
                Type = FieldType.Text,
                GetValueMethod = model => model.Email
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<SistemPendukungKeputusanApplicationUser>()
            {
                Name = "IsSuperAdmin",
                Caption = "Is Super Admin",
                ColumnWidth = 10,
                Type = FieldType.Boolean,
                GetValueMethod = model => model.SistemPendukungKeputusanApplicationRole.IsSuperAdmin == true ? "Yes" : "No"
            });
            return datatableSetting;
        }

        [Authorize(Roles = MODULE_NAME + "_Read")]
        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<SistemPendukungKeputusanApplicationUser> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = MODULE_NAME + "_ResetPassword")]
        public async Task<ActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                TempData["ErrorNote"] = "Sorry, something went wrong";
                return RedirectToAction("Index");
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                TempData["InfoNote"] = "User is not yet confirm email or not exists in the system";
            }
            try
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                return Redirect(callbackUrl);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorNote"] = ex.Message;
            }

            return RedirectToAction("Details", new { Id = user.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

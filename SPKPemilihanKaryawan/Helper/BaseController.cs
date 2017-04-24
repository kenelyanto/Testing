using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemPendukungKeputusan.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SistemPendukungKeputusan.Helper
{
    public class BaseController : Controller
    {
        #region Fields

        protected SPKContext db = new SPKContext();

        #endregion Fields

        #region Methods

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (User != null)
            {
                string currentUserId = User.Identity.GetUserId();
                IdentityUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                if (currentUser != null)
                {
                    ViewBag.CurrentUser = currentUser;
                    db.CurrentUserName = ((SistemPendukungKeputusan.Models.Security.IUser)currentUser).Name;
                }
            }
        }

        #endregion Methods
    }
}
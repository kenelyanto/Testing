using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SPKPemilihanKaryawan.Web
{
    public class CustomRoleProvider : RoleProvider
    {
        #region Properties

        public override string ApplicationName
        {
            get
            {
                return "SPKPemilihanKaryawan";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion Properties

        #region Methods

        public static bool IsUserInRole(SistemPendukungKeputusanApplicationUser user, string roleName)
        {
            if (user == null) return false;
            if (user.SistemPendukungKeputusanApplicationRole.IsSuperAdmin) return true;
            return user.SistemPendukungKeputusanApplicationRole.GetRoles().Exists(m => m == roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new SPKContext())
            {
                var user = db.SistemPendukungKeputusanApplicationUsers.SingleOrDefault(u => u.UserName == username);
                if (user == null)
                    return false;
                return IsUserInRole(user, roleName);
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var db = new SPKContext())
            {
                var user = db.SistemPendukungKeputusanApplicationUsers.SingleOrDefault(u => u.UserName == username);
                if (user == null)
                    return new string[] { };
                if (user.SistemPendukungKeputusanApplicationRole.IsSuperAdmin) return GetAllRoles();
                return user.SistemPendukungKeputusanApplicationRole.GetRoles().ToArray();
            }
        }

        public override string[] GetAllRoles()
        {

            List<string> lst = new List<string>();
            Type type = typeof(SistemPendukungKeputusanApplicationRole);
            var properties = type.GetProperties();
            string[] excludedProperties = { "Id", "Code", "Name", "IsSuperAdmin", "Active"};
            foreach (var property in properties)
                if (!excludedProperties.Contains(property.Name))
                    lst.Add(property.Name);
            return lst.ToArray();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
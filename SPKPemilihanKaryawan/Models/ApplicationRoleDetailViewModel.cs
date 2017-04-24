using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPKPemilihanKaryawan.Models
{
    public class ApplicationRoleDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string RoleDescription { get; set; }
        public string Permission { get; set; }
    }
}

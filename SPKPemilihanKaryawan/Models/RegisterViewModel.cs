using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPKPemilihanKaryawan.Models
{

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
  
        public RegisterViewModel() : base()
        {
  
        }

        [UIHint("ForeignObject")]
        [Display(Name = "Application Role")]
        public int SistemPendukungKeputusanApplicationRoleId { get; set; }
    }
}

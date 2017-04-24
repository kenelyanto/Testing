using SistemPendukungKeputusan.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPendukungKeputusan.Models.Base
{
    public abstract class PrioritasHierarki : AuditEntityBase
    {

        [Required]
        [Display(Name = "Study")]
        public Prioritas Study { get; set; }

        [Required]
        [Display(Name = "Experience")]
        public Prioritas Experience { get; set; }

        [Required]
        [Display(Name = "Age")]
        public Prioritas Age { get; set; }

        [Required]
        [Display(Name = "Certificate")]
        public Prioritas Certificate { get; set; }
    }
}

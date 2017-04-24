using SistemPendukungKeputusan.DAL;
using SistemPendukungKeputusan.Models.Base;
using SPKPemilihanKaryawan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemPendukungKeputusan.Models
{
    public class ShowReportParameters
    {
        [Display(Name = "Report")]
        [UIHint("ForeignObject")]
        public int ReportId { get; set; }

        [UIHint("ForeignObject")]
        [Display(Name = "Vacancy")]
        public int? VacancyId { get; set; }

    }
}
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
    public class Report
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [UIHint("MultiLineText")]
        public string Query { get; set; }

    }
}
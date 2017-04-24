using SistemPendukungKeputusan.Models.Base;
using SPKPemilihanKaryawan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemPendukungKeputusan.Models.Enum
{
    public enum Prioritas
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Very Low")]
        VeryLow = 0,
        [System.ComponentModel.DataAnnotations.Display(Name = "Low")]
        Low = 1,
        [System.ComponentModel.DataAnnotations.Display(Name = "Average")]
        Average = 2,
        [System.ComponentModel.DataAnnotations.Display(Name = "High")]
        High = 3,
        [System.ComponentModel.DataAnnotations.Display(Name = "Very High")]
        VeryHigh = 4


    }
}
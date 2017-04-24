using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPendukungKeputusan.Models.Security
{
    public interface IUser
    {
        string Name { get; set; }
    }
}

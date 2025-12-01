using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class Meses
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string nombreMes {  get; set; }
    }
}
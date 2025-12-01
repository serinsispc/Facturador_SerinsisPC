using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class CobrosEnviados
    {
        [Required]
        public int id {  get; set; }
        [Required]
        public DateTime fechaCobro { get; set; }
        [Required]
        public int idCliente { get; set; }
        [Required]
        public int sedesCobradas { get; set; }
        [Required]
        public int mesesCobrados { get; set; }
        [Required]
        public decimal valotTotalCobrado { get; set; }
    }
}
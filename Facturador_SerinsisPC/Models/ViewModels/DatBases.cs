using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class DatBases
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int idCliente { get; set; }
        [Required]
        public string nameDataBase { get; set; }
        [Required]
        public int estado {  get; set; }
    }

    public class EstadoBaseServidor
    {
        public string name { get; set; }
        public string state_desc { get; set; }
    }
}

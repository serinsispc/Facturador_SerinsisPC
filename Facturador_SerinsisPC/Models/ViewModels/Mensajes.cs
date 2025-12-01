using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class Mensajes
    {
        [Required]
       public int id {  get; set; }
        [Required]
       public string nombreMensaje { get; set; }
        [Required]
       public string mensajeText { get; set; }
        [Required]
       public int variables { get; set; }
    }
}
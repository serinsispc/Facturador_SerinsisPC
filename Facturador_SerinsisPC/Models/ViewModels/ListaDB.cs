using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class ListaDB
    {
        [Required]
        public int database_id { get; set; }
        [Required]
        public string name {  get; set; }
    }
}
using System;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class UsuarioAdmin
    {
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string loginUsuario { get; set; }
        public string whatsApp { get; set; }
        public bool estado { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaUltimoAcceso { get; set; }
    }
}

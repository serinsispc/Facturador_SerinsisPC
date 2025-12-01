using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFacturacionElectronicaDIAN.Entities.Request
{
    public class CorreoRequest : Acceso
    {
        public List<To> to { get; set; }
        public List<Cc> cc { get; set; }
    }
    public class Cc
    {
        public string email { get; set; }
    }
    public class To
    {
        public string email { get; set; }
    }
}

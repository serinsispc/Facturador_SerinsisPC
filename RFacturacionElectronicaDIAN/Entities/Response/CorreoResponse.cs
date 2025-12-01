using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFacturacionElectronicaDIAN.Entities.Response
{
    public class To
    {
        public string email { get; set; }
    }
    public class Cc
    {
        public string email { get; set; }
    }
    public class CorreoResponse
    {
        public bool? is_valid { get; set; }
    }
}

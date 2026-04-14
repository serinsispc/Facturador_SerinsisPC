using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Facturador_SerinsisPC.Servicios;

namespace Facturador_SerinsisPC
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ClassConexionPrincipal.Configurar();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturador_SerinsisPC.Servicios;

namespace Facturador_SerinsisPC
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClassConexionPrincipal.Configurar();

            if (Session["idUsuarioAdmin"] == null)
            {
                Response.Redirect("login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}

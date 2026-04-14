using Facturador_SerinsisPC.Models.Controlers;
using System;
using System.Web.UI;

namespace Facturador_SerinsisPC
{
    public partial class ingresos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rpIngresosMensuales.DataSource = control_V_IngresosMensuales.ListaCompleta();
                rpIngresosMensuales.DataBind();

                rpPagosRecibidos.DataSource = control_PagosRecibidos.ListaCompleta();
                rpPagosRecibidos.DataBind();
            }
        }
    }
}

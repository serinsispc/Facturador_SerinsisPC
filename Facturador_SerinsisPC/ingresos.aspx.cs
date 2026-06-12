using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class ingresos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMetodosPagoFiltro();
                CargarResumenIngresos();
                CargarPagosRecibidos();
                CargarGestionPagos();
            }
        }

        protected void FiltrosPagoChanged(object sender, EventArgs e)
        {
            CargarGestionPagos();
        }

        protected void btnLimpiarFiltrosPago_Click(object sender, EventArgs e)
        {
            txtFiltroPago.Text = string.Empty;
            ddlFiltroMetodoPago.SelectedIndex = 0;
            ddlFiltroPeriodoPago.SelectedIndex = 0;
            CargarGestionPagos();
        }

        protected void btnEliminarPago_Click(object sender, EventArgs e)
        {
            int idPago = Convert.ToInt32((sender as LinkButton).CommandArgument);
            RespuestaSQL respuesta = control_PagosRecibidos.EliminarPagoRecibido(idPago);

            if (respuesta == null || !respuesta.respuesta)
            {
                Mensage("Error", respuesta != null && !string.IsNullOrWhiteSpace(respuesta.mensaje)
                    ? respuesta.mensaje
                    : "No fue posible eliminar el pago seleccionado.", "error");
                return;
            }

            CargarResumenIngresos();
            CargarPagosRecibidos();
            CargarGestionPagos();
            Mensage("Ok", "El pago fue eliminado y el cobro asociado quedo reactivado.", "success");
        }

        private void CargarResumenIngresos()
        {
            rpIngresosMensuales.DataSource = control_V_IngresosMensuales.ListaCompleta();
            rpIngresosMensuales.DataBind();
        }

        private void CargarPagosRecibidos()
        {
            rpPagosRecibidos.DataSource = control_PagosRecibidos.ListaCompleta();
            rpPagosRecibidos.DataBind();
        }

        private void CargarMetodosPagoFiltro()
        {
            ddlFiltroMetodoPago.Items.Clear();
            ddlFiltroMetodoPago.Items.Add(new ListItem("Todos", string.Empty));

            foreach (MetodoPago metodo in control_MetodoPago.ListaCompleta())
            {
                ddlFiltroMetodoPago.Items.Add(new ListItem(metodo.nombreMetodo, metodo.nombreMetodo));
            }
        }

        private void CargarGestionPagos()
        {
            List<V_PagosRecibidos> pagos = FiltrarPagos(control_PagosRecibidos.ListaCompleta());
            rpPagosGestion.DataSource = pagos;
            rpPagosGestion.DataBind();
            lblSinPagos.Visible = pagos.Count == 0;
        }

        private List<V_PagosRecibidos> FiltrarPagos(List<V_PagosRecibidos> pagos)
        {
            pagos = pagos ?? new List<V_PagosRecibidos>();

            string texto = (txtFiltroPago.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(texto))
            {
                pagos = pagos.Where(x =>
                    (x.nombreComercial ?? string.Empty).IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (x.nombreRepresentate ?? string.Empty).IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (x.referenciaPago ?? string.Empty).IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (x.numeroComprobante ?? string.Empty).IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            string metodo = ddlFiltroMetodoPago.SelectedValue;
            if (!string.IsNullOrWhiteSpace(metodo))
            {
                pagos = pagos.Where(x => string.Equals((x.nombreMetodo ?? string.Empty).Trim(), metodo, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            string periodo = ddlFiltroPeriodoPago.SelectedValue;
            DateTime hoy = DateTime.Today;
            if (string.Equals(periodo, "hoy", StringComparison.OrdinalIgnoreCase))
            {
                pagos = pagos.Where(x => x.fechaPago.Date == hoy).ToList();
            }
            else if (int.TryParse(periodo, out int dias) && dias > 0)
            {
                DateTime desde = hoy.AddDays(-dias);
                pagos = pagos.Where(x => x.fechaPago.Date >= desde && x.fechaPago.Date <= hoy).ToList();
            }

            return pagos
                .OrderByDescending(x => x.fechaPago)
                .ThenBy(x => x.nombreComercial)
                .ToList();
        }

        protected void Mensage(string titulo, string mensage, string tipo)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
        }
    }
}

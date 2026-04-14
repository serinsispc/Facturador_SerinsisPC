using App_WhatsApp;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Facturador_SerinsisPC
{
    public partial class cobrar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltroPlanes();
                CargarDG();
            }
        }

        protected async void CargarListaCobroWhatsApp()
        {
            ConfigWhatsAppMeta configMeta = control_ConfigWhatsAppMeta.Consultar();
            if (configMeta == null ||
                string.IsNullOrWhiteSpace(configMeta.accessToken) ||
                string.IsNullOrWhiteSpace(configMeta.phoneNumberId))
            {
                Mensage("Error", "No se encontro la configuracion de WhatsApp Meta.", "error");
                return;
            }

            List<V_Facturas> whatsApp = control_Facturas.Lista().Where(x => x.idEstado == 1).ToList();
            if (whatsApp.Count > 0)
            {
                int contadorEnviados = 0;
                foreach (V_Facturas wor in whatsApp)
                {
                    App_WhatsApp.WhatsAppResponse appResponse = await recordatorio_de_pago.plantilla(
                        wor.celular,
                        wor.nombreComercial,
                        wor.nombreMes,
                        wor.contador.ToString(),
                        Convert.ToInt32(wor.saldoPendiente > 0 ? wor.saldoPendiente : wor.valorAPagar),
                        wor.fechaVencimiento.HasValue ? wor.fechaVencimiento.Value.ToString("dd-MM-yyyy") : DateTime.Today.ToString("dd-MM-yyyy"),
                        "454-000044-36",
                        "901824648-7",
                        "SERINSIS SAS",
                        configMeta.accessToken,
                        configMeta.phoneNumberId);

                    App_WhatsApp.Messages messages = appResponse?.messages?.FirstOrDefault();
                    if (messages != null && messages.message_status == "accepted")
                    {
                        contadorEnviados++;
                        GuardarCobro(wor.idCliente, wor.sedes, wor.contador, wor.saldoPendiente > 0 ? wor.saldoPendiente : wor.valorAPagar);
                    }
                }
                Mensage("OK", $"Se enviaron {contadorEnviados} cobros correctamente", "success");
            }
        }

        protected void GuardarCobro(int idCliente, int sedes, int meses, decimal total)
        {
            CobrosEnviados cobrosEnviados = new CobrosEnviados
            {
                id = 0,
                fechaCobro = DateTime.Now,
                idCliente = idCliente,
                sedesCobradas = sedes,
                mesesCobrados = meses,
                valotTotalCobrado = total
            };
            controlador_CobrosEnviados.Crud(cobrosEnviados, 0);
        }

        protected void Mensage(string titulo, string mensage, string tipo, string aspx = "")
        {
            if (aspx != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta('" + titulo + "','" + mensage + "','" + tipo + "','" + aspx + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
            }
        }

        protected void btnCobrar_Click(object sender, EventArgs e)
        {
            CargarListaCobroWhatsApp();
        }

        protected void FiltrosControlPagosChanged(object sender, EventArgs e)
        {
            CargarDG();
        }

        protected void FiltrosHistorialChanged(object sender, EventArgs e)
        {
            CargarDG();
        }

        protected void CargarDG()
        {
            List<V_ControlPagosClientes> controlPagos = FiltrarControlPagos(control_V_ControlPagosClientes.ListaCompleta());
            rpControlPagos.DataSource = controlPagos;
            rpControlPagos.DataBind();

            List<V_CobrosEnviados> historialCobros = FiltrarHistorialCobros(controlador_CobrosEnviados.LIstaCompleta());
            rpCobrosEnviados.DataSource = historialCobros;
            rpCobrosEnviados.DataBind();

            CargarResumen(controlPagos);
        }

        protected void btnCargarDG_Click(object sender, EventArgs e)
        {
            CargarDG();
        }

        protected void btnLimpiarFiltrosCobro_Click(object sender, EventArgs e)
        {
            txtFiltroCliente.Text = string.Empty;
            ddlFiltroPlan.SelectedIndex = 0;
            ddlFiltroSaldo.SelectedIndex = 0;
            ddlFiltroVencimiento.SelectedIndex = 0;
            txtFiltroHistorial.Text = string.Empty;
            ddlFiltroPeriodoHistorial.SelectedIndex = 0;
            CargarDG();
        }

        private void CargarFiltroPlanes()
        {
            List<string> planes = control_V_ControlPagosClientes.ListaCompleta()
                .Where(x => !string.IsNullOrWhiteSpace(x.nombrePlan))
                .Select(x => x.nombrePlan.Trim().ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ddlFiltroPlan.Items.Clear();
            ddlFiltroPlan.Items.Add(new System.Web.UI.WebControls.ListItem("Todos", string.Empty));

            foreach (string plan in planes)
            {
                ddlFiltroPlan.Items.Add(new System.Web.UI.WebControls.ListItem(plan, plan));
            }
        }

        private List<V_ControlPagosClientes> FiltrarControlPagos(List<V_ControlPagosClientes> controlPagos)
        {
            controlPagos = controlPagos ?? new List<V_ControlPagosClientes>();

            string texto = (txtFiltroCliente.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(texto))
            {
                controlPagos = controlPagos.Where(x =>
                    ContieneTexto(x.nombreComercial, texto) ||
                    ContieneTexto(x.nombreRepresentate, texto)).ToList();
            }

            string plan = ddlFiltroPlan.SelectedValue;
            if (!string.IsNullOrWhiteSpace(plan))
            {
                controlPagos = controlPagos.Where(x => string.Equals((x.nombrePlan ?? string.Empty).Trim(), plan, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            switch (ddlFiltroSaldo.SelectedValue)
            {
                case "con_saldo":
                    controlPagos = controlPagos.Where(x => x.saldoPendienteTotal > 0).ToList();
                    break;
                case "sin_saldo":
                    controlPagos = controlPagos.Where(x => x.saldoPendienteTotal <= 0).ToList();
                    break;
            }

            DateTime hoy = DateTime.Today;
            switch (ddlFiltroVencimiento.SelectedValue)
            {
                case "vencidos":
                    controlPagos = controlPagos.Where(x => x.proximoVencimiento.HasValue && x.proximoVencimiento.Value.Date <= hoy).ToList();
                    break;
                case "7dias":
                    controlPagos = controlPagos.Where(x => x.proximoVencimiento.HasValue && x.proximoVencimiento.Value.Date >= hoy && x.proximoVencimiento.Value.Date <= hoy.AddDays(7)).ToList();
                    break;
                case "30dias":
                    controlPagos = controlPagos.Where(x => x.proximoVencimiento.HasValue && x.proximoVencimiento.Value.Date >= hoy && x.proximoVencimiento.Value.Date <= hoy.AddDays(30)).ToList();
                    break;
                case "sin_fecha":
                    controlPagos = controlPagos.Where(x => !x.proximoVencimiento.HasValue).ToList();
                    break;
            }

            return controlPagos
                .OrderByDescending(x => x.saldoPendienteTotal)
                .ThenBy(x => x.proximoVencimiento ?? DateTime.MaxValue)
                .ThenBy(x => x.nombreComercial)
                .ToList();
        }

        private List<V_CobrosEnviados> FiltrarHistorialCobros(List<V_CobrosEnviados> historialCobros)
        {
            historialCobros = historialCobros ?? new List<V_CobrosEnviados>();

            string texto = (txtFiltroHistorial.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(texto))
            {
                historialCobros = historialCobros.Where(x =>
                    ContieneTexto(x.nombreComercial, texto) ||
                    ContieneTexto(x.nombreRepresentate, texto) ||
                    ContieneTexto(x.celular, texto)).ToList();
            }

            if (int.TryParse(ddlFiltroPeriodoHistorial.SelectedValue, out int dias))
            {
                DateTime desde = DateTime.Today.AddDays(-dias);
                historialCobros = historialCobros.Where(x => x.fechaCobro.Date >= desde).ToList();
            }

            return historialCobros
                .OrderByDescending(x => x.fechaCobro)
                .ThenBy(x => x.nombreComercial)
                .ToList();
        }

        private void CargarResumen(List<V_ControlPagosClientes> controlPagos)
        {
            controlPagos = controlPagos ?? new List<V_ControlPagosClientes>();
            DateTime hoy = DateTime.Today;

            lblResumenSaldoPendiente.Text = controlPagos.Sum(x => x.saldoPendienteTotal).ToString("C0");
            lblResumenClientesConSaldo.Text = controlPagos.Count(x => x.saldoPendienteTotal > 0).ToString();
            lblResumenVencidos.Text = controlPagos.Count(x => x.proximoVencimiento.HasValue && x.proximoVencimiento.Value.Date <= hoy).ToString();
        }

        private bool ContieneTexto(string origen, string texto)
        {
            return !string.IsNullOrWhiteSpace(origen) &&
                   origen.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}

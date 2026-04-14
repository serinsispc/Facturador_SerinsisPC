using App_WhatsApp;
using App_WhatsApp.Plantillas;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using RFacturacionElectronicaDIAN.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class facturar : Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFiltroEstadoFacturas();
                Cargar_rpFacturas();
                CargarMeses();
                CargarMetodosPago();
            }

            if (Session["month"] != null) ddl_Mes.SelectedValue = $"{Session["month"]}";
            if (Session["year"] != null) txtyear.Text = $"{Session["year"]}";
        }

        protected void btnBUscarCliente_Click(object sender, EventArgs e)
        {
            CargarListaClientes();
            panelModalBuscarCliente.Visible = true;
        }

        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            if (VerificarCampos())
            {
                Session["month"] = Convert.ToInt32(ddl_Mes.SelectedItem.Value);
                Session["year"] = txtyear.Text;
                if (GestionarFactura((int)ViewState["idCliente"], Convert.ToInt32(ddl_Mes.SelectedItem.Value), Convert.ToInt32(txtyear.Text)))
                {
                    Mensage(1, "Ok", "La factura fue gestionada correctamente.", "success", "facturar.aspx");
                }
                else
                {
                    Mensage(2, "Error", "La factura no fue gestionada correctamente.", "error", "");
                }
            }
            else
            {
                Mensage(2, "Error", "Aun se encuentran campos vacios.", "error", "");
            }
        }

        protected void btnCerrarModalBuscarCliente_Click(object sender, EventArgs e)
        {
            panelModalBuscarCliente.Visible = false;
        }

        protected void btnCerrarModalBuscarCliente2_Click(object sender, EventArgs e)
        {
            panelModalBuscarCliente.Visible = false;
        }

        protected void btnCerrarModalPago_Click(object sender, EventArgs e)
        {
            panelModalPago.Visible = false;
        }

        protected void btnSeleccionarCliente_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"] = id;
            BuscarIdCliente(id);
            panelModalBuscarCliente.Visible = false;
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Facturas factura = control_Facturas.Consultar_id(id);
            V_Facturas vista = control_Facturas.Consultar_id_vista(id);
            if (factura == null || vista == null)
            {
                Mensage(2, "Error", "No fue posible cargar la factura seleccionada.", "error", "");
                return;
            }

            ViewState["idFacturaPago"] = id;
            lblPagoFactura.Text = vista.nombreComercial + " - " + vista.nombreMes + " " + vista.yearFactura;
            txtFechaPago.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtValorPago.Text = Convert.ToInt32(factura.saldoPendiente > 0 ? factura.saldoPendiente : factura.valorAPagar).ToString();
            txtNumeroComprobante.Text = string.Empty;
            txtReferenciaPago.Text = string.Empty;
            txtObservacionPago.Text = string.Empty;
            panelModalPago.Visible = true;
        }

        protected async void btnRegistrarPago_Click(object sender, EventArgs e)
        {
            if (ViewState["idFacturaPago"] == null)
            {
                Mensage(2, "Error", "No hay una factura seleccionada para registrar el pago.", "error", "");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFechaPago.Text) || string.IsNullOrWhiteSpace(txtValorPago.Text) || ddlMetodoPago.SelectedItem == null)
            {
                panelModalPago.Visible = true;
                Mensage(2, "Error", "Completa la fecha, el valor y el metodo de pago.", "error", "");
                return;
            }

            int idFactura = Convert.ToInt32(ViewState["idFacturaPago"]);
            Facturas factura = control_Facturas.Consultar_id(idFactura);
            V_Facturas facturaVista = control_Facturas.Consultar_id_vista(idFactura);
            if (factura == null || facturaVista == null)
            {
                Mensage(2, "Error", "No fue posible consultar la factura.", "error", "");
                return;
            }

            decimal valorRecibido = Convert.ToDecimal(txtValorPago.Text);
            if (valorRecibido <= 0)
            {
                panelModalPago.Visible = true;
                Mensage(2, "Error", "El valor recibido debe ser mayor a cero.", "error", "");
                return;
            }
            PagosRecibidos pago = new PagosRecibidos
            {
                fechaPago = Convert.ToDateTime(txtFechaPago.Text),
                idCliente = factura.idCliente,
                idMetodoPago = Convert.ToInt32(ddlMetodoPago.SelectedValue),
                valorRecibido = valorRecibido,
                numeroComprobante = txtNumeroComprobante.Text,
                referenciaPago = txtReferenciaPago.Text,
                observacion = txtObservacionPago.Text,
                usuarioRegistro = Convert.ToString(Session["nombreUsuario"])
            };

            RespuestaSQL respuesta = control_PagosRecibidos.RegistrarPagoFactura(pago, idFactura, valorRecibido);
            if (respuesta == null || !respuesta.respuesta)
            {
                panelModalPago.Visible = true;
                Mensage(2, "Error", "No fue posible registrar el pago recibido.", "error", "");
                return;
            }

            Facturas facturaActualizada = control_Facturas.Consultar_id(idFactura);
            if (facturaActualizada != null && facturaActualizada.idEstado == 3)
            {
                ActualizarControlPagoCliente(facturaActualizada, pago.fechaPago);
                await EnviarConfirmacionPago(facturaVista);
            }

            Cargar_rpFacturas();
            panelModalPago.Visible = false;
            Mensage(1, "Ok", "Pago registrado correctamente.", "success", "facturar.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Facturas facturas = control_Facturas.Consultar_id(id);
            if (facturas != null && control_Facturas.Crud(facturas, 2).respuesta)
            {
                Mensage(1, "Ok", "Factura eliminada.", "success", "facturar.aspx");
            }
        }

        protected void ddlFiltroEstadoFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cargar_rpFacturas();
        }
        #endregion

        #region Funciones
        protected void Cargar_rpFacturas()
        {
            var facturasTodas = NormalizarSaldosFacturas(control_Facturas.Lista());
            ViewState["ValorPendiente"] = CalcularPendienteAcumulado(facturasTodas);

            var facturas = facturasTodas;

            string estadoSeleccionado = ddlFiltroEstadoFacturas.SelectedValue;
            if (!string.IsNullOrWhiteSpace(estadoSeleccionado))
            {
                if (string.Equals(estadoSeleccionado, "CON_SALDO", StringComparison.OrdinalIgnoreCase))
                {
                    facturas = facturas
                        .Where(x => x.saldoPendiente > 0)
                        .ToList();
                }
                else
                {
                    facturas = facturas
                        .Where(x => string.Equals((x.nombreEstado ?? string.Empty).Trim(), estadoSeleccionado, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            rpFacturas.DataSource = facturas;
            rpFacturas.DataBind();
        }

        protected List<V_Facturas> NormalizarSaldosFacturas(List<V_Facturas> facturas)
        {
            facturas = facturas ?? new List<V_Facturas>();

            foreach (V_Facturas factura in facturas)
            {
                bool esActiva = factura != null &&
                                string.Equals((factura.nombreEstado ?? string.Empty).Trim(), "ACTIVA", StringComparison.OrdinalIgnoreCase);

                if (esActiva && factura.saldoPendiente <= 0)
                {
                    factura.saldoPendiente = factura.valorAPagar;
                }
            }

            return facturas;
        }

        protected decimal CalcularPendienteAcumulado(List<V_Facturas> facturas)
        {
            facturas = facturas ?? new List<V_Facturas>();

            return facturas
                .Where(x =>
                    string.Equals((x.nombreEstado ?? string.Empty).Trim(), "ACTIVA", StringComparison.OrdinalIgnoreCase) ||
                    x.saldoPendiente > 0)
                .Sum(x => x.saldoPendiente > 0 ? x.saldoPendiente : x.valorAPagar);
        }

        protected void CargarFiltroEstadoFacturas()
        {
            var estados = control_Facturas.Lista()
                .Where(x => !string.IsNullOrWhiteSpace(x.nombreEstado))
                .Select(x => x.nombreEstado.Trim().ToUpper())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ddlFiltroEstadoFacturas.Items.Clear();
            ddlFiltroEstadoFacturas.Items.Add(new ListItem("Todos", string.Empty));
            ddlFiltroEstadoFacturas.Items.Add(new ListItem("Con saldo", "CON_SALDO"));

            foreach (string estado in estados)
            {
                ddlFiltroEstadoFacturas.Items.Add(new ListItem(estado, estado));
            }

            if (ddlFiltroEstadoFacturas.Items.FindByValue("ACTIVA") != null)
            {
                ddlFiltroEstadoFacturas.SelectedValue = "ACTIVA";
            }
        }

        protected void CargarMeses()
        {
            ddl_Mes.DataValueField = "id";
            ddl_Mes.DataTextField = "nombreMes";
            ddl_Mes.DataSource = control_Meses.Lista();
            ddl_Mes.DataBind();
        }

        protected void CargarMetodosPago()
        {
            ddlMetodoPago.DataValueField = "id";
            ddlMetodoPago.DataTextField = "nombreMetodo";
            ddlMetodoPago.DataSource = control_MetodoPago.ListaCompleta();
            ddlMetodoPago.DataBind();
        }

        protected void CargarListaClientes()
        {
            rpClientes.DataSource = control_Clientes.ListaCompleta();
            rpClientes.DataBind();
        }

        protected void BuscarIdCliente(int idCliente)
        {
            V_Clientes cliente = control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (cliente != null)
            {
                DateTime? fechaProximoPago = ResolverFechaProximoPagoCliente(cliente);

                txtCliente.Text = cliente.nombreComercial;
                txtValorPlan.Text = Convert.ToInt32(cliente.valorPlan).ToString();
                txtSedes.Text = cliente.sedes.ToString();
                txtValorAPagar.Text = Convert.ToInt32(cliente.sedes * cliente.valorPlan).ToString();
                txtDiaPagoCliente.Text = (cliente.diaPago ?? 5).ToString();
                txtProximoPagoCliente.Text = fechaProximoPago.HasValue ? fechaProximoPago.Value.ToString("yyyy-MM-dd") : string.Empty;
                txtFechaVencimientoFactura.Text = fechaProximoPago.HasValue ? fechaProximoPago.Value.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd");
                lblPeriodicidadCliente.Text = FormatearPeriodicidad(cliente.periodicidadMeses);
                lblInicioPlanCliente.Text = cliente.fechaInicioPlan.HasValue ? cliente.fechaInicioPlan.Value.ToString("yyyy-MM-dd") : "Sin definir";
                lblUltimoPagoCliente.Text = cliente.fechaUltimoPago.HasValue ? cliente.fechaUltimoPago.Value.ToString("yyyy-MM-dd") : "Sin registrar";
                ViewState["PeriodicidadCliente"] = cliente.periodicidadMeses;
            }
        }

        protected DateTime? ResolverFechaProximoPagoCliente(V_Clientes cliente)
        {
            if (cliente == null)
            {
                return null;
            }

            if (cliente.fechaProximoPago.HasValue)
            {
                return cliente.fechaProximoPago.Value;
            }

            if (cliente.fechaUltimoPago.HasValue && cliente.diaPago.HasValue)
            {
                int periodicidad = cliente.periodicidadMeses <= 0 ? 1 : cliente.periodicidadMeses;
                return ClassCartera.CalcularProximaFechaPago(cliente.fechaUltimoPago.Value, cliente.diaPago.Value, periodicidad);
            }

            if (cliente.fechaInicioPlan.HasValue && cliente.diaPago.HasValue)
            {
                int periodicidad = cliente.periodicidadMeses <= 0 ? 1 : cliente.periodicidadMeses;
                return ClassCartera.CalcularProximoPagoInicial(cliente.fechaInicioPlan.Value, cliente.diaPago.Value, periodicidad);
            }

            return null;
        }

        protected bool VerificarCampos()
        {
            return ViewState["idCliente"] != null &&
                   txtyear.Text != "" &&
                   ddl_Mes.SelectedItem != null &&
                   txtCliente.Text != "" &&
                   txtValorPlan.Text != "" &&
                   txtSedes.Text != "" &&
                   txtValorAPagar.Text != "" &&
                   txtFechaVencimientoFactura.Text != "";
        }

        protected string FormatearPeriodicidad(int periodicidadMeses)
        {
            switch (periodicidadMeses)
            {
                case 12: return "Anual";
                case 6: return "Semestral";
                default: return "Mensual";
            }
        }

        protected bool GestionarFactura(int idCliente, int idMes, int year)
        {
            Facturas facturas = control_Facturas.ConsultarFactura(idCliente, idMes, year);
            if (facturas != null)
            {
                return false;
            }

            V_Clientes cliente = control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (cliente == null)
            {
                return false;
            }

            DateTime fechaVencimiento = Convert.ToDateTime(txtFechaVencimientoFactura.Text);
            int periodicidad = cliente.periodicidadMeses <= 0 ? 1 : cliente.periodicidadMeses;
            DateTime periodoHasta = fechaVencimiento;
            DateTime periodoDesde = fechaVencimiento.AddMonths(-periodicidad).AddDays(1);

            facturas = new Facturas
            {
                id = 0,
                fechaFactura = DateTime.Now,
                idCliente = idCliente,
                idMes = idMes,
                valorPlan = Convert.ToDecimal(txtValorPlan.Text),
                sedes = Convert.ToInt32(txtSedes.Text),
                valorAPagar = Convert.ToDecimal(txtValorAPagar.Text),
                idEstado = 1,
                contador = periodicidad,
                yearFactura = Convert.ToInt32(txtyear.Text),
                fechaVencimiento = fechaVencimiento,
                periodoDesde = periodoDesde,
                periodoHasta = periodoHasta,
                saldoPendiente = Convert.ToDecimal(txtValorAPagar.Text),
                fechaPagoCompleto = null
            };

            RespuestaSQL respuesta = control_Facturas.Crud(facturas, 0);
            return respuesta != null && respuesta.respuesta;
        }

        protected void ActualizarControlPagoCliente(Facturas factura, DateTime fechaPago)
        {
            Clientes cliente = control_Clientes.ConsultarIdCliente(factura.idCliente);
            V_Clientes clienteVista = control_Clientes.ConsultarIdCliente_vista(factura.idCliente);
            if (cliente == null || clienteVista == null || !cliente.diaPago.HasValue)
            {
                return;
            }

            int periodicidad = clienteVista.periodicidadMeses <= 0 ? 1 : clienteVista.periodicidadMeses;
            cliente.fechaUltimoPago = fechaPago.Date;
            cliente.fechaProximoPago = ClassCartera.CalcularProximaFechaPago(fechaPago.Date, cliente.diaPago.Value, periodicidad);
            control_Clientes.Crud(cliente, 1);
        }

        protected async System.Threading.Tasks.Task EnviarConfirmacionPago(V_Facturas factura)
        {
            try
            {
                ConfigWhatsAppMeta configMeta = control_ConfigWhatsAppMeta.Consultar();
                if (configMeta == null ||
                    string.IsNullOrWhiteSpace(configMeta.accessToken) ||
                    string.IsNullOrWhiteSpace(configMeta.phoneNumberId))
                {
                    return;
                }

                WhatsAppResponse appResponse = await confirmacin_de_pago.plantilla(
                    factura.celular,
                    factura.nombreComercial,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    $"{factura.valorAPagar:N0}",
                    factura.nombreMes,
                    configMeta.accessToken,
                    configMeta.phoneNumberId);

                App_WhatsApp.Messages messages = appResponse?.messages?.FirstOrDefault();
                string estado = messages?.message_status;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        protected void Mensage(int estilo, string titulo, string mensage, string tipo, string aspx)
        {
            if (estilo == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta('" + titulo + "','" + mensage + "','" + tipo + "','" + aspx + "');", true);
            }
            if (estilo == 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
            }
        }
        #endregion

        protected void ddl_Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_Mes.SelectedItem.Value) > 0)
            {
                Session["month"] = Convert.ToInt32(ddl_Mes.SelectedItem.Value);
            }
        }
    }
}

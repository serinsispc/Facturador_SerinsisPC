using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class clientes_editar : System.Web.UI.Page
    {
        protected int IdCliente
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
            }
        }

        protected bool EsNuevo => IdCliente <= 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTipoPlan();
                litTitulo.Text = EsNuevo ? "Nuevo Cliente" : "Editar Cliente";
                btnGuardar.Text = EsNuevo ? "Crear Cliente" : "Guardar Cambios";

                if (EsNuevo)
                {
                    LimpiarCampos();
                }
                else
                {
                    CargarCliente(IdCliente);
                }
            }
        }

        protected void CargarTipoPlan()
        {
            ddl_TipoPlan.Items.Clear();
            List<TipoPlan> tiposPlan = control_TipoPlan.ListaCompleta() ?? new List<TipoPlan>();
            foreach (TipoPlan tipoPlan in tiposPlan)
            {
                ListItem item = new ListItem(tipoPlan.nombrePlan, tipoPlan.id.ToString());
                item.Attributes["data-periodicidad"] = (tipoPlan.periodicidadMeses <= 0 ? 1 : tipoPlan.periodicidadMeses).ToString(CultureInfo.InvariantCulture);
                ddl_TipoPlan.Items.Add(item);
            }
        }

        protected void LimpiarCampos()
        {
            txtNit.Text = string.Empty;
            txtNombreComersial.Text = string.Empty;
            txtRepresentante.Text = string.Empty;
            txtWhatSapp.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtSedes.Text = string.Empty;
            txtDiaPago.Text = "5";
            txtFechaInicioPlan.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFechaProximoPago.Text = ClassCartera.CalcularProximoPagoInicial(DateTime.Today, 5, 1).ToString("yyyy-MM-dd");
            txtObservacionCartera.Text = string.Empty;
            MostrarEstado(string.Empty, false);
        }

        protected void CargarCliente(int idCliente)
        {
            V_Clientes cliente = control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (cliente == null)
            {
                MostrarEstado("No fue posible cargar el cliente.", true);
                return;
            }

            ddl_TipoPlan.SelectedValue = Convert.ToString(cliente.idTipoPlan);
            txtNit.Text = cliente.nit;
            txtNombreComersial.Text = cliente.nombreComercial;
            txtRepresentante.Text = cliente.nombreRepresentate;
            txtWhatSapp.Text = cliente.celular;
            txtValor.Text = Convert.ToString(Convert.ToInt32(cliente.valorPlan));
            txtCorreo.Text = cliente.correo;
            txtSedes.Text = Convert.ToString(cliente.sedes);
            txtDiaPago.Text = Convert.ToString(cliente.diaPago ?? 5);
            txtFechaInicioPlan.Text = cliente.fechaInicioPlan.HasValue ? cliente.fechaInicioPlan.Value.ToString("yyyy-MM-dd") : string.Empty;
            txtFechaProximoPago.Text = cliente.fechaProximoPago.HasValue ? cliente.fechaProximoPago.Value.ToString("yyyy-MM-dd") : string.Empty;
            txtObservacionCartera.Text = cliente.observacionCartera;
            MostrarEstado(string.Empty, false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerificarCampos())
                {
                    MostrarEstado("Completa todos los campos obligatorios.", true);
                    return;
                }

                int accion = EsNuevo ? 0 : 1;
                bool guardado = GestionarCliente(accion, IdCliente);
                if (!guardado)
                {
                    MostrarEstado(ViewState["UltimoMensajeGuardar"] as string ?? "La base de datos no confirmo el guardado del cliente.", true);
                    return;
                }

                Response.Redirect("clientes.aspx?guardado=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarEstado("Error al guardar: " + ex.Message, true);
            }
        }

        protected bool VerificarCampos()
        {
            return ddl_TipoPlan.SelectedItem != null &&
                   LimpiarSoloDigitos(txtNit.Text) != string.Empty &&
                   txtNombreComersial.Text != string.Empty &&
                   txtRepresentante.Text != string.Empty &&
                   LimpiarSoloDigitos(txtWhatSapp.Text) != string.Empty &&
                   txtValor.Text != string.Empty &&
                   txtCorreo.Text != string.Empty &&
                   txtSedes.Text != string.Empty &&
                   txtDiaPago.Text != string.Empty &&
                   txtFechaInicioPlan.Text != string.Empty;
        }

        protected int ObtenerPeriodicidadPlan()
        {
            int idTipoPlan = Convert.ToInt32(ddl_TipoPlan.SelectedValue);
            TipoPlan tipoPlan = control_TipoPlan.ListaCompleta().FirstOrDefault(x => x.id == idTipoPlan);
            return tipoPlan == null || tipoPlan.periodicidadMeses <= 0 ? 1 : tipoPlan.periodicidadMeses;
        }

        protected bool GestionarCliente(int accion, int idCliente)
        {
            Clientes cliente = control_Clientes.ConsultarIdCliente(idCliente);
            if (cliente != null && accion == 0)
            {
                return false;
            }

            if (accion == 0 || cliente == null)
            {
                cliente = new Clientes();
                idCliente = 0;
            }

            int diaPago = Convert.ToInt32(txtDiaPago.Text);
            DateTime fechaInicioPlan = ParseFechaFormulario(txtFechaInicioPlan.Text);
            DateTime? fechaUltimoPago = cliente.fechaUltimoPago;
            int periodicidadMeses = ObtenerPeriodicidadPlan();
            DateTime fechaProximoPago = fechaUltimoPago.HasValue
                ? ClassCartera.CalcularProximaFechaPago(fechaUltimoPago.Value, diaPago, periodicidadMeses)
                : ClassCartera.CalcularProximoPagoInicial(fechaInicioPlan, diaPago, periodicidadMeses);

            cliente.id = idCliente;
            cliente.idTipoPlan = Convert.ToInt32(ddl_TipoPlan.SelectedValue);
            cliente.nit = LimpiarSoloDigitos(txtNit.Text);
            cliente.nombreComercial = txtNombreComersial.Text.ToUpper();
            cliente.nombreRepresentate = txtRepresentante.Text.ToUpper();
            cliente.celular = LimpiarSoloDigitos(txtWhatSapp.Text);
            cliente.correo = txtCorreo.Text;
            cliente.sedes = Convert.ToInt32(txtSedes.Text);
            cliente.valorPlan = Convert.ToDecimal(txtValor.Text);
            cliente.estado = accion == 0 ? 1 : (cliente.estado == 0 ? 0 : 1);
            cliente.diaPago = diaPago;
            cliente.fechaInicioPlan = fechaInicioPlan;
            cliente.fechaUltimoPago = fechaUltimoPago;
            cliente.fechaProximoPago = fechaProximoPago;
            cliente.observacionCartera = txtObservacionCartera.Text;
            txtFechaProximoPago.Text = fechaProximoPago.ToString("yyyy-MM-dd");

            RespuestaSQL respuesta = control_Clientes.Crud(cliente, accion);
            ViewState["UltimoMensajeGuardar"] = respuesta?.mensaje;
            return respuesta != null && respuesta.respuesta;
        }

        protected void MostrarEstado(string mensaje, bool esError)
        {
            lblEstadoGuardar.Text = mensaje;
            lblEstadoGuardar.CssClass = string.IsNullOrWhiteSpace(mensaje)
                ? "cliente-edit-status"
                : "cliente-edit-status cliente-edit-status--show " + (esError ? "cliente-edit-status--error" : "cliente-edit-status--ok");
        }

        protected string LimpiarSoloDigitos(string valor)
        {
            return new string((valor ?? string.Empty).Where(char.IsDigit).ToArray());
        }

        protected DateTime ParseFechaFormulario(string valor)
        {
            string[] formatos = { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "M/d/yyyy" };
            DateTime fecha;

            if (DateTime.TryParseExact(valor, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                return fecha;
            }

            if (DateTime.TryParse(valor, new CultureInfo("es-CO"), DateTimeStyles.None, out fecha))
            {
                return fecha;
            }

            if (DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                return fecha;
            }

            throw new FormatException("La fecha de inicio del plan no tiene un formato valido.");
        }
    }
}

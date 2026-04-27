using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Facturador_SerinsisPC
{
    public partial class clientes : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            RestaurarEstadoModales();

            if (!IsPostBack)
            {
                CargarListaClientes();
                CargarTipoPlan();
            }
        }

        protected void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("clientes_editar.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnCerrarModal1_Click(object sender, EventArgs e)
        {
            OcultarModalCliente();
        }

        protected void btnCerrarModal2_Click(object sender, EventArgs e)
        {
            OcultarModalCliente();
        }

        protected void btnCerrarModalDB_Click(object sender, EventArgs e)
        {
            OcultarModalDB();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                lblEstadoGuardar.Visible = true;
                lblEstadoGuardar.Text = "Entrando a guardar...";

                if (VerificarCampos())
                {
                    int boton = btnGuardar.Text == "Crear" ? 0 : 1;
                    bool respuesta = GestionarCliente(boton, Convert.ToInt32(ViewState["idCliente"]));
                    if (respuesta)
                    {
                        CargarListaClientes();
                        OcultarModalCliente();
                        Mensage(1, "Ok", "Cliente gestionado correctamente.", "success", "clientes.aspx");
                    }
                    else
                    {
                        MostrarModalCliente();
                        lblEstadoGuardar.Text = "El procedimiento de base de datos respondio false.";
                        Mensage(1, "Error", "El proceso termino con error al guardar en base de datos.", "error", "clientes.aspx");
                    }
                }
                else
                {
                    MostrarModalCliente();
                    lblEstadoGuardar.Text = "Validacion local fallida: revisa NIT, WhatsApp, valor, sedes, dia de pago e inicio del plan.";
                    Mensage(2, "Error", "Aun se encuentran campos vacios o datos de pago sin definir.", "error", "");
                }
            }
            catch (Exception ex)
            {
                MostrarModalCliente();
                lblEstadoGuardar.Visible = true;
                lblEstadoGuardar.Text = "Excepcion en guardar: " + ex.Message;
                Mensage(2, "Error", "Fallo btnGuardar_Click: " + ex.Message.Replace("'", ""), "error", "");
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Response.Redirect("clientes_editar.aspx?id=" + id, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"] = id;
            OcultarModalCliente();
            if (GestionarCliente(2, id))
            {
                Mensage(1, "Ok", "Cliente gestionado correctamente.", "success", "clientes.aspx");
            }
            else
            {
                Mensage(1, "Error", "El proceso termino con error.", "error", "clientes.aspx");
            }
        }

        protected void btnEstado_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"] = id;
            Clientes clientes = control_Clientes.ConsultarIdCliente(id);
            if (clientes == null)
            {
                Mensage(2, "Error", "No fue posible consultar la informacion del cliente.", "error", "");
                return;
            }

            clientes.estado = clientes.estado == 0 ? 1 : 0;
            RespuestaSQL respuesta = control_Clientes.Crud(clientes, 1);
            if (respuesta == null || !respuesta.respuesta)
            {
                Mensage(2, "Error", "No fue posible actualizar el estado del cliente.", "error", "");
                return;
            }

            CargarListaClientes();
            Mensage(2, "Ok", clientes.estado == 1 ? "Cliente activado correctamente." : "Cliente inactivado correctamente.", "success", "");
        }

        protected void btnAgregarDB_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idClienteDB"] = id;
            CargarModalDB(id);
            MostrarModalDB();
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            if (ViewState["idClienteDB"] == null)
            {
                return;
            }

            int idCliente = Convert.ToInt32(ViewState["idClienteDB"]);
            DatBases datBases = new DatBases
            {
                idCliente = idCliente,
                nameDataBase = ddl_db.SelectedItem.Text,
                estado = 1
            };

            string query = $"exec InsertInto_DatBases {datBases.idCliente},'{datBases.nameDataBase}',{datBases.estado}";
            JsonConvert.DeserializeObject<List<RespuestaSQL>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query)).FirstOrDefault();

            CargarModalDB(idCliente);
            MostrarModalDB();
        }

        protected void btnEliminarDB_Click(object sender, EventArgs e)
        {
            if (ViewState["idClienteDB"] == null)
            {
                return;
            }

            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            try
            {
                string query = $"exec Delete_DatBases {id}";
                JsonConvert.DeserializeObject<List<RespuestaSQL>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query)).FirstOrDefault();
                int idCliente = Convert.ToInt32(ViewState["idClienteDB"]);
                CargarModalDB(idCliente);
                MostrarModalDB();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        protected void btnToggleDB_Click(object sender, EventArgs e)
        {
            if (ViewState["idClienteDB"] == null)
            {
                return;
            }

            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            try
            {
                string queryConsulta = $"select *from DatBases where id={id}";
                DatBases datBase = JsonConvert.DeserializeObject<List<DatBases>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, queryConsulta)).FirstOrDefault();
                if (datBase == null)
                {
                    Mensage(2, "Error", "No fue posible consultar la base de datos seleccionada.", "error", "");
                    return;
                }

                int nuevoEstado = datBase.estado == 1 ? 0 : 1;
                CambiarEstadoBaseServidor(datBase.nameDataBase, nuevoEstado == 1);
                int estadoReal = ObtenerEstadoBaseServidor(datBase.nameDataBase);

                string queryUpdate = $"update DatBases set estado={estadoReal} where id={id}";
                DBEntities.EjecutarSQLServer(DBEntities.connectionString, queryUpdate);

                int idCliente = Convert.ToInt32(ViewState["idClienteDB"]);
                CargarModalDB(idCliente);
                MostrarModalDB();
                Mensage(2, "Ok", estadoReal == 1 ? "Base de datos marcada como online." : "Base de datos marcada como offline.", "success", "");
            }
            catch (Exception ex)
            {
                Mensage(2, "Error", "No fue posible cambiar el estado de la base: " + ex.Message.Replace("'", ""), "error", "");
            }
            finally
            {
                ClassConexionPrincipal.Configurar();
            }
        }
        #endregion

        #region Funciones
        protected void CargarListaClientes()
        {
            rpClientes.DataSource = control_Clientes.ListaCompleta();
            rpClientes.DataBind();
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

        protected void LimpiarCamposModal()
        {
            lblEstadoGuardar.Text = string.Empty;
            lblEstadoGuardar.Visible = false;
            txtNit.Text = string.Empty;
            txtNombreComersial.Text = string.Empty;
            txtRepresentante.Text = string.Empty;
            txtWhatSapp.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtSedes.Text = string.Empty;
            txtDiaPago.Text = "5";
            txtFechaInicioPlan.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFechaProximoPago.Text = ClassCartera.CalcularPrimeraFechaPago(DateTime.Today, 5).ToString("yyyy-MM-dd");
            txtObservacionCartera.Text = string.Empty;
        }

        protected void CargarModalDB(int idCliente)
        {
            V_Clientes vCliente = control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (vCliente != null)
            {
                lblClienteDB.Text = string.IsNullOrWhiteSpace(vCliente.nombreComercial)
                    ? vCliente.nombreRepresentate
                    : vCliente.nombreComercial;
            }

            CargarListaDBDisponibles();
            CargarDBCliente(idCliente);
        }

        protected void CargarListaDBDisponibles()
        {
            try
            {
                string query = "select *from ListaDB";
                List<ListaDB> listaDB = JsonConvert.DeserializeObject<List<ListaDB>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query));
                if (listaDB.Count > 0)
                {
                    ddl_db.DataValueField = "database_id";
                    ddl_db.DataTextField = "name";
                    ddl_db.DataSource = listaDB;
                    ddl_db.DataBind();
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
        }

        protected void CargarDBCliente(int idCliente)
        {
            string query = $"select *from DatBases where idCliente={idCliente}";
            List<DatBases> datBases = JsonConvert.DeserializeObject<List<DatBases>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query));

            foreach (DatBases datBase in datBases ?? new List<DatBases>())
            {
                try
                {
                    int estadoReal = ObtenerEstadoBaseServidor(datBase.nameDataBase);
                    if (datBase.estado != estadoReal)
                    {
                        datBase.estado = estadoReal;
                        string queryUpdate = $"update DatBases set estado={estadoReal} where id={datBase.id}";
                        DBEntities.EjecutarSQLServer(DBEntities.connectionString, queryUpdate);
                    }
                }
                catch
                {
                    // Si no se puede consultar el servidor, mantenemos el ultimo estado conocido.
                }
                finally
                {
                    ClassConexionPrincipal.Configurar();
                }
            }

            rpDB_Cliente.DataSource = datBases;
            rpDB_Cliente.DataBind();
        }

        protected bool VerificarCampos()
        {
            return ddl_TipoPlan.SelectedItem != null &&
                   LimpiarSoloDigitos(txtNit.Text) != string.Empty &&
                   txtNombreComersial.Text != string.Empty &&
                   txtRepresentante.Text != string.Empty &&
                   txtWhatSapp.Text != string.Empty &&
                   txtValor.Text != string.Empty &&
                   txtCorreo.Text != string.Empty &&
                   txtSedes.Text != string.Empty &&
                   txtDiaPago.Text != string.Empty &&
                   txtFechaInicioPlan.Text != string.Empty;
        }

        protected void CargarCamposModals(int idCliente)
        {
            V_Clientes vCliente = control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (vCliente != null)
            {
                lblEstadoGuardar.Text = string.Empty;
                lblEstadoGuardar.Visible = false;
                ddl_TipoPlan.SelectedValue = Convert.ToString(vCliente.idTipoPlan);
                txtNit.Text = vCliente.nit;
                txtNombreComersial.Text = vCliente.nombreComercial;
                txtRepresentante.Text = vCliente.nombreRepresentate;
                txtWhatSapp.Text = vCliente.celular;
                txtValor.Text = Convert.ToString(Convert.ToInt32(vCliente.valorPlan));
                txtCorreo.Text = vCliente.correo;
                txtSedes.Text = Convert.ToString(vCliente.sedes);
                txtDiaPago.Text = Convert.ToString(vCliente.diaPago ?? 5);
                txtFechaInicioPlan.Text = vCliente.fechaInicioPlan.HasValue ? vCliente.fechaInicioPlan.Value.ToString("yyyy-MM-dd") : string.Empty;
                txtFechaProximoPago.Text = vCliente.fechaProximoPago.HasValue ? vCliente.fechaProximoPago.Value.ToString("yyyy-MM-dd") : string.Empty;
                txtObservacionCartera.Text = vCliente.observacionCartera;
                btnGuardar.Text = "Editar";
                MostrarModalCliente();
            }
        }

        protected void RestaurarEstadoModales()
        {
            panelModal.Style["display"] = Convert.ToBoolean(ViewState["ModalClienteVisible"] ?? false) ? "flex" : "none";
            panelModalDB.Style["display"] = Convert.ToBoolean(ViewState["ModalDBVisible"] ?? false) ? "flex" : "none";
        }

        protected void MostrarModalCliente()
        {
            ViewState["ModalClienteVisible"] = true;
            panelModal.Style["display"] = "flex";
        }

        protected void OcultarModalCliente()
        {
            ViewState["ModalClienteVisible"] = false;
            panelModal.Style["display"] = "none";
        }

        protected void MostrarModalDB()
        {
            ViewState["ModalDBVisible"] = true;
            panelModalDB.Style["display"] = "flex";
        }

        protected void OcultarModalDB()
        {
            ViewState["ModalDBVisible"] = false;
            panelModalDB.Style["display"] = "none";
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

        protected int ObtenerPeriodicidadPlan()
        {
            int idTipoPlan = Convert.ToInt32(ddl_TipoPlan.SelectedValue);
            TipoPlan tipoPlan = control_TipoPlan.ListaCompleta().FirstOrDefault(x => x.id == idTipoPlan);
            return tipoPlan == null || tipoPlan.periodicidadMeses <= 0 ? 1 : tipoPlan.periodicidadMeses;
        }

        protected bool GestionarCliente(int boton, int idCliente)
        {
            Clientes clientes = control_Clientes.ConsultarIdCliente(idCliente);
            if (clientes != null && boton == 0)
            {
                return false;
            }

            if (boton == 0 || clientes == null)
            {
                clientes = new Clientes();
                idCliente = 0;
            }

            if (boton == 2)
            {
                clientes.id = idCliente;
                RespuestaSQL respuestaEliminar = control_Clientes.Crud(clientes, boton);
                return respuestaEliminar != null && respuestaEliminar.respuesta;
            }

            int diaPago = Convert.ToInt32(txtDiaPago.Text);
            DateTime fechaInicioPlan = ParseFechaFormulario(txtFechaInicioPlan.Text);
            DateTime? fechaUltimoPago = clientes.fechaUltimoPago;
            int periodicidadMeses = ObtenerPeriodicidadPlan();
            DateTime fechaProximoPago = fechaUltimoPago.HasValue
                ? ClassCartera.CalcularProximaFechaPago(fechaUltimoPago.Value, diaPago, periodicidadMeses)
                : ClassCartera.CalcularProximoPagoInicial(fechaInicioPlan, diaPago, periodicidadMeses);

            clientes.id = idCliente;
            clientes.idTipoPlan = Convert.ToInt32(ddl_TipoPlan.SelectedItem.Value);
            clientes.nit = LimpiarSoloDigitos(txtNit.Text);
            clientes.nombreComercial = txtNombreComersial.Text.ToUpper();
            clientes.nombreRepresentate = txtRepresentante.Text.ToUpper();
            clientes.celular = txtWhatSapp.Text;
            clientes.correo = txtCorreo.Text;
            clientes.sedes = Convert.ToInt32(txtSedes.Text);
            clientes.valorPlan = Convert.ToDecimal(txtValor.Text);
            clientes.estado = boton == 0 ? 1 : (clientes.estado == 0 ? 0 : 1);
            clientes.diaPago = diaPago;
            clientes.fechaInicioPlan = fechaInicioPlan;
            clientes.fechaUltimoPago = fechaUltimoPago;
            clientes.fechaProximoPago = fechaProximoPago;
            clientes.observacionCartera = txtObservacionCartera.Text;
            txtFechaProximoPago.Text = fechaProximoPago.ToString("yyyy-MM-dd");

            RespuestaSQL respuesta = control_Clientes.Crud(clientes, boton);
            return respuesta != null && respuesta.respuesta;
        }

        protected string LimpiarSoloDigitos(string valor)
        {
            return new string((valor ?? string.Empty).Where(char.IsDigit).ToArray());
        }

        protected DateTime ParseFechaFormulario(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new FormatException("La fecha de inicio del plan es obligatoria.");
            }

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

        protected void CambiarEstadoBaseServidor(string nombreBase, bool activar)
        {
            string nombreSeguro = NormalizarNombreBase(nombreBase);
            string accion = activar
                ? $"ALTER DATABASE [{nombreSeguro}] SET ONLINE"
                : $"ALTER DATABASE [{nombreSeguro}] SET OFFLINE WITH ROLLBACK IMMEDIATE";

            DBEntities.ConexionBase("www.serinsispc.com", "master", "emilianop", "Ser1ns1s@2020*");
            DBEntities.EjecutarSQLServer(DBEntities.connectionString, accion);
        }

        protected int ObtenerEstadoBaseServidor(string nombreBase)
        {
            string nombreSeguro = NormalizarNombreBase(nombreBase);
            string query = "select name, state_desc from sys.databases where name='" + nombreSeguro + "'";

            DBEntities.ConexionBase("www.serinsispc.com", "master", "emilianop", "Ser1ns1s@2020*");
            List<EstadoBaseServidor> estados = JsonConvert.DeserializeObject<List<EstadoBaseServidor>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query));
            EstadoBaseServidor estado = estados.FirstOrDefault();
            if (estado == null)
            {
                throw new InvalidOperationException("La base de datos no existe en el servidor.");
            }

            return string.Equals((estado.state_desc ?? string.Empty).Trim(), "ONLINE", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        }

        protected string NormalizarNombreBase(string nombreBase)
        {
            string valor = (nombreBase ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new InvalidOperationException("La base de datos no tiene un nombre valido.");
            }

            if (!Regex.IsMatch(valor, @"^[A-Za-z0-9_]+$"))
            {
                throw new InvalidOperationException("El nombre de la base de datos contiene caracteres no permitidos.");
            }

            return valor;
        }
        #endregion
    }
}

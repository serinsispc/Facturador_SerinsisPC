using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using RFacturacionElectronicaDIAN.Entities.Request;
using RFacturacionElectronicaDIAN.Entities.Response;
using RFacturacionElectronicaDIAN.Factories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC","emilianop","Ser1ns1s@2020*");
                CargarRP("pendiente");
            }
        }
        protected void CargarRP(string estado)
        {
            rpCuentas.DataSource = controladorCuentas.GetClientes(estado);
            rpCuentas.DataBind();
        }
        protected void btnSustemderTodo_Click(object sender, EventArgs e)
        {
            EjecutarCambioMasivo("pendiente", 2);
        }

        protected void btnAvisoTodos_Click(object sender, EventArgs e)
        {
            EjecutarCambioMasivo("pendiente", 1);
        }

        protected void btnReconectarTodos_Click(object sender, EventArgs e)
        {
            EjecutarCambioMasivo("pendiente", 0);
        }

        protected bool ModificarEstadoCuenta(string dataBase, string estado, string mensaje, int IdCliente)
        {
            try
            {
                /*lo primero es consultar la cantidad de bases de datos que tiene el usuario*/
                List<DatBases> datBases = new List<DatBases>();
                string query = $"select *from DatBases where idCliente={IdCliente}";
                datBases=JsonConvert.DeserializeObject<List<DatBases>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString,query));
                if (datBases.Count > 0)
                {
                    foreach(DatBases lista in datBases)
                    {
                        DBEntities.ConexionBase("www.serinsispc.com", lista.nameDataBase, "emilianop", "Ser1ns1s@2020*");
                        string mensajeSeguro = (mensaje ?? string.Empty).Replace("'", "''");
                        query = $"update AdminControl set tipo_admincontrol={estado},mensaje_admincontrol='{mensajeSeguro}'";
                        string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString,query);
                        /*actualizamos el estado en la tabla local*/
                        DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
                        query = $"update DatBases set estado={estado} where id={lista.id}";
                        string respuesta2 = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                    }
                }
                DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
                return true;
            }
            catch(Exception ex)
            {
                string erro = ex.Message;
                DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
                return false;
            }
        }

        protected void btnSuspender_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            EjecutarCambioPorCliente(idCliente, 2);
        }

        protected void btnConectar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            EjecutarCambioPorCliente(idCliente, 0);
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

        protected void btnAbiso_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            EjecutarCambioPorCliente(idCliente, 1);
        }

        protected void btnAvisoSeleccionados_Click(object sender, EventArgs e)
        {
            EjecutarCambioSeleccionados(1);
        }

        protected void btnSuspenderSeleccionados_Click(object sender, EventArgs e)
        {
            EjecutarCambioSeleccionados(2);
        }

        protected void btnReconectarSeleccionados_Click(object sender, EventArgs e)
        {
            EjecutarCambioSeleccionados(0);
        }

        protected void EjecutarCambioMasivo(string estadoFiltro, int nuevoEstado)
        {
            List<EstadoCuentaClientes> estados = controladorCuentas.GetClientes(estadoFiltro) ?? new List<EstadoCuentaClientes>();
            ProcesarCambioEstado(estados, nuevoEstado, true);
        }

        protected void EjecutarCambioPorCliente(int idCliente, int nuevoEstado)
        {
            List<EstadoCuentaClientes> estados = controladorCuentas.Consultar_idCliente(idCliente) ?? new List<EstadoCuentaClientes>();
            ProcesarCambioEstado(estados, nuevoEstado, false);
        }

        protected void EjecutarCambioSeleccionados(int nuevoEstado)
        {
            List<int> idsSeleccionados = ObtenerClientesSeleccionados();
            if (idsSeleccionados.Count == 0)
            {
                Mensage(2, "Sin selección", "Selecciona al menos un cliente para ejecutar la acción masiva.", "warning", "");
                return;
            }

            List<EstadoCuentaClientes> estados = new List<EstadoCuentaClientes>();
            foreach (int idCliente in idsSeleccionados.Distinct())
            {
                EstadoCuentaClientes estado = controladorCuentas.Consultar_idCliente(idCliente)?.FirstOrDefault();
                if (estado != null)
                {
                    estados.Add(estado);
                }
            }

            ProcesarCambioEstado(estados, nuevoEstado, false);
        }

        protected List<int> ObtenerClientesSeleccionados()
        {
            List<int> idsSeleccionados = new List<int>();
            foreach (RepeaterItem item in rpCuentas.Items)
            {
                CheckBox chkSeleccionar = item.FindControl("chkSeleccionar") as CheckBox;
                HiddenField hfIdCliente = item.FindControl("hfIdCliente") as HiddenField;

                if (chkSeleccionar != null && chkSeleccionar.Checked && hfIdCliente != null)
                {
                    if (int.TryParse(hfIdCliente.Value, out int idCliente))
                    {
                        idsSeleccionados.Add(idCliente);
                    }
                }
            }

            return idsSeleccionados;
        }

        protected void ProcesarCambioEstado(List<EstadoCuentaClientes> estados, int nuevoEstado, bool esMasivo)
        {
            if (estados == null || estados.Count == 0)
            {
                Mensage(2, "Sin resultados", "No se encontraron cuentas para actualizar.", "warning", "");
                return;
            }

            int clientesActualizados = 0;
            int basesActualizadas = 0;

            foreach (EstadoCuentaClientes estadoCuenta in estados)
            {
                List<DatBases> bases = controladorDatBases.ListdatBases(estadoCuenta.id) ?? new List<DatBases>();
                if (bases.Count == 0)
                {
                    continue;
                }

                string mensaje = ConstruirMensajeCorporativo(estadoCuenta, nuevoEstado, bases.Count);
                bool actualizado = ModificarEstadoCuenta(string.Empty, nuevoEstado.ToString(), mensaje, estadoCuenta.id);
                if (actualizado)
                {
                    clientesActualizados++;
                    basesActualizadas += bases.Count;
                }
            }

            MostrarResultadoCambio(nuevoEstado, esMasivo, clientesActualizados, basesActualizadas);
        }

        protected string ConstruirMensajeCorporativo(EstadoCuentaClientes estadoCuenta, int nuevoEstado, int totalBases)
        {
            string nombreCliente = (estadoCuenta.nombreRepresentate ?? string.Empty).Trim();
            string comercio = (estadoCuenta.nombreComercial ?? string.Empty).Trim();
            string saldo = $"{estadoCuenta.total:C0}";
            string salto = Environment.NewLine + Environment.NewLine;
            string alcance = totalBases > 1
                ? $"Esta medida aplica para las {totalBases} bases de datos asociadas a su cuenta."
                : "Esta medida aplica para la base de datos asociada a su cuenta.";

            if (nuevoEstado == 1)
            {
                return ConstruirMensajeEstructurado(
                    nombreCliente,
                    $"Desde SERINSIS PC S.A.S. le informamos que el establecimiento {comercio} presenta un saldo pendiente por {saldo} correspondiente al servicio de software POS.",
                    $"Su cuenta ha sido registrada en estado de aviso preventivo. {alcance}",
                    "Le agradecemos realizar el pago antes del día 5 del mes en curso en la cuenta de ahorros Bancolombia 454-000044-36 y enviar el comprobante de consignación únicamente al WhatsApp 3144628361.");
            }

            if (nuevoEstado == 2)
            {
                return ConstruirMensajeEstructurado(
                    nombreCliente,
                    $"Desde SERINSIS PC S.A.S. le informamos que el servicio de software POS del establecimiento {comercio} ha sido suspendido temporalmente por mora en el pago y actualmente registra un saldo pendiente por {saldo}.",
                    $"{alcance}",
                    "Para gestionar la reactivación del servicio, por favor realice el pago en la cuenta de ahorros Bancolombia 454-000044-36 y envíe el comprobante de consignación únicamente al WhatsApp 3144628361.");
            }

            return ConstruirMensajeEstructurado(
                nombreCliente,
                $"Desde SERINSIS PC S.A.S. le confirmamos que el servicio de software POS del establecimiento {comercio} ha sido restablecido correctamente.",
                $"{alcance}",
                "Agradecemos su atención y confianza en nuestros servicios.");
        }

        protected string ConstruirMensajeEstructurado(string nombreCliente, string parrafo1, string parrafo2, string parrafo3)
        {
            string salto = Environment.NewLine + Environment.NewLine;
            return
                $"Estimado(a) {nombreCliente}:{salto}" +
                $"{parrafo1}{salto}" +
                $"{parrafo2}{salto}" +
                $"{parrafo3}";
        }

        protected void MostrarResultadoCambio(int nuevoEstado, bool esMasivo, int clientesActualizados, int basesActualizadas)
        {
            if (clientesActualizados == 0)
            {
                Mensage(1, "Sin cambios", "No fue posible actualizar el estado de las cuentas seleccionadas.", "warning", "index.aspx");
                return;
            }

            string accion = "actualizar";
            if (nuevoEstado == 1)
            {
                accion = "registrar en aviso";
            }
            else if (nuevoEstado == 2)
            {
                accion = "suspender";
            }
            else if (nuevoEstado == 0)
            {
                accion = "reconectar";
            }

            string detalle = $"Se logró {accion} {clientesActualizados} cliente(s) y {basesActualizadas} base(s) de datos asociadas.";
            string titulo = esMasivo ? "Proceso masivo completado" : "Proceso completado";
            Mensage(1, titulo, detalle, "success", "index.aspx");
        }
    }
}

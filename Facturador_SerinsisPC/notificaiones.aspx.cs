using Facturador_SerinsisPC.Email;
using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using RFacturacionElectronicaDIAN.Entities.Request;
using RFacturacionElectronicaDIAN.Entities.Response;
using RFacturacionElectronicaDIAN.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace Facturador_SerinsisPC
{
    public partial class notificaiones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*En esta parte cargamos la lista de menajes disponibles para enviar */
                Cargar_DG();
            }
        }
        int mensajesEnviados = 0;
        protected void Cargar_DG()
        {
            rpMensajes.DataSource = controladorMensajes.ConsultarLista();
            rpMensajes.DataBind();
        }
        protected void btnNuevoMensaje_Click(object sender, EventArgs e)
        {
            panelModalMensajes.Visible = true;
            txtTitulo.Focus();
        }

        protected void btnWhatSapp_Click(object sender, EventArgs e)
        {
            string mensajeWhatsApp = string.Empty;
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Mensajes mensajes = new Mensajes();
            mensajes = controladorMensajes.Consultar_id(id);
            if (mensajes != null)
            {    
                /*En esta parte consultamos las facturas pendientes por pago*/
                List<V_Facturas> v_Facturas = new List<V_Facturas>();
                v_Facturas = control_Facturas.Lista_Estado(1);
                if(v_Facturas != null)
                {
                    ViewState["contador"] = v_Facturas.Count();
                    ViewState["mensajesEnviados"] = 0;
                    foreach (V_Facturas v_ in v_Facturas)
                    {
                        string mensaje = CobroServicioMensualidad.Plantilla(v_.nombreRepresentate,$"1/{DateTime.Now.Month}/{DateTime.Now.Year}", $"1/{DateTime.Now.Month+1}/{DateTime.Now.Year}",$"{v_.valorAPagar:C0}");

                        List<CorreosNotificaciones> coreos = new List<CorreosNotificaciones>();
                        coreos.Add(new CorreosNotificaciones { id = 1, email = v_.correo });

                        Correo.CrearCuerpoCorreo("SERINSIS PC S.A.S.", coreos, $"Recordatorio Pago Software Facturacion Electrónica - {v_.nombreComercial}", mensaje);
                        Correo.Enviar();
                    }
                    Mensage(1, "¡Ok!", $"¡{ViewState["contador"]} Mensajes enviados...!", "success", "notificaiones.aspx");
                }
            }
        }

        protected void btnGuardarMensaje_Click(object sender, EventArgs e)
        {
            if(txtTitulo.Text !=string.Empty &&
               txtMensaje.Text != string.Empty &&
               txtVariables.Text!=string.Empty)
            {
                Mensajes mensajes = new Mensajes();
                mensajes.id = 0;
                mensajes.nombreMensaje= txtTitulo.Text;
                mensajes.mensajeText= txtMensaje.Text;
                mensajes.variables = Convert.ToInt32(txtVariables.Text);
                if (controladorMensajes.Crud(mensajes, 0).respuesta)
                {
                    Mensage(1, "¡Ok!", "¡Mensaje Guardado correctamente...!", "success", "notificaiones.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", "¡Mensaje no se Guardado correctamente...!", "error", "notificaiones.aspx");
                }
            }
            else
            {
                Mensage(2, "¡Error!", "¡Aun se encuentran capos vacíos...!", "error", "");
            }
        }

        protected void btnCerrarModalBuscarCliente2_Click(object sender, EventArgs e)
        {
            panelModalMensajes.Visible = false;
        }

        protected void btnCerrarModalBuscarCliente_Click(object sender, EventArgs e)
        {
            panelModalMensajes.Visible = false;
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

        protected void btnEliminarMensaje_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Mensajes mensajes = new Mensajes();
            mensajes=controladorMensajes.Consultar_id(id);
            if(mensajes!=null)
            {
                if (controladorMensajes.Crud(mensajes, 2).respuesta)
                {
                    Mensage(1, "¡Ok!", "¡Mensaje Eliminado correctamente...!", "success", "notificaiones.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", "¡Mensaje no se Elimino correctamente...!", "error", "notificaiones.aspx");
                }
            }
        }
    }
}
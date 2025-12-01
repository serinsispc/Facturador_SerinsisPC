using App_WhatsApp;
using App_WhatsApp.Plantillas;
using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using RFacturacionElectronicaDIAN.Entities.Request;
using RFacturacionElectronicaDIAN.Entities.Response;
using RFacturacionElectronicaDIAN.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class facturar : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int totalPendiente = controlador_V_CobroWhatsApp.totalPendiente();
                ViewState["ValorPendiente"] = totalPendiente;
                Cargar_rpFacturas();
                CargarMeses();
            }
            if (Session["month"] != null) ddl_Mes.SelectedValue =$"{Session["month"]}";
            if (Session["year"] != null) txtyear.Text=$"{Session["year"]}";
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
                    Mensage(1, "¡Ok!", "¡La factura fue gestionada correctamente...!", "success", "facturar.aspx");
                }
                else
                {
                    Mensage(2, "¡Error!", "¡La factura no fue gestionada correctamente...!", "error", "");
                }
            }
            else
            {
                Mensage(2,"¡Error!", "¡Aun se encuentran campos vacíos...!","error","");
            }
        }
        protected void btnCerrarModalBuscarCliente_Click(object sender, EventArgs e)
        {
            panelModalBuscarCliente.Visible=false;
        }
        protected void btnCerrarModalBuscarCliente2_Click(object sender, EventArgs e)
        {
            panelModalBuscarCliente.Visible = false;
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
            ReportarPago(id);
        }
        protected async void ReportarPago(int id)
        {
            Facturas facturas = control_Facturas.Consultar_id(id);
            if (facturas != null)
            {
                facturas.idEstado = 3;
                if (control_Facturas.Crud(facturas, 1).respuesta)
                {
                    /*En esta parte consultamos las facturas pendientes por pago*/
                    V_Facturas v_Facturas = new V_Facturas();
                    v_Facturas = control_Facturas.Consultar_id_vista(facturas.id);
                    if (v_Facturas != null)
                    {
                        string mensajeWhatsApp = "*v1* el pago del servicio de software POS del establecimiento *v2* por un valor *v3*... ¡Fue cargado exitosamente...!";
                        /* tenga en cuenta que :
                          v1=nombre del cleinte
                          v2=nombre comercial
                          v3=valor pensiente */
                        mensajeWhatsApp = mensajeWhatsApp.Replace("v1", $"{v_Facturas.nombreRepresentate}");
                        mensajeWhatsApp = mensajeWhatsApp.Replace("v2", $"{v_Facturas.nombreComercial}");
                        mensajeWhatsApp = mensajeWhatsApp.Replace("v3", $"{v_Facturas.valorAPagar:C0}");



                        WhatsAppResponse appResponse = new WhatsAppResponse();

                        appResponse = await confirmacin_de_pago.plantilla(
                            v_Facturas.celular,
                            v_Facturas.nombreComercial,
                            DateTime.Now.ToString(),
                            $"{v_Facturas.valorAPagar:N0}",
                            v_Facturas.nombreMes);

                        if (appResponse != null)
                        {
                            App_WhatsApp.Messages messages = new App_WhatsApp.Messages();
                            messages = appResponse.messages.FirstOrDefault();
                            if (messages != null)
                            {
                                if (messages.message_status == "accepted")
                                {
                                    Mensage(1, "¡Ok!", "¡Pago reportado...!", "success", "facturar.aspx");
                                }
                            }
                        }
                    }

                }
            }
        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Facturas facturas = control_Facturas.Consultar_id(id);
            if (facturas != null)
            {
                if (control_Facturas.Crud(facturas, 2).respuesta)
                {
                    Mensage(1,"¡Ok!","¡Factura Eliminada...!","success","facturar.aspx");
                }
            }
        }
        #endregion





        #region Funciones
        protected void Cargar_rpFacturas()
        {
            rpFacturas.DataSource = control_Facturas.Lista_Estado(1);
            rpFacturas.DataBind();
        }
        protected void CargarMeses()
        {
            ddl_Mes.DataValueField = "id";
            ddl_Mes.DataTextField = "nombreMes";
            ddl_Mes.DataSource = control_Meses.Lista();
            ddl_Mes.DataBind();
        }
        protected void CargarListaClientes()
        {
            rpClientes.DataSource = control_Clientes.ListaCompleta();
            rpClientes.DataBind();
        }
        protected void BuscarIdCliente(int IdCliente)
        {
            V_Clientes v_Clientes= new V_Clientes();
            v_Clientes = control_Clientes.ConsultarIdCliente_vista(IdCliente);
            if(v_Clientes != null)
            {
                txtCliente.Text = v_Clientes.nombreComercial;
                int valor= Convert.ToInt32(v_Clientes.valorPlan);
                txtValorPlan.Text = $"{valor}";
                txtSedes.Text = $"{v_Clientes.sedes}";
                int total = Convert.ToInt32(v_Clientes.sedes) * Convert.ToInt32(v_Clientes.valorPlan);
                txtValorAPagar.Text = $"{total}";
            }
        }
        protected bool VerificarCampos()
        {
            if(txtyear.Text!=""&&
               ddl_Mes.SelectedItem.Text!=""&&
               txtCliente.Text!=""&&
               txtValorPlan.Text!=""&&
               txtSedes.Text!=""&&
               txtValorAPagar.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected bool GestionarFactura(int idCliente,int idMes,int year)
        {
            Facturas facturas = new Facturas();
            facturas = control_Facturas.ConsultarFactura(idCliente,idMes,year);
            if(facturas == null) 
            {
                facturas= new Facturas();
                facturas.id = 0;
                facturas.fechaFactura=DateTime.Now;
                facturas.idCliente = idCliente;
                facturas.idMes= idMes;
                facturas.valorPlan = Convert.ToDecimal(txtValorPlan.Text);
                facturas.sedes = Convert.ToInt32(txtSedes.Text);
                facturas.valorAPagar = Convert.ToDecimal(txtValorAPagar.Text);
                facturas.idEstado = 1;
                facturas.contador = 1;
                facturas.yearFactura = Convert.ToInt32(txtyear.Text);
                return control_Facturas.Crud(facturas, 0).respuesta;
            }
            else { return false; }
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
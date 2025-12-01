using App_WhatsApp;
using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using RFacturacionElectronicaDIAN.Entities.Request;
using RFacturacionElectronicaDIAN.Entities.Response;
using RFacturacionElectronicaDIAN.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class cobrar : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }
        protected async void CargarListaCobroWhatsApp()
        {
            List<V_Facturas> whatsApp = new List<V_Facturas>();
            whatsApp = control_Facturas.Lista().Where(x=>x.idEstado==1).ToList();
            if(whatsApp.Count > 0)
            {
                int contadorEnviados = 0;
                foreach(V_Facturas wor in whatsApp)
                {
                    App_WhatsApp.WhatsAppResponse appResponse = new App_WhatsApp.WhatsAppResponse();
                    appResponse=await recordatorio_de_pago.plantilla(
                       wor.celular,
                        wor.nombreComercial,
                        wor.nombreMes,
                        "1",
                        Convert.ToInt32(wor.valorAPagar),
                        $"10-{DateTime.Now.Month}-{DateTime.Now.Year}",
                        "454-000044-36","901824648-7","SERINSIS SAS");
                    if(appResponse != null)
                    {
                        App_WhatsApp.Messages messages = new App_WhatsApp.Messages();
                        messages = appResponse.messages.FirstOrDefault();
                        if (messages != null) 
                        {
                            if (messages.message_status == "accepted")
                            {
                                contadorEnviados++;
                            }
                        }
                    }
                }
                Mensage("OK",$"se enviaron {contadorEnviados} corrextamente","success");
            }
        }
        protected void GuardarCobro(int idCliente,int sedes,int meses,decimal total)
        {
            
            CobrosEnviados cobrosEnviados = new CobrosEnviados();
            cobrosEnviados.id = 0;
            cobrosEnviados.fechaCobro = DateTime.Now;
            cobrosEnviados.idCliente= idCliente;
            cobrosEnviados.sedesCobradas= sedes;
            cobrosEnviados.mesesCobrados= meses;
            cobrosEnviados.valotTotalCobrado= total;
            controlador_CobrosEnviados.Crud(cobrosEnviados,0);
        }
        protected void Mensage( string titulo, string mensage, string tipo, string aspx = "")
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
            /* lo primero que hacemo es llamar la vista V_CobroWhatsApp */
            CargarListaCobroWhatsApp();
        }
        protected void CargarDG()
        {
            rpCobrosEnviados.DataSource = controlador_CobrosEnviados.LIstaCompleta();
            rpCobrosEnviados.DataBind();
        }
        protected void btnCargarDG_Click(object sender, EventArgs e)
        {
            CargarDG();
        }
    }
}
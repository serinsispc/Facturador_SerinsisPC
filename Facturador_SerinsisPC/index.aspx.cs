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
            ViewState["mensajesEnviados"] = 0;
            /*lo primero es traer el listado de los cleintes en mora*/
            List<EstadoCuentaClientes> estados = new List<EstadoCuentaClientes>();
            estados = controladorCuentas.GetClientes("pendiente");
            if (estados.Count > 0)
            {
                foreach(EstadoCuentaClientes listEstado in estados)
                {
                    /*lo segundo es consultar la o las bases da datos de daca cliente en mora*/
                    List<DatBases> bases=new List<DatBases>();
                    bases = controladorDatBases.ListdatBases(listEstado.id);
                    if(bases.Count > 0)
                    {
                        foreach(DatBases basesItado in bases)
                        {
                            string mensaje = $"{listEstado.nombreRepresentate}  el servicio de software POS del establecimiento {listEstado.nombreComercial} por un valor {listEstado.total}... ¡Ha sido suspendido por exceder el plazo máximo de pago...!* Consigna a las cuentas *NEQUI* *3144628361* o a la cuenta de ahorro *Bancolombia* *14708107591*... Recuerda enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *3125743162* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ".Replace("*","");
                            string mensaje2 = $"*{listEstado.nombreRepresentate}*  el servicio de software POS del establecimiento *{listEstado.nombreComercial}* por un valor *{listEstado.total}*... *¡Ha sido suspendido por exceder el plazo máximo de pago...!* Consigna a las cuentas *NEQUI* *3144628361* o a la cuenta de ahorro *Bancolombia* *14708107591*... Recuerda enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *3125743162* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ";
                            ModificarEstadoCuenta(basesItado.nameDataBase,"2",mensaje,listEstado.id);
                       
                            ViewState["mensajesEnviados"] = (int)ViewState["mensajesEnviados"] + 1;
                        }
                    }
                }
                if ((int)ViewState["mensajesEnviados"] >= estados.Count)
                {
                    Mensage(1,"¡OK!",$"¡Se suspendieron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!","success","index.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", $"¡Se suspendieron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "error", "index.aspx");
                }
            }
        }

        protected void ModificarEstadoCuenta(string dataBase,string estado,string mensaje,int IdCliente)
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
                        query = $"update AdminControl set tipo_admincontrol={estado},mensaje_admincontrol='{mensaje}'";
                        string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString,query);
                        /*actualizamos el estado en la tabla local*/
                        DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
                        query = $"update DatBases set estado={estado} where id={lista.id}";
                        string respuesta2 = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                    }
                }
                DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
            }
            catch(Exception ex)
            {
                string erro = ex.Message;
            }
        }

        protected void btnSuspender_Click(object sender, EventArgs e)
        {
            ViewState["mensajesEnviados"] = 0;
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            /*lo primero es traer el listado de los cleintes en mora*/
            List<EstadoCuentaClientes> estados = new List<EstadoCuentaClientes>();
            estados = controladorCuentas.Consultar_idCliente(idCliente);
            if (estados.Count > 0)
            {
                foreach (EstadoCuentaClientes listEstado in estados)
                {
                    /*lo segundo es consultar la o las bases da datos de daca cliente en mora*/
                    List<DatBases> bases = new List<DatBases>();
                    bases = controladorDatBases.ListdatBases(listEstado.id);
                    if (bases.Count > 0)
                    {
                        foreach (DatBases basesItado in bases)
                        {
                            string mensaje = $"{listEstado.nombreRepresentate}  el servicio de software POS del establecimiento {listEstado.nombreComercial} por un valor {listEstado.total}... ¡Ha sido suspendido por exceder el plazo máximo de pago...!* Consigna a la cuenta de ahorro *Bancolombia* *454-000044-36*... Recuerda enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *322 3760749* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ".Replace("*", "");
                            string mensaje2 = $"*{listEstado.nombreRepresentate}*  el servicio de software POS del establecimiento *{listEstado.nombreComercial}* por un valor *{listEstado.total}*... *¡Ha sido suspendido por exceder el plazo máximo de pago...!* Consigna a la cuenta de ahorro *Bancolombia* *454-000044-36*... Recuerda enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *322 3760749* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ";
                            ModificarEstadoCuenta(basesItado.nameDataBase, "2", mensaje, listEstado.id);
                     
                            ViewState["mensajesEnviados"] = (int)ViewState["mensajesEnviados"] + 1;
                        }
                    }
                }
                if ((int)ViewState["mensajesEnviados"] >= estados.Count)
                {
                    Mensage(1, "¡OK!", $"¡Se suspendieron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "success", "index.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", $"¡Se suspendieron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "error", "index.aspx");
                }
            }
        }

        protected void btnConectar_Click(object sender, EventArgs e)
        {
            ViewState["mensajesEnviados"] = 0;
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            /*lo primero es traer el listado de los cleintes en mora*/
            List<EstadoCuentaClientes> estados = new List<EstadoCuentaClientes>();
            estados = controladorCuentas.Consultar_idCliente(idCliente);
            if (estados.Count > 0)
            {
                foreach (EstadoCuentaClientes listEstado in estados)
                {
                    /*lo segundo es consultar la o las bases da datos de daca cliente en mora*/
                    List<DatBases> bases = new List<DatBases>();
                    bases = controladorDatBases.ListdatBases(listEstado.id);
                    if (bases.Count > 0)
                    {
                        foreach (DatBases basesItado in bases)
                        {
                            string mensaje = $"*{listEstado.nombreRepresentate}*  el servicio de software POS del establecimiento *{listEstado.nombreComercial}*... *¡Ha sido restablecido exitosamente...!".Replace("*","");
                            string mensaje2 = $"*{listEstado.nombreRepresentate}*  el servicio de software POS del establecimiento *{listEstado.nombreComercial}*... *¡Ha sido restablecido exitosamente...!";
                            ModificarEstadoCuenta(basesItado.nameDataBase, "0", mensaje, listEstado.id);
                        
                            ViewState["mensajesEnviados"] = (int)ViewState["mensajesEnviados"] + 1;
                        }
                    }
                    if ((int)ViewState["mensajesEnviados"] >= estados.Count)
                    {
                        Mensage(1, "¡OK!", $"¡Se activaron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "success", "index.aspx");
                    }
                    else
                    {
                        Mensage(1, "¡Error!", $"¡Se activaron {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "error", "index.aspx");
                    }
                }
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

        protected void btnAbiso_Click(object sender, EventArgs e)
        {
            ViewState["mensajesEnviados"] = 0;
            int idCliente = Convert.ToInt32((sender as LinkButton).CommandArgument);
            /*lo primero es traer el listado de los cleintes en mora*/
            List<EstadoCuentaClientes> estados = new List<EstadoCuentaClientes>();
            estados = controladorCuentas.Consultar_idCliente(idCliente);
            if (estados.Count > 0)
            {
                foreach (EstadoCuentaClientes listEstado in estados)
                {
                    /*lo segundo es consultar la o las bases da datos de daca cliente en mora*/
                    List<DatBases> bases = new List<DatBases>();
                    bases = controladorDatBases.ListdatBases(listEstado.id);
                    if (bases.Count > 0)
                    {
                        foreach (DatBases basesItado in bases)
                        {
                            string mensaje = $"{listEstado.nombreRepresentate}  el servicio de software POS del establecimiento {listEstado.nombreComercial} por un valor {listEstado.total}... ¡Esta pendiente de pago...!* Consigna a la cuenta de ahorro *Bancolombia* *454-000044-36*... Recuerda la fecha limite de pago es el día 5 de cada mes... enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *322 3760749* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ".Replace("*", "");
                            string mensaje2 = $"*{listEstado.nombreRepresentate}*  el servicio de software POS del establecimiento *{listEstado.nombreComercial}* por un valor *{listEstado.total}*... *¡Esta pendiente de pago...!* Consigna a la cuenta de ahorro *Bancolombia* *454-000044-36*... Recuerda  la fecha limite de pago es el día 5 de cada mes... enviar el comprobante de consignación al *WhatsApp* *3144628361* o al correo *facturacion@serinsispc.com*... *NOTA*: esta cuenta de *WhatsApp* *322 3760749* solo es informativa. Si envías un mensaje por medio de esta cuenta no será recibido... Recuerda nuestros canales de atención, *WhatsApp* *3144628361* o a los correos, *soporte@serinsispc.com*, *facturacion@serinsispc.com*  ";
                            ModificarEstadoCuenta(basesItado.nameDataBase, "1", mensaje, listEstado.id);

                            ViewState["mensajesEnviados"] = (int)ViewState["mensajesEnviados"] + 1;
                        }
                    }
                }
                if ((int)ViewState["mensajesEnviados"] >= estados.Count)
                {
                    Mensage(1, "¡OK!", $"¡Se activo el abiso {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "success", "index.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", $"¡Se activo el abiso {(int)ViewState["mensajesEnviados"]} cuentas exitosamente!", "error", "index.aspx");
                }
            }
        }
    }
}
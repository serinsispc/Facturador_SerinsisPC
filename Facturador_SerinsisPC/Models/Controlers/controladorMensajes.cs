using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class controladorMensajes
    {
        public static RespuestaSQL Crud(Mensajes mensajes, int Boton)
        {
            try
            {
                string query = string.Empty;
                if (Boton == 0)
                {
                    query = $"exec InsertInto_Mensajes " +
                        $"{mensajes.nombreMensaje}," +
                        $"'{mensajes.mensajeText}'," +
                        $"{mensajes.variables}";
                }
                if (Boton == 1)
                {
                    query = $"exec InsertInto_Mensajes " +
                        $"{mensajes.id}" +
                        $"{mensajes.nombreMensaje}," +
                        $"'{mensajes.mensajeText}'," +
                        $"{mensajes.variables}";
                }
                if (Boton == 2)
                {
                    query = $"exec Delete_Mensajes {mensajes.id}";
                }
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL respuestaSQL = new RespuestaSQL();
                respuestaSQL = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return respuestaSQL;
            }
            catch(Exception ex)
            {
                string erro = ex.Message;
                return null;
            }
        }
        public static List<Mensajes> ConsultarLista()
        {
            try
            {
                string query = $"select *from Mensajes";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Mensajes>>(respuesta);
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                return null;
            }
        }
        public static Mensajes Consultar_id(int Id)
        {
            try
            {
                string query = $"select *from Mensajes where id={Id}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Mensajes>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                return null;
            }
        }
    }
}
using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class controlador_V_CobroWhatsApp
    {
        public static List<V_CobroWhatsApp> listaComplata()
        {
            try
            {
                string query = $"select *from V_CobroWhatsApp";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_CobroWhatsApp>>(respuesta);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static int totalPendiente()
        {
            try
            {
                string query = $"select  SUM(valorAPagar) as sumaTotal from V_Facturas where idEstado<>3";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                respuesta= respuesta.Replace("[","");
                respuesta = respuesta.Replace("]", "");
                var lista = JsonConvert.DeserializeObject<Sumas>(respuesta);
                

                return Convert.ToInt32(lista.sumaTotal);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return 0;
            }
        }
    }
}
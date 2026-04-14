using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_ConfigWhatsAppMeta
    {
        public static ConfigWhatsAppMeta Consultar(string nombreConfiguracion = null)
        {
            try
            {
                string query;

                if (string.IsNullOrWhiteSpace(nombreConfiguracion))
                {
                    query = "select top 1 * from Config_WhatsApp_Meta order by id desc";
                }
                else
                {
                    string nombre = nombreConfiguracion.Replace("'", "''");
                    query = $"select top 1 * from Config_WhatsApp_Meta where nombreConfiguracion='{nombre}' order by id desc";
                }

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<ConfigWhatsAppMeta>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}

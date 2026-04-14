using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_MetodoPago
    {
        public static List<MetodoPago> ListaCompleta()
        {
            try
            {
                string query = "select *from MetodoPago order by nombreMetodo";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<MetodoPago>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return new List<MetodoPago>();
            }
        }
    }
}

using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_V_IngresosMensuales
    {
        public static List<V_IngresosMensuales> ListaCompleta()
        {
            try
            {
                string query = "select *from V_IngresosMensuales order by anio desc, mes desc";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_IngresosMensuales>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return new List<V_IngresosMensuales>();
            }
        }
    }
}

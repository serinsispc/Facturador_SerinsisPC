using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_V_ControlPagosClientes
    {
        public static List<V_ControlPagosClientes> ListaCompleta()
        {
            try
            {
                string query = "select *from V_ControlPagosClientes order by fechaProximoPago asc, nombreComercial asc";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_ControlPagosClientes>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return new List<V_ControlPagosClientes>();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_TipoPlan
    {
        public static List<TipoPlan> ListaCompleta() 
        {
            try
            {
                string query = $"select *from TipoPlan";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<TipoPlan>>(respuesta);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
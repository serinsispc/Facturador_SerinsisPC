using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_Meses
    {
        public static List<Meses> Lista()
        {
            try
            {
                string query = $"select *from Meses";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Meses>>(respuesta);
            }
            catch(Exception ex)
            {
                string error= ex.Message;
                return null;
            }
        }
    }
}
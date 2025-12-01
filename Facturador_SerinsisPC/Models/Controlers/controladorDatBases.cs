using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class controladorDatBases
    {
        public static List<DatBases> ListdatBases(int IdCliente)
        {
            try
            {
                string query = $"select *from DatBases where idCliente={IdCliente}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<DatBases>>(respuesta);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return null;
            }
        }  
    }
}
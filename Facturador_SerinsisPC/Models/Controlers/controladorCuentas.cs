using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class controladorCuentas
    {
        public static List<EstadoCuentaClientes> GetClientes(string estado)
        {
            try
            {
                string query =string.Empty;
                if (estado == "pendiente")
                {
                    query = $"select *from EstadoCuentaClientes where total>0";

                }
                else
                {
                    query = $"select *from EstadoCuentaClientes where total=0";
                }
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<EstadoCuentaClientes>>(respuesta);
            }
            catch(Exception ex)
            {
                string error=ex.Message;
                return null;
            }
        }
        public static List<EstadoCuentaClientes> Consultar_idCliente(int idCliente)
        {
            try
            {
                string query = $"select *from EstadoCuentaClientes where id={idCliente}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<EstadoCuentaClientes>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_Clientes
    {
        public static RespuestaSQL Crud(Clientes clientes,int Boton)
        {
            try
            {
                string query=string.Empty;
                if (Boton == 0)
                {
                    query = $"exec InsertInto_Clientes " +
                        $"{clientes.idTipoPlan}," +
                        $"'{clientes.nombreComercial}'," +
                        $"'{clientes.nombreRepresentate}'," +
                        $"'{clientes.celular}'," +
                        $"'{clientes.correo}'," +
                        $"{clientes.sedes}," +
                        $"{clientes.valorPlan}," +
                        $"'{clientes.nit}'," +
                        $"{clientes.estado}";
                }
                if (Boton == 1) 
                {
                    query = $"exec Update_Clientes " +
                        $"{clientes.id},"+
                        $"{clientes.idTipoPlan}," +
                        $"'{clientes.nombreComercial}'," +
                        $"'{clientes.nombreRepresentate}'," +
                        $"'{clientes.celular}'," +
                        $"'{clientes.correo}'," +
                        $"{clientes.sedes}," +
                        $"{clientes.valorPlan}," +
                        $"'{clientes.nit}'," +
                        $"{clientes.estado}";
                }
                if(Boton == 2)
                {
                    query = $"exec Delete_Clientes {clientes.id}";
                }
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString,query);
                RespuestaSQL respuestaSQL = new RespuestaSQL();
                respuestaSQL = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return respuestaSQL;
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static List<V_Clientes> ListaCompleta()
        {
            try
            {
                string query = $"select *from V_Clientes";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Clientes>>(respuesta);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static V_Clientes ConsultarIdCliente_vista(int idCliente)
        {
            try
            {
                string query = $"select *from V_Clientes where id={idCliente}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Clientes>>(respuesta).FirstOrDefault();
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static Clientes ConsultarIdCliente(int idCliente)
        {
            try
            {
                string query = $"select *from Clientes where id={idCliente}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Clientes>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
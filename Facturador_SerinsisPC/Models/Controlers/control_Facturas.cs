using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_Facturas
    {
        public static RespuestaSQL Crud(Facturas facturas, int Boton)
        {
            try
            {
                string query = string.Empty;
                if (Boton == 0)
                {
                    query = $"exec InsertInto_Facturas " +
                        $"'{ClassDateTime.FormatoDate(facturas.fechaFactura)}'," +
                        $"{facturas.idCliente}," +
                        $"{facturas.idMes}," +
                        $"{facturas.valorPlan}," +
                        $"{facturas.sedes}," +
                        $"{facturas.valorAPagar}," +
                        $"{facturas.idEstado}," +
                        $"{facturas.contador}," +
                        $"{facturas.yearFactura}";
                }
                if (Boton == 1)
                {
                    query = $"exec Update_Facturas " +
                        $"{facturas.id}," +
                        $"'{ClassDateTime.FormatoDate(facturas.fechaFactura)}'," +
                        $"{facturas.idCliente}," +
                        $"{facturas.idMes}," +
                        $"{Convert.ToInt32(facturas.valorPlan)}," +
                        $"{facturas.sedes}," +
                        $"{Convert.ToInt32(facturas.valorAPagar)}," +
                        $"{facturas.idEstado}," +
                        $"{facturas.contador}," +
                        $"{facturas.yearFactura}";
                }
                if (Boton == 2)
                {
                    query = $"exec Detale_Facturas {facturas.id}";
                }
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
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
        public static Facturas ConsultarFactura(int idCliente,int idMes,int year)
        {
            try
            {
                string query = $"select *from Facturas where idCliente={idCliente} and idMes={idMes} and yearFactura={year}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Facturas>>(respuesta).FirstOrDefault();
            }
            catch(Exception ex)
            {
                string error=ex.Message;
                return null;
            }
        }
        public static List<V_Facturas>Lista_Estado(int IdEstado)
        {
            try
            {
                string query = $"select *from V_Facturas where idEstado={IdEstado}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Facturas>>(respuesta);
            }
            catch(Exception ex)
            {
                string error=ex.Message;
                return null;
            }
        }
        public static Facturas Consultar_id(int idFactura)
        {
            try
            {
                string query = $"select *from Facturas where id={idFactura}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Facturas>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static V_Facturas Consultar_id_vista(int idFactura)
        {
            try
            {
                string query = $"select *from V_Facturas where id={idFactura}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Facturas>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        public static List<V_Facturas>Lista()
        {
            try
            {
                string query = $"select *from V_Facturas";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Facturas>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
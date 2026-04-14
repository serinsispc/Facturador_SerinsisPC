using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_Facturas
    {
        private static string SqlDateOrNull(DateTime? date)
        {
            return date.HasValue ? $"'{ClassDateTime.FormatoDate(date.Value)}'" : "null";
        }

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
                        $"{facturas.valorPlan.ToString(CultureInfo.InvariantCulture)}," +
                        $"{facturas.sedes}," +
                        $"{facturas.valorAPagar.ToString(CultureInfo.InvariantCulture)}," +
                        $"{facturas.idEstado}," +
                        $"{facturas.contador}," +
                        $"{facturas.yearFactura}," +
                        $"{SqlDateOrNull(facturas.fechaVencimiento)}," +
                        $"{SqlDateOrNull(facturas.periodoDesde)}," +
                        $"{SqlDateOrNull(facturas.periodoHasta)}," +
                        $"{facturas.saldoPendiente.ToString(CultureInfo.InvariantCulture)}," +
                        $"{SqlDateOrNull(facturas.fechaPagoCompleto)}";
                }
                if (Boton == 1)
                {
                    query = $"exec Update_Facturas " +
                        $"{facturas.id}," +
                        $"'{ClassDateTime.FormatoDate(facturas.fechaFactura)}'," +
                        $"{facturas.idCliente}," +
                        $"{facturas.idMes}," +
                        $"{facturas.valorPlan.ToString(CultureInfo.InvariantCulture)}," +
                        $"{facturas.sedes}," +
                        $"{facturas.valorAPagar.ToString(CultureInfo.InvariantCulture)}," +
                        $"{facturas.idEstado}," +
                        $"{facturas.contador}," +
                        $"{facturas.yearFactura}," +
                        $"{SqlDateOrNull(facturas.fechaVencimiento)}," +
                        $"{SqlDateOrNull(facturas.periodoDesde)}," +
                        $"{SqlDateOrNull(facturas.periodoHasta)}," +
                        $"{facturas.saldoPendiente.ToString(CultureInfo.InvariantCulture)}," +
                        $"{SqlDateOrNull(facturas.fechaPagoCompleto)}";
                }
                if (Boton == 2)
                {
                    query = $"exec Detale_Facturas {facturas.id}";
                }

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static Facturas ConsultarFactura(int idCliente, int idMes, int year)
        {
            try
            {
                string query = $"select *from Facturas where idCliente={idCliente} and idMes={idMes} and yearFactura={year}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<Facturas>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static List<V_Facturas> Lista_Estado(int IdEstado)
        {
            try
            {
                string query = $"select *from V_Facturas where idEstado={IdEstado}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Facturas>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
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

        public static List<V_Facturas> Lista()
        {
            try
            {
                string query = "select *from V_Facturas";
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

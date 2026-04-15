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
                    query =
                        "BEGIN TRY " +
                        "SET NOCOUNT ON; " +
                        "INSERT INTO dbo.Facturas " +
                        "(fechaFactura, idCliente, idMes, valorPlan, sedes, valorAPagar, idEstado, contador, yearFactura, fechaVencimiento, periodoDesde, periodoHasta, saldoPendiente, fechaPagoCompleto) " +
                        "VALUES " +
                        "(" +
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
                        $"{SqlDateOrNull(facturas.fechaPagoCompleto)}" +
                        "); " +
                        "SELECT CAST(1 AS bit) AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId, CAST('OK' AS VARCHAR(250)) AS mensaje; " +
                        "END TRY " +
                        "BEGIN CATCH " +
                        "SELECT CAST(0 AS bit) AS respuesta, 0 AS nuevoId, ERROR_MESSAGE() AS mensaje; " +
                        "END CATCH";
                }
                if (Boton == 1)
                {
                    query =
                        "BEGIN TRY " +
                        "SET NOCOUNT ON; " +
                        "UPDATE dbo.Facturas SET " +
                        $"fechaFactura = '{ClassDateTime.FormatoDate(facturas.fechaFactura)}', " +
                        $"idCliente = {facturas.idCliente}, " +
                        $"idMes = {facturas.idMes}, " +
                        $"valorPlan = {facturas.valorPlan.ToString(CultureInfo.InvariantCulture)}, " +
                        $"sedes = {facturas.sedes}, " +
                        $"valorAPagar = {facturas.valorAPagar.ToString(CultureInfo.InvariantCulture)}, " +
                        $"idEstado = {facturas.idEstado}, " +
                        $"contador = {facturas.contador}, " +
                        $"yearFactura = {facturas.yearFactura}, " +
                        $"fechaVencimiento = {SqlDateOrNull(facturas.fechaVencimiento)}, " +
                        $"periodoDesde = {SqlDateOrNull(facturas.periodoDesde)}, " +
                        $"periodoHasta = {SqlDateOrNull(facturas.periodoHasta)}, " +
                        $"saldoPendiente = {facturas.saldoPendiente.ToString(CultureInfo.InvariantCulture)}, " +
                        $"fechaPagoCompleto = {SqlDateOrNull(facturas.fechaPagoCompleto)} " +
                        $"WHERE id = {facturas.id}; " +
                        "SELECT CAST(CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS bit) AS respuesta, " +
                        $"{facturas.id} AS nuevoId, " +
                        "CAST(CASE WHEN @@ROWCOUNT > 0 THEN 'OK' ELSE 'No se encontro la factura para actualizar.' END AS VARCHAR(250)) AS mensaje; " +
                        "END TRY " +
                        "BEGIN CATCH " +
                        "SELECT CAST(0 AS bit) AS respuesta, 0 AS nuevoId, ERROR_MESSAGE() AS mensaje; " +
                        "END CATCH";
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

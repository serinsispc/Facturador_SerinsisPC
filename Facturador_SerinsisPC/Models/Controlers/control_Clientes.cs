using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_Clientes
    {
        private static string EscapeSql(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("'", "''");
        }

        private static string SqlDateOrNull(DateTime? date)
        {
            return date.HasValue ? $"'{ClassDateTime.FormatoDate(date.Value)}'" : "null";
        }

        public static RespuestaSQL Crud(Clientes clientes, int Boton)
        {
            try
            {
                string query = string.Empty;
                if (Boton == 0)
                {
                    query =
                        "BEGIN TRY " +
                        "SET NOCOUNT ON; " +
                        "INSERT INTO dbo.Clientes " +
                        "(idTipoPlan, nombreComercial, nombreRepresentate, celular, correo, sedes, valorPlan, nit, estado, diaPago, fechaInicioPlan, fechaUltimoPago, fechaProximoPago, observacionCartera) " +
                        "VALUES " +
                        "(" +
                        $"{clientes.idTipoPlan}," +
                        $"'{EscapeSql(clientes.nombreComercial)}'," +
                        $"'{EscapeSql(clientes.nombreRepresentate)}'," +
                        $"'{EscapeSql(clientes.celular)}'," +
                        $"'{EscapeSql(clientes.correo)}'," +
                        $"{clientes.sedes}," +
                        $"{clientes.valorPlan.ToString(CultureInfo.InvariantCulture)}," +
                        $"'{EscapeSql(clientes.nit)}'," +
                        $"{clientes.estado}," +
                        $"{(clientes.diaPago.HasValue ? clientes.diaPago.Value.ToString() : "null")}," +
                        $"{SqlDateOrNull(clientes.fechaInicioPlan)}," +
                        $"{SqlDateOrNull(clientes.fechaUltimoPago)}," +
                        $"{SqlDateOrNull(clientes.fechaProximoPago)}," +
                        $"'{EscapeSql(clientes.observacionCartera)}'" +
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
                        "UPDATE dbo.Clientes SET " +
                        $"idTipoPlan = {clientes.idTipoPlan}, " +
                        $"nombreComercial = '{EscapeSql(clientes.nombreComercial)}', " +
                        $"nombreRepresentate = '{EscapeSql(clientes.nombreRepresentate)}', " +
                        $"celular = '{EscapeSql(clientes.celular)}', " +
                        $"correo = '{EscapeSql(clientes.correo)}', " +
                        $"sedes = {clientes.sedes}, " +
                        $"valorPlan = {clientes.valorPlan.ToString(CultureInfo.InvariantCulture)}, " +
                        $"nit = '{EscapeSql(clientes.nit)}', " +
                        $"estado = {clientes.estado}, " +
                        $"diaPago = {(clientes.diaPago.HasValue ? clientes.diaPago.Value.ToString() : "null")}, " +
                        $"fechaInicioPlan = {SqlDateOrNull(clientes.fechaInicioPlan)}, " +
                        $"fechaUltimoPago = {SqlDateOrNull(clientes.fechaUltimoPago)}, " +
                        $"fechaProximoPago = {SqlDateOrNull(clientes.fechaProximoPago)}, " +
                        $"observacionCartera = '{EscapeSql(clientes.observacionCartera)}' " +
                        $"WHERE id = {clientes.id}; " +
                        "SELECT CAST(CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS bit) AS respuesta, " +
                        $"{clientes.id} AS nuevoId, " +
                        "CAST(CASE WHEN @@ROWCOUNT > 0 THEN 'OK' ELSE 'No se encontro el cliente para actualizar.' END AS VARCHAR(250)) AS mensaje; " +
                        "END TRY " +
                        "BEGIN CATCH " +
                        "SELECT CAST(0 AS bit) AS respuesta, 0 AS nuevoId, ERROR_MESSAGE() AS mensaje; " +
                        "END CATCH";
                }
                if (Boton == 2)
                {
                    query =
                        "BEGIN TRY " +
                        "SET NOCOUNT ON; " +
                        $"DELETE FROM dbo.Clientes WHERE id = {clientes.id}; " +
                        "SELECT CAST(CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS bit) AS respuesta, 0 AS nuevoId, " +
                        "CAST(CASE WHEN @@ROWCOUNT > 0 THEN 'OK' ELSE 'No se encontro el cliente para eliminar.' END AS VARCHAR(250)) AS mensaje; " +
                        "END TRY " +
                        "BEGIN CATCH " +
                        "SELECT CAST(0 AS bit) AS respuesta, 0 AS nuevoId, ERROR_MESSAGE() AS mensaje; " +
                        "END CATCH";
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

        public static List<V_Clientes> ListaCompleta()
        {
            try
            {
                string query = "select *from V_Clientes";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_Clientes>>(respuesta);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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

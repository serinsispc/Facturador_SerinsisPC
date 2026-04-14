using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_PagosRecibidos
    {
        private static string EscapeSql(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("'", "''");
        }

        public static RespuestaSQL RegistrarPagoFactura(PagosRecibidos pago, int idFactura, decimal valorAplicado)
        {
            try
            {
                string query = "exec InsertInto_PagoRecibidoFactura " +
                    $"'{ClassDateTime.FormatoDate(pago.fechaPago)}'," +
                    $"{pago.idCliente}," +
                    $"{pago.idMetodoPago}," +
                    $"{pago.valorRecibido.ToString(CultureInfo.InvariantCulture)}," +
                    $"'{EscapeSql(pago.numeroComprobante)}'," +
                    $"'{EscapeSql(pago.referenciaPago)}'," +
                    $"'{EscapeSql(pago.observacion)}'," +
                    $"'{EscapeSql(pago.usuarioRegistro)}'," +
                    $"{idFactura}," +
                    $"{valorAplicado.ToString(CultureInfo.InvariantCulture)}";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static List<V_PagosRecibidos> ListaCompleta()
        {
            try
            {
                string query = "select *from V_PagosRecibidos order by fechaPago desc";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_PagosRecibidos>>(respuesta);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return new List<V_PagosRecibidos>();
            }
        }
    }
}

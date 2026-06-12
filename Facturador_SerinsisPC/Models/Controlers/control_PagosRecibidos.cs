using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

        public static RespuestaSQL EliminarPagoRecibido(int idPago)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine("SET NOCOUNT ON;");
                query.AppendLine("BEGIN TRY");
                query.AppendLine("    BEGIN TRANSACTION;");
                query.AppendLine($"    DECLARE @idPago INT = {idPago};");
                query.AppendLine("    DECLARE @idCliente INT;");
                query.AppendLine("    DECLARE @fechaUltimoPago DATE;");
                query.AppendLine("    DECLARE @fechaInicioPlan DATE;");
                query.AppendLine("    DECLARE @diaPago TINYINT;");
                query.AppendLine("    DECLARE @periodicidad INT;");
                query.AppendLine();
                query.AppendLine("    SELECT @idCliente = pr.idCliente");
                query.AppendLine("    FROM dbo.PagosRecibidos pr");
                query.AppendLine("    WHERE pr.id = @idPago;");
                query.AppendLine();
                query.AppendLine("    IF @idCliente IS NULL");
                query.AppendLine("    BEGIN");
                query.AppendLine("        RAISERROR('No se encontro el pago seleccionado.', 16, 1);");
                query.AppendLine("    END;");
                query.AppendLine();
                query.AppendLine("    UPDATE f");
                query.AppendLine("    SET");
                query.AppendLine("        f.saldoPendiente = CASE");
                query.AppendLine("            WHEN ISNULL(f.saldoPendiente, 0) + pdf.valorAplicado > f.valorAPagar THEN f.valorAPagar");
                query.AppendLine("            ELSE ISNULL(f.saldoPendiente, 0) + pdf.valorAplicado");
                query.AppendLine("        END,");
                query.AppendLine("        f.fechaPagoCompleto = NULL,");
                query.AppendLine("        f.idEstado = CASE");
                query.AppendLine("            WHEN ISNULL(f.saldoPendiente, 0) + pdf.valorAplicado > 0 THEN 1");
                query.AppendLine("            ELSE f.idEstado");
                query.AppendLine("        END");
                query.AppendLine("    FROM dbo.Facturas f");
                query.AppendLine("    INNER JOIN dbo.PagoDetalleFactura pdf ON pdf.idFactura = f.id");
                query.AppendLine("    WHERE pdf.idPagoRecibido = @idPago;");
                query.AppendLine();
                query.AppendLine("    DELETE FROM dbo.PagoDetalleFactura WHERE idPagoRecibido = @idPago;");
                query.AppendLine("    DELETE FROM dbo.PagosRecibidos WHERE id = @idPago;");
                query.AppendLine();
                query.AppendLine("    SELECT");
                query.AppendLine("        @fechaUltimoPago = MAX(CAST(pr.fechaPago AS DATE))");
                query.AppendLine("    FROM dbo.PagosRecibidos pr");
                query.AppendLine("    WHERE pr.idCliente = @idCliente;");
                query.AppendLine();
                query.AppendLine("    SELECT");
                query.AppendLine("        @fechaInicioPlan = c.fechaInicioPlan,");
                query.AppendLine("        @diaPago = c.diaPago,");
                query.AppendLine("        @periodicidad = CASE WHEN ISNULL(tp.periodicidadMeses, 0) <= 0 THEN 1 ELSE tp.periodicidadMeses END");
                query.AppendLine("    FROM dbo.Clientes c");
                query.AppendLine("    INNER JOIN dbo.TipoPlan tp ON tp.id = c.idTipoPlan");
                query.AppendLine("    WHERE c.id = @idCliente;");
                query.AppendLine();
                query.AppendLine("    UPDATE dbo.Clientes");
                query.AppendLine("    SET");
                query.AppendLine("        fechaUltimoPago = ISNULL(@fechaUltimoPago, @fechaInicioPlan),");
                query.AppendLine("        fechaProximoPago = CASE");
                query.AppendLine("            WHEN @fechaUltimoPago IS NOT NULL THEN");
                query.AppendLine("                DATEFROMPARTS(");
                query.AppendLine("                    YEAR(DATEADD(MONTH, @periodicidad, @fechaUltimoPago)),");
                query.AppendLine("                    MONTH(DATEADD(MONTH, @periodicidad, @fechaUltimoPago)),");
                query.AppendLine("                    CASE");
                query.AppendLine("                        WHEN @diaPago > DAY(EOMONTH(DATEADD(MONTH, @periodicidad, @fechaUltimoPago))) THEN DAY(EOMONTH(DATEADD(MONTH, @periodicidad, @fechaUltimoPago)))");
                query.AppendLine("                        ELSE @diaPago");
                query.AppendLine("                    END");
                query.AppendLine("                )");
                query.AppendLine("            ELSE");
                query.AppendLine("                CASE");
                query.AppendLine("                    WHEN DATEFROMPARTS(");
                query.AppendLine("                        YEAR(@fechaInicioPlan),");
                query.AppendLine("                        MONTH(@fechaInicioPlan),");
                query.AppendLine("                        CASE");
                query.AppendLine("                            WHEN @diaPago > DAY(EOMONTH(@fechaInicioPlan)) THEN DAY(EOMONTH(@fechaInicioPlan))");
                query.AppendLine("                            ELSE @diaPago");
                query.AppendLine("                        END");
                query.AppendLine("                    ) < @fechaInicioPlan THEN");
                query.AppendLine("                        DATEFROMPARTS(");
                query.AppendLine("                            YEAR(DATEADD(MONTH, 1, @fechaInicioPlan)),");
                query.AppendLine("                            MONTH(DATEADD(MONTH, 1, @fechaInicioPlan)),");
                query.AppendLine("                            CASE");
                query.AppendLine("                                WHEN @diaPago > DAY(EOMONTH(DATEADD(MONTH, 1, @fechaInicioPlan))) THEN DAY(EOMONTH(DATEADD(MONTH, 1, @fechaInicioPlan)))");
                query.AppendLine("                                ELSE @diaPago");
                query.AppendLine("                            END");
                query.AppendLine("                        )");
                query.AppendLine("                    ELSE");
                query.AppendLine("                        DATEFROMPARTS(");
                query.AppendLine("                            YEAR(@fechaInicioPlan),");
                query.AppendLine("                            MONTH(@fechaInicioPlan),");
                query.AppendLine("                            CASE");
                query.AppendLine("                                WHEN @diaPago > DAY(EOMONTH(@fechaInicioPlan)) THEN DAY(EOMONTH(@fechaInicioPlan))");
                query.AppendLine("                                ELSE @diaPago");
                query.AppendLine("                            END");
                query.AppendLine("                        )");
                query.AppendLine("                END");
                query.AppendLine("        END");
                query.AppendLine("    WHERE id = @idCliente;");
                query.AppendLine();
                query.AppendLine("    COMMIT TRANSACTION;");
                query.AppendLine("    SELECT CAST(1 AS bit) AS respuesta, @idPago AS nuevoId, CAST('Pago eliminado correctamente.' AS VARCHAR(250)) AS mensaje;");
                query.AppendLine("END TRY");
                query.AppendLine("BEGIN CATCH");
                query.AppendLine("    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;");
                query.AppendLine("    SELECT CAST(0 AS bit) AS respuesta, 0 AS nuevoId, ERROR_MESSAGE() AS mensaje;");
                query.AppendLine("END CATCH;");

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query.ToString());
                return JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new RespuestaSQL
                {
                    respuesta = false,
                    nuevoId = 0,
                    mensaje = ex.Message
                };
            }
        }
    }
}

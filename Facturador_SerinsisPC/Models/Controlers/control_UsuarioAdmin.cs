using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class control_UsuarioAdmin
    {
        private static string EscapeSql(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("'", "''");
        }

        public static UsuarioAdmin Validar(string loginUsuario, string passwordPlano)
        {
            try
            {
                string query = "exec Validar_UsuarioAdmin " +
                    $"'{EscapeSql(loginUsuario)}'," +
                    $"'{EscapeSql(passwordPlano)}'";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<UsuarioAdmin>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static bool ActualizarUltimoAcceso(int id)
        {
            try
            {
                string query = $"exec UpdateUltimoAcceso_UsuarioAdmin {id}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL resultado = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return resultado != null && resultado.respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        public static UsuarioAdmin ConsultarRecuperacion(string loginUsuario, string whatsApp)
        {
            try
            {
                string query = "exec Consultar_UsuarioAdmin_Recuperacion " +
                    $"'{EscapeSql(loginUsuario)}'," +
                    $"'{EscapeSql(whatsApp)}'";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<UsuarioAdmin>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static bool ActualizarPasswordRecuperacion(string loginUsuario, string whatsApp, string passwordNueva)
        {
            try
            {
                string query = "exec UpdatePassword_UsuarioAdmin_Recuperacion " +
                    $"'{EscapeSql(loginUsuario)}'," +
                    $"'{EscapeSql(whatsApp)}'," +
                    $"'{EscapeSql(passwordNueva)}'";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL resultado = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return resultado != null && resultado.respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        public static bool GenerarCodigoRecuperacion(int idUsuarioAdmin, string codigoPlano, int minutosVigencia)
        {
            try
            {
                string query = "exec GenerarTokenRecuperacion_UsuarioAdmin " +
                    $"{idUsuarioAdmin}," +
                    $"'{EscapeSql(codigoPlano)}'," +
                    $"{minutosVigencia}";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL resultado = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return resultado != null && resultado.respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        public static bool ConfirmarRecuperacionPorCodigo(string loginUsuario, string whatsApp, string codigoPlano, string passwordNueva)
        {
            try
            {
                string query = "exec ConfirmarRecuperacion_UsuarioAdmin " +
                    $"'{EscapeSql(loginUsuario)}'," +
                    $"'{EscapeSql(whatsApp)}'," +
                    $"'{EscapeSql(codigoPlano)}'," +
                    $"'{EscapeSql(passwordNueva)}'";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL resultado = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return resultado != null && resultado.respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        public static UsuarioAdmin ConsultarPorId(int id)
        {
            try
            {
                string query = $"select *from V_UsuarioAdmin where id={id}";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<UsuarioAdmin>>(respuesta).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public static bool ActualizarPasswordInterna(int id, string passwordNueva)
        {
            try
            {
                string query = "exec UpdatePassword_UsuarioAdmin_Interna " +
                    $"{id}," +
                    $"'{EscapeSql(passwordNueva)}'";

                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL resultado = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return resultado != null && resultado.respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        public static void RegistrarBitacora(int? idUsuarioAdmin, string loginUsuario, string accion, string detalle)
        {
            try
            {
                string query = "exec InsertInto_BitacoraSeguridadAdmin " +
                    $"{(idUsuarioAdmin.HasValue ? idUsuarioAdmin.Value.ToString() : "null")}," +
                    $"'{EscapeSql(loginUsuario)}'," +
                    $"'{EscapeSql(accion)}'," +
                    $"'{EscapeSql(detalle)}'";

                DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }
}

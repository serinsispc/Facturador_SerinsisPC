using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.Controlers
{
    public class controlador_CobrosEnviados
    {
        public static RespuestaSQL Crud(CobrosEnviados cobrosEnviados,int Boton)
        {
            try
            {
                string query = string.Empty;
                if (Boton == 0)
                {
                    query = $"exec InsertInto_CobrosEnviados " +
                        $"'{ClassDateTime.FormatoDate(cobrosEnviados.fechaCobro)}'," +
                        $"{cobrosEnviados.idCliente}," +
                        $"{cobrosEnviados.sedesCobradas}," +
                        $"{cobrosEnviados.mesesCobrados}," +
                        $"{cobrosEnviados.valotTotalCobrado},";
                }
                if (Boton == 1)
                {
                    query = $"exec Update_CobrosEnviados " +
                        $"{cobrosEnviados.id}," +
                        $"{cobrosEnviados.idCliente}," +
                        $"{cobrosEnviados.sedesCobradas}," +
                        $"{cobrosEnviados.mesesCobrados}," +
                        $"{cobrosEnviados.valotTotalCobrado},";
                }
                if (Boton == 2)
                {
                    query = $"exec Delete_CobrosEnviados {cobrosEnviados.id}";
                }
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                RespuestaSQL respuestaSQL = new RespuestaSQL();
                respuestaSQL = JsonConvert.DeserializeObject<List<RespuestaSQL>>(respuesta).FirstOrDefault();
                return respuestaSQL;
            }
            catch(Exception e)
            {
                string msg = e.Message;
                return null;
            }
        }
        public static List<V_CobrosEnviados> LIstaCompleta()
        {
            try
            {
                string query = $"select *from V_CobrosEnviados";
                string respuesta = DBEntities.ConsultaSQLServer(DBEntities.connectionString, query);
                return JsonConvert.DeserializeObject<List<V_CobrosEnviados>>(respuesta);
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

    }
}
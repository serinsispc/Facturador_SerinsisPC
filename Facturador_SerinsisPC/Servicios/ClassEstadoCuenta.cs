using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Facturador_SerinsisPC.Servicios
{
    public static class ClassEstadoCuenta
    {
        public static bool ActivarCuentaCliente(int idCliente, string mensaje = null)
        {
            try
            {
                List<DatBases> datBases = controladorDatBases.ListdatBases(idCliente) ?? new List<DatBases>();
                string mensajeSeguro = (mensaje ?? "Su servicio ha sido reactivado correctamente.").Replace("'", "''");

                foreach (DatBases lista in datBases)
                {
                    DBEntities.ConexionBase("www.serinsispc.com", lista.nameDataBase, "emilianop", "Ser1ns1s@2020*");
                    string queryAdminControl = $"update AdminControl set tipo_admincontrol=0,mensaje_admincontrol='{mensajeSeguro}'";
                    DBEntities.EjecutarSQLServer(DBEntities.connectionString, queryAdminControl);

                    DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
                    string queryDatBase = $"update DatBases set estado=0 where id={lista.id}";
                    DBEntities.EjecutarSQLServer(DBEntities.connectionString, queryDatBase);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                ClassConexionPrincipal.Configurar();
            }
        }
    }
}

using Facturador_SerinsisPC.Models;

namespace Facturador_SerinsisPC.Servicios
{
    public static class ClassConexionPrincipal
    {
        public static void Configurar()
        {
            DBEntities.ConexionBase("www.serinsispc.com", "DBMensualidadesSerinsisPC", "emilianop", "Ser1ns1s@2020*");
        }
    }
}

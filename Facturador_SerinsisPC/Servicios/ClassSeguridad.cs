using System.Security.Cryptography;

namespace Facturador_SerinsisPC.Servicios
{
    public static class ClassSeguridad
    {
        public static string GenerarCodigoRecuperacion(int longitud = 6)
        {
            if (longitud <= 0)
            {
                longitud = 6;
            }

            char[] codigo = new char[longitud];
            byte[] buffer = new byte[longitud];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            for (int i = 0; i < longitud; i++)
            {
                codigo[i] = (char)('0' + (buffer[i] % 10));
            }

            return new string(codigo);
        }
    }
}

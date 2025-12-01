using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Servicios
{
    public class ClassDateTime
    {
        public static string FormatoDate(DateTime fecha)
        {
            return $"{fecha.Year}-{Mes(fecha.Month)}-{Dia(fecha.Day)}";
        }
        private static string Mes(int mes)
        {
            if(mes < 10)
            {
                return $"0{mes}";
            }
            else
            {
                return $"{mes}"; 
            }
        }
        private static string Dia(int dia)
        {
            if (dia < 10)
            {
                return $"0{dia}";
            }
            else
            {
                return $"{dia}";
            }
        }
    }
}
using System;

namespace Facturador_SerinsisPC.Servicios
{
    public static class ClassCartera
    {
        public static DateTime AjustarDiaPago(DateTime fechaBase, int diaPago)
        {
            int diaSeguro = Math.Min(Math.Max(diaPago, 1), DateTime.DaysInMonth(fechaBase.Year, fechaBase.Month));
            return new DateTime(fechaBase.Year, fechaBase.Month, diaSeguro);
        }

        public static DateTime CalcularPrimeraFechaPago(DateTime fechaInicioPlan, int diaPago)
        {
            DateTime fechaCalculada = AjustarDiaPago(fechaInicioPlan.Date, diaPago);
            if (fechaCalculada < fechaInicioPlan.Date)
            {
                fechaCalculada = AjustarDiaPago(fechaInicioPlan.Date.AddMonths(1), diaPago);
            }

            return fechaCalculada;
        }

        public static DateTime CalcularProximoPagoInicial(DateTime fechaInicioPlan, int diaPago, int periodicidadMeses)
        {
            DateTime primeraFechaPago = CalcularPrimeraFechaPago(fechaInicioPlan, diaPago);
            int meses = periodicidadMeses <= 0 ? 1 : periodicidadMeses;
            return AjustarDiaPago(primeraFechaPago.AddMonths(meses), diaPago);
        }

        public static DateTime CalcularProximaFechaPago(DateTime fechaReferencia, int diaPago, int periodicidadMeses)
        {
            int meses = periodicidadMeses <= 0 ? 1 : periodicidadMeses;
            DateTime siguienteBase = fechaReferencia.Date.AddMonths(meses);
            return AjustarDiaPago(siguienteBase, diaPago);
        }

        public static DateTime CalcularFinPeriodo(DateTime periodoDesde, int periodicidadMeses)
        {
            int meses = periodicidadMeses <= 0 ? 1 : periodicidadMeses;
            return periodoDesde.Date.AddMonths(meses).AddDays(-1);
        }
    }
}

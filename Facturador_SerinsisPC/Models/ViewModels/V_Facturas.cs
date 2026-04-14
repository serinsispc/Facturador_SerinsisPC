using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_Facturas
    {
        public int id {  get; set; }
        public DateTime fechaFactura { get; set; }
        public int idCliente { get; set; }
        public string celular { get; set; }
        public string nombreRepresentate { get; set; }
        public string nombreComercial { get; set; }
        public string correo { get; set; }
        public int yearFactura { get; set; }
        public int idMes { get; set; }
        public string nombreMes { get; set; }
        public decimal valorPlan { get; set; }
        public int sedes { get; set; }
        public decimal valorAPagar { get; set; }
        public int idEstado { get; set; }
        public string nombreEstado { get; set; }
        public int contador { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public DateTime? periodoDesde { get; set; }
        public DateTime? periodoHasta { get; set; }
        public decimal saldoPendiente { get; set; }
        public DateTime? fechaPagoCompleto { get; set; }
    }
}

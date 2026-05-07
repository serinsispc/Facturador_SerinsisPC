using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Services;

public class SolutionBlueprintService
{
    public DashboardSummaryDto GetInitialSummary()
    {
        return new DashboardSummaryDto();
    }

    public IReadOnlyList<ModuleDefinitionDto> GetModules()
    {
        return
        [
            new ModuleDefinitionDto { Name = "Clientes", Route = "/clientes", Description = "Gestión de clientes, contacto y estado comercial." },
            new ModuleDefinitionDto { Name = "Suscripciones", Route = "/suscripciones", Description = "Planes, fechas de cobro y reglas de servicio." },
            new ModuleDefinitionDto { Name = "Bases de Datos", Route = "/bases-datos", Description = "Relación de bases, estado técnico y lectura para POS." },
            new ModuleDefinitionDto { Name = "Facturación", Route = "/facturacion", Description = "Emisión automática y manual de facturas por período." },
            new ModuleDefinitionDto { Name = "Pagos", Route = "/pagos", Description = "Registro de pagos, cruces y reactivación de servicio." },
            new ModuleDefinitionDto { Name = "Ingresos", Route = "/ingresos", Description = "Reporte mensual de ingresos y recaudo real." },
            new ModuleDefinitionDto { Name = "Automatización", Route = "/automatizacion", Description = "Procesos de cobro, aviso, suspensión y bitácora." }
        ];
    }
}

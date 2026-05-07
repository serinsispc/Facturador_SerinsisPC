namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class AutomationLogItemDto
{
    public int Id { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
    public int ProcessedCount { get; set; }
    public int SuccessfulCount { get; set; }
    public int ErrorCount { get; set; }
    public string Details { get; set; } = string.Empty;
}

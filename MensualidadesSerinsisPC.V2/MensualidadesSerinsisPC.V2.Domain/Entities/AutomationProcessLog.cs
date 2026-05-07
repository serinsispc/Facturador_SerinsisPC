namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class AutomationProcessLog
{
    public int Id { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    public int ProcessedCount { get; set; }
    public int SuccessfulCount { get; set; }
    public int ErrorCount { get; set; }
    public string Details { get; set; } = string.Empty;
}

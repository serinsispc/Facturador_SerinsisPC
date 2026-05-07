namespace MensualidadesSerinsisPC.V2.Web.Services;

public sealed class UiFeedbackService
{
    public bool IsLoading { get; private set; }
    public string LoadingMessage { get; private set; } = "Procesando...";

    public event Action? OnChange;

    public void ShowLoading(string? message = null)
    {
        IsLoading = true;
        LoadingMessage = string.IsNullOrWhiteSpace(message) ? "Procesando..." : message;
        OnChange?.Invoke();
    }

    public void HideLoading()
    {
        IsLoading = false;
        LoadingMessage = "Procesando...";
        OnChange?.Invoke();
    }
}

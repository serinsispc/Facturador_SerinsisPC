using Microsoft.JSInterop;

namespace MensualidadesSerinsisPC.V2.Web.Services;

public sealed class SweetAlertService(IJSRuntime jsRuntime)
{
    public ValueTask Success(string title, string text)
        => jsRuntime.InvokeVoidAsync("ms2Alerts.success", title, text);

    public ValueTask Error(string title, string text)
        => jsRuntime.InvokeVoidAsync("ms2Alerts.error", title, text);

    public ValueTask Warning(string title, string text)
        => jsRuntime.InvokeVoidAsync("ms2Alerts.warning", title, text);

    public ValueTask Info(string title, string text)
        => jsRuntime.InvokeVoidAsync("ms2Alerts.info", title, text);

    public async ValueTask<bool> Confirm(string title, string text, string confirmText = "Continuar", string cancelText = "Cancelar")
    {
        return await jsRuntime.InvokeAsync<bool>("ms2Alerts.confirm", title, text, confirmText, cancelText);
    }
}

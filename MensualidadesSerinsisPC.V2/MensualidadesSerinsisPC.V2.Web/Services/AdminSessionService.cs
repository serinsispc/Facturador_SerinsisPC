using MensualidadesSerinsisPC.V2.Web.Options;
using Microsoft.Extensions.Options;

namespace MensualidadesSerinsisPC.V2.Web.Services;

public class AdminSessionService(IOptions<AdminAccessOptions> options)
{
    private readonly AdminAccessOptions _options = options.Value;

    public bool IsInitialized => true;
    public bool IsAuthenticated { get; private set; }
    public string DisplayName { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;

    public event Action? StateChanged;

    public Task<bool> LoginAsync(string username, string password)
    {
        string normalizedUsername = (username ?? string.Empty).Trim();
        string normalizedPassword = (password ?? string.Empty).Trim();

        bool validUser = string.Equals(normalizedUsername, _options.Username.Trim(), StringComparison.OrdinalIgnoreCase);
        bool validPassword = string.Equals(normalizedPassword, _options.Password.Trim(), StringComparison.Ordinal);

        if (!validUser || !validPassword)
        {
            return Task.FromResult(false);
        }

        IsAuthenticated = true;
        Username = _options.Username.Trim();
        DisplayName = string.IsNullOrWhiteSpace(_options.DisplayName) ? Username : _options.DisplayName.Trim();
        NotifyStateChanged();
        return Task.FromResult(true);
    }

    public Task LogoutAsync()
    {
        IsAuthenticated = false;
        Username = string.Empty;
        DisplayName = string.Empty;
        NotifyStateChanged();
        return Task.CompletedTask;
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}

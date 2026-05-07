using MensualidadesSerinsisPC.V2.Infrastructure;
using MensualidadesSerinsisPC.V2.Web.Components;
using MensualidadesSerinsisPC.V2.Web.Options;
using MensualidadesSerinsisPC.V2.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddMensualidadesV2Infrastructure(builder.Configuration);
builder.Services.AddOptions<AdminAccessOptions>()
    .Bind(builder.Configuration.GetSection(AdminAccessOptions.SectionName));
builder.Services.AddScoped<AdminSessionService>();
builder.Services.AddScoped<UiFeedbackService>();
builder.Services.AddScoped<SweetAlertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using MensualidadesSerinsisPC.V2.Infrastructure;
using MensualidadesSerinsisPC.V2.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMensualidadesV2Infrastructure(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

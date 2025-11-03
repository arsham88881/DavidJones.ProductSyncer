
using DavidJones.ProductSyncer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDiLayers();


var host = builder.Build();
host.Run();

using Microsoft.AspNetCore.Components.Web;
using Poc.Data.Application.Services;
using Poc.Data.Infrastructure.Services;
using Poc.Web.Hubs;
using DevExpress.Blazor;

namespace Poc.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Aggiungi i servizi standard per Razor Pages e Blazor Server (se non ci sono già)
            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // REGISTRA I SERVIZI DEVERPRESS BLAZOR QUI
            builder.Services.AddDevExpressBlazor(options => {
                options.BootstrapVersion = BootstrapVersion.v5; // Specifica la tua versione di Bootstrap (v5 o v4)
            });

            builder.Services.AddScoped<IKpiService, KpiService>();

            // DevExpress preferisce PascalCase o la policy predefinita per i suoi helper interni
            builder.Services.AddRazorPages()
                .AddJsonOptions(options =>
                {
                    // Forza la serializzazione delle proprietà in camelCase (es. da VesselName a vesselName)
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            builder.Services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.MapHub<GridFilterHub>("/hubs/messagegrid");

            app.Run();
        }
    }
}

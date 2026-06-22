using System.Text.Json;
using DevExtreme.AspNet.Mvc;
using Poc.Data.Application.Services;
using Poc.Data.Infrastructure.Services;
using Poc.Web.Hubs;

namespace Poc.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();
            app.MapHub<GridFilterHub>("/hubs/messagegrid");

            app.Run();
        }
    }
}

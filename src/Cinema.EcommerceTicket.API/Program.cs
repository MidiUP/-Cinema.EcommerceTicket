using System.Diagnostics.CodeAnalysis;
using Cinema.EcommerceTicket.API.Filters;
using Cinema.EcommerceTicket.Domain;
using Cinema.EcommerceTicket.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.EcommerceTicket.API
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
            .AddNewtonsoftJson()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            builder.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
                options.AppendTrailingSlash = false;
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDomainServices();
            builder.Services.AddInfrastructureServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health", new HealthCheckOptions());

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                builder.Logging.AddConsole();
            }

            app.Run();
        }
    }
}
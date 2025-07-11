using K1QuickGen.Api.Config;
using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Api.Services;
using K1QuickGen.PdfGeneration;
using K1QuickGen.PdfGeneration.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using NLog;
using NLog.Web;
using System;

namespace K1QuickGen.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Replace default logging with NLog
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();

                // Add services
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "K1QuickGen API", Version = "v1" });
                });

                builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings"));

                builder.Services.AddSingleton<IMongoClient>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    return new MongoClient(settings.ConnectionString);
                });

                builder.Services.AddScoped(serviceProvider =>
                {
                    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                    var client = serviceProvider.GetRequiredService<IMongoClient>();
                    return client.GetDatabase(settings.DatabaseName);
                });

                builder.Services.AddScoped<Form1065Repository>();
                builder.Services.AddScoped<PartnerSubmissionRepository>();
                builder.Services.AddScoped<ReturnRepository>();
                builder.Services.AddScoped<IForm1065PdfGenerator, Form1065PdfGenerator>();
                builder.Services.AddScoped<Form1065Service>();
                builder.Services.AddScoped<IForm1065Service, Form1065Service>();
                builder.Services.AddScoped<PartnersSubmissionService>();
                builder.Services.AddScoped<IPartnersSubmissionService, PartnersSubmissionService>();

                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseMiddleware<K1QuickGen.Api.Middleware.ErrorHandlingMiddleware>();

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of an exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }

        }
    }
}
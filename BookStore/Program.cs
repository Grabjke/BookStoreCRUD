using System.Text.Json.Serialization;
using BookStore.Domain;
using BookStore.Domain.Entities;
using BookStore.Domain.Repository.EntityFramework;
using BookStore.Domain.Repository.Interfaces;
using BookStore.Infrastructure;
using BookStore.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace BookStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            //  Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) 
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog(); 

            //  Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            // 
            IConfigurationBuilder configBuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configBuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;

            // 
            builder.Services.AddDbContext<AppDbContext>(x => x.UseNpgsql(config.Database.ConnectionString)
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            //
            builder.Services.AddScoped<IValidator<Book>, BookValidator>();
            builder.Services.AddScoped<IValidator<Author>, AuthorValidator>();
            builder.Services.AddScoped<IValidator<Genre>, GenreValidator>();
            builder.Services.AddScoped<IAuthorRepository, EFAuthor>();
            builder.Services.AddScoped<IBookRepository, EFBook>();
            builder.Services.AddScoped<IGenreRepository, EFGenre>();
            builder.Services.AddTransient<DataManager>();

            

            // 
            builder.Services.AddControllers();

            var app = builder.Build();

            //  Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

         

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            
            app.MapControllers(); 

            await app.RunAsync();
        }
    }
}


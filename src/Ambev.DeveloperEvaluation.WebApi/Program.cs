using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.Seeds;
using Microsoft.OpenApi.Models;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Ambev Developer Evaluation API", 
                    Version = "v1" 
                });

                // Configuração do botão Authorize
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                  Enter 'Bearer' [space] and then your token in the text input below. 
                                  Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
            {
                var connectionString = builder.Configuration.GetValue<bool>("UseDocker") 
                    ? builder.Configuration.GetConnectionString("DockerConnection")
                    : builder.Configuration.GetConnectionString("DefaultConnection");

                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                );
            });

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    try
                    {
                        logger.LogInformation("Waiting for database...");
                        var retryCount = 0;
                        const int MAX_RETRIES = 3;

                        while (retryCount < MAX_RETRIES)
                        {
                            try
                            {
                                logger.LogInformation("Attempting database connection...");
                                await context.Database.CanConnectAsync();

                                logger.LogInformation("Applying migrations...");
                                await context.Database.MigrateAsync();

                                logger.LogInformation("Seeding database...");
                                await DefaultSeeder.SeedAsync(context);
                                
                                logger.LogInformation("Database setup completed successfully");
                                break;
                            }
                            catch (Npgsql.PostgresException ex)
                            {
                                retryCount++;
                                logger.LogWarning(ex, $"Database connection failed. Retrying in 5 seconds... (Attempt {retryCount} of {MAX_RETRIES})");
                                
                                if (retryCount == MAX_RETRIES)
                                {
                                    logger.LogError(ex, "Failed to connect to database after {MaxRetries} attempts", MAX_RETRIES);
                                    throw;
                                }

                                await Task.Delay(5000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while setting up the database");
                        throw;
                    }
                }
            }

            app.UseHttpsRedirection();

            app.UseUnauthorizedHandler();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

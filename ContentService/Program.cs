using ContentService.Data;
using ContentService.Middleware;
using ContentService.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ContentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ContentDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IContentRepository, ContentRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddControllers();
            // Register FluentValidation
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => // Modify this line to include a configuration parameter
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Content API", Version = "v1" });
            });
            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            var app = builder.Build();
            // Apply migrations
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<ContentDbContext>();
            //    dbContext.Database.Migrate();
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
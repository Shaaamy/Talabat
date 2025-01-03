using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services and Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connectionString =
            builder.Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddApplicationServices();
            #endregion

            var app = builder.Build();

            #region Update Database and data seeding

            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = Services.GetRequiredService<StoreDbContext>();
                await dbContext.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured While Applying The Migration");
            }

            #endregion


     

            #region Configure the HTTP request pipeline.


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               app.UseMiddleware<ExceptionMiddleware>();
               app.UseSwaggerMiddlewares();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}

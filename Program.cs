using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                    builder.WithOrigins("https://localhost:5173") // Allow the frontend localhost origin
                           .AllowAnyMethod() // Allow any method (GET, POST, etc.)
                           .AllowAnyHeader() // Allow any headers
                           .AllowCredentials()); // Allow credentials such as cookies or authorization headers
            });

            // Retrieve the connection string for the database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            // Configure Entity Framework Core to use SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // Enable Swagger UI for testing purposes in development
            }

            // Middleware configuration
            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
            app.UseCors("AllowOrigin"); // Enable CORS using the defined policy
            app.UseAuthorization(); // Enable authorization middleware

            // Map the controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}

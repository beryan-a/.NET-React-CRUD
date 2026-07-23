//Program.cs entry file of the application

using System.Runtime.CompilerServices;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

//CORS
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5295").
                          AllowAnyMethod().
                          AllowAnyHeader(); // X-Pagination
                      });
});



// services
builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new ArgumentNullException("connectionString is null");

builder.Services.AddDbContext<AppDbContext>(op=>op.UseSqlite(connectionString));


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins); //CORS

// middlewares: code that runs between request and respond
app.MapControllers();

app.Run();

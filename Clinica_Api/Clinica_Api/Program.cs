using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Clinica_Api.Modelss;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200","https://white-rock-00c5c0110.5.azurestaticapps.net") // Cambia esto por la URL de tu aplicación Angular si es diferente
                            .AllowAnyHeader()
                            .AllowAnyMethod());
});


// Add services to the container.
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Agrega servicios al contenedor.
builder.Services.AddDbContext<DbOliveraClinicaContext>(options =>
    options.UseSqlServer("Data Source=LAPTOP-UB7952GK\\MSSQLSERVER01;Integrated Security=True;Connect Timeout=300;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

builder.Services.AddControllers();

// Agregar servicios al contenedor. K
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
// Configure the HTTP request pipeline.

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

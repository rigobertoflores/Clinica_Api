using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Clinica_Api.Modelss;
using System.Reflection;
using System.Runtime.Loader;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200","https://white-rock-00c5c0110.5.azurestaticapps.net") // Cambia esto por la URL de tu aplicaci�n Angular si es diferente
                            .AllowAnyHeader()
                            .AllowAnyMethod());
});

// Construir la ruta absoluta para la biblioteca nativa
var absolutePath = Path.Combine(AppContext.BaseDirectory, "lib", "libwkhtmltox.dll");

CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();


string path;
string runtimeArchitecture = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

var architectureFolder = (IntPtr.Size == 8) ? "X64\\libwkhtmltox" : "X86\\libwkhtmltox";
string dl = architectureFolder + ".dll";
string so = architectureFolder + ".so";
string dylib = architectureFolder + ".dylib";



// Construir la ruta absoluta para la biblioteca nativa
var projectRootFolder = AppContext.BaseDirectory;



if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    path = Path.Combine(projectRootFolder, "lib",dl);
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    path = Path.Combine(projectRootFolder, "lib",so);
else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    path = Path.Combine(projectRootFolder, "lib",dylib);
else
    throw new InvalidOperationException("Supported OS Platform not found");


context.LoadUnmanagedLibrary(path);

// Agregar servicios al contenedor.
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

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        return LoadUnmanagedDllFromPath(unmanagedDllName);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        throw new NotImplementedException();
    }
}
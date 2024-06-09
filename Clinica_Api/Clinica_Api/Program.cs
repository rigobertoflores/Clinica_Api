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
        builder => builder.WithOrigins("http://localhost:4200","https://white-rock-00c5c0110.5.azurestaticapps.net") // Cambia esto por la URL de tu aplicación Angular si es diferente
                            .AllowAnyHeader()
                            .AllowAnyMethod());
});

// Construir la ruta absoluta para la biblioteca nativa
//var absolutePath = Path.Combine(AppContext.BaseDirectory, "lib", "libwkhtmltox.dll");

//CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();

// Agregar servicios al contenedor.
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
//string path;
//string runtimeArchitecture = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

//var architectureFolder = (IntPtr.Size == 8) ? "x64" : "x86";

//string projectRootFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);


//if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//    path = Path.Combine(projectRootFolder, "runtimes\\win-", architectureFolder, "\\native", "libwkhtmltox.dll");
//else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
//    path = Path.Combine(projectRootFolder, "runtimes\\linux-", runtimeArchitecture, "\\native", "libwkhtmltox.so");
//else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
//    path = Path.Combine(projectRootFolder, "runtimes\\osx-", runtimeArchitecture, "\\native", "libwkhtmltox.dylib");
//else
//    throw new InvalidOperationException("Supported OS Platform not found");


//context.LoadUnmanagedLibrary(path);




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
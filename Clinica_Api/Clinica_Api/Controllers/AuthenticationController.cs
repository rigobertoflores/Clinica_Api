using Clinica_Api.ClassModels;
using Clinica_Api.Modelss;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Clinica_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbOliveraClinicaContext _context;
        private  static string tenantId = "1013c841-bd63-4b43-941b-777e2ea80e22";
        private string clientId = "a452a240-6ce5-41e1-be76-a1eff120432f";
        private string clientSecret = "Qoh8Q~GNwEKf_REzErf-0miNl52wQWUzdSgdFbwB"; // Asegúrate de reemplazar esto con tu client secret real.
        private string redirectUri = "https://localhost:7210/api/authentication/Login";
        private string scope = "openid profile User.Read";
        private string tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

        public AuthenticationController(DbOliveraClinicaContext context)
        {
            _context = context;
        }


        [HttpGet("Login")]
        public async Task<IActionResult> UserLogin()
        {
            try
            {

           
            var p = Request.Query.FirstOrDefault();
            var p1 = Request.QueryString;
            var p2 = Request.Body;

            var code = p.Value;

            var values = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "scope", scope },
            { "code", code },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" },
            { "client_secret", clientSecret }
        };

            using var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(values);
            var response = await httpClient.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("No se pudo obtener el token de acceso.");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TokenResponse>(responseString);

                Redirect("https://localhost:7210/swagger/index.html");

            return Ok(responseData);

                //Redirigir al portal 
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("envio")]
        public IActionResult RedirectToAzureAD()
        {
            var tenantId = "1013c841-bd63-4b43-941b-777e2ea80e22";
            var clientId = "a452a240-6ce5-41e1-be76-a1eff120432f";
            var redirectUri = "https://localhost:7210/api/authentication/Login";
            var responseType = "code";
            var responseMode = "query";
            var scope = "openid profile User.Read";
            var state = "12345"; // Asegúrate de generar un estado seguro y único para cada solicitud para prevenir ataques CSRF.

            var authorizationUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize?client_id={clientId}&response_type={responseType}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_mode={responseMode}&scope={Uri.EscapeDataString(scope)}&state={state}";

            return Redirect(authorizationUrl);

        }
    }
}

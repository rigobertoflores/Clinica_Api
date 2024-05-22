using Clinica_Api.Modelss;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Gmail.v1;
//using Google.Apis.Util.Store;
//using MailKit.Security;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;

using System.Net.Mail;


namespace Clinica_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationEmailController : ControllerBase
    {
        private readonly DbOliveraClinicaContext _context;
        public NotificationEmailController(DbOliveraClinicaContext context)
        {
            _context = context;
        }



        [HttpGet("NotificationEmailController/GetPlantilla")]
        public IActionResult Get()
        {
            var plantilla = _context.Plantillas.ToList();
            return Ok(plantilla);
        }

        [HttpGet("NotificationEmailController/GetPlantillaId/{id:int}")]
        public IActionResult GetById(int id)
        {
            var plantilla = _context.Plantillas.ToList().FirstOrDefault(x => x.Id == id);
            return Ok(plantilla);
        }




        [HttpPost("NotificationEmailController/PostPlantillas")]
        public IActionResult PostPlantillas([FromBody] Plantillas plantilla)
        {
            try
            {
                var context = _context.Plantillas;

                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }
                if (plantilla.Id == 0)
                    _context.Plantillas.Add(plantilla);
                else
                    _context.Plantillas.Update(plantilla);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(plantilla);
        }




        [HttpDelete("NotificationEmailController/DeletePlantilla/{id:int}")]
        public IActionResult Delete(int id)
        {

            var plantillaAEliminar = _context.Plantillas.Where(t => t.Id == id).First();
            if (plantillaAEliminar == null) return NotFound();
            else

                _context.Plantillas.Remove(plantillaAEliminar);
            _context.SaveChanges();
            return Ok(plantillaAEliminar);
        }



        [HttpGet("NotificationEmailController/SendEmail")]
        public IActionResult SendEmail()
        {           

                string emailOrigen = "developertestkat@gmail.com";
                string pass = "susouzafsrjtkcnw";
                string emailDestino = "developertestkat@gmail.com";
                string correoTexto = "este es un correo de prueba";

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailOrigen, "AMECAE");
                mailMessage.To.Add(new MailAddress(emailDestino, "Yo"));
                mailMessage.Body = correoTexto;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "prueba";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, pass);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
                return Ok("Correo enviado con exito");
        }
    }
}

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
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.Json;

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
        public IActionResult PostPlantillas([FromBody] Plantilla plantilla)
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




        #region "Relacion Plantillas Pacientes"

        [HttpGet("NotificationEmailController/PacientesVinculadosPP/{idPlantilla:int}")]
        public IActionResult PacientesVinculados(int? idPlantilla)
        {
            var listRelaciones = new List<RelacionPlantilllaPaciente>();
            try
            {
                var plantilllaPacientes = _context.RelacionPlantilllaPacientes.ToList();
                if (idPlantilla != null)
                {
                    listRelaciones = plantilllaPacientes.Where(pp => pp.PlantillaId == idPlantilla).ToList();
                }
                else
                {
                    listRelaciones = plantilllaPacientes;
                }

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(listRelaciones);
        }

        [HttpPost("NotificationEmailController/AgregarVinculoPlantillasPacientes")]
        public IActionResult AgregarVinculoPP([FromBody] List<RelacionPlantilllaPaciente> listNuevosVinculos)
        {
            List<RelacionPlantilllaPaciente> relacionAgregadas = new List<RelacionPlantilllaPaciente>();
            try
            {
                //[FromBody] string listAgregar,  string listEliminar
                //var listNuevosVinculos = JsonSerializer.Deserialize<List<RelacionPlantilllaPaciente>>(listAgregar);
                //var listDesvincular = JsonSerializer.Deserialize<List<RelacionPlantilllaPaciente>>(listEliminar);

                var context = _context.RelacionPlantilllaPacientes.ToList();
                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }
                if (listNuevosVinculos != null && listNuevosVinculos.Any())
                {
                    listNuevosVinculos.ForEach(vinculo =>
                    {
                        var nuevaRelacion = (new RelacionPlantilllaPaciente()
                        {
                            PlantillaId = vinculo.PlantillaId,
                            PacienteId = vinculo.PacienteId,
                            NombrePlantilla = vinculo.NombrePlantilla,
                            NombrePaciente = vinculo.NombrePaciente,
                            FechaCreacion = DateTime.Now,
                            FechaUltActualizacion = DateTime.Now,
                            Status = StatusCorreo.sinEnviar.GetHashCode().ToString(),
                        });
                        _context.RelacionPlantilllaPacientes.Add(nuevaRelacion);
                        relacionAgregadas.Add(nuevaRelacion);
                    });
                }

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(relacionAgregadas);
        }

        [HttpDelete("NotificationEmailController/EliminarVinculoPlantillasPacientes/{id:int}")]
        public IActionResult EliminarVinculoPP([FromQuery(Name = "id")] int[] ids)
        {
            //[FromBody] List<RelacionPlantilllaPaciente> listDesvincular,
            try
            {
                var context = _context.RelacionPlantilllaPacientes.ToList();

                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }

                //if (listDesvincular != null && listDesvincular.Any() && context.Any())
                //{
                //    listDesvincular.ForEach(vinculo =>
                //    {
                //        var relacionAEliminar = context.Where(x => x.PlantillaId == vinculo.PlantillaId
                //        && x.PacienteId == vinculo.PacienteId).First();

                //        _context.Remove(relacionAEliminar);
                //    });
                //}

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok("Correo enviado con exito");
        }

        #endregion

        #region "Enviar Notificaciones"

        [HttpPost("NotificationEmailController/SendEmail")]
        public IActionResult SendEmail([FromBody] ParametrosModel parametros)
        {
            var fecha = parametros.fecha;
            var pacientesANotificar = ObtenerPacientesPorFecha(fecha);
            var plantilla = parametros.plantilla;
            var pacientePlantilla = _context.RelacionPlantilllaPacientes.ToList();
            var correosEnviados =  new List<RelacionPlantilllaPaciente>();
            var hoy = DateTime.Now.ToString("yyyy-MM-dd");
           
            try
            {
                var cantCorreosEnviados = pacientePlantilla.Where(correos => correos.Status == StatusCorreo.enviado.GetHashCode().ToString() &&
                Convert.ToDateTime(correos.FechaUltActualizacion).ToString("yyyy-MM-dd") == hoy).ToList();
                if (pacientesANotificar.Count() >= 100 || cantCorreosEnviados.Count()>=100)
                {
                    return Ok("El maximo de correos a enviar en un dia es 100");
                }

                pacientesANotificar.ForEach(p =>
                {
                    if (p.Email != null && plantilla.CuerpoEmail != null)
                    {
                        EnviarCorreo(plantilla.Asunto, plantilla.CuerpoEmail, p.Email);

                        RelacionPlantilllaPaciente pacientePlantillaAgregar = new RelacionPlantilllaPaciente()
                        {
                            PlantillaId = plantilla.Id,
                            PacienteId = Convert.ToInt32(p.ID),
                            NombrePlantilla = plantilla.Nombre,
                            NombrePaciente = p.Nombre,
                            FechaCreacion = DateTime.Now,
                            FechaUltActualizacion = DateTime.Now,
                            Status = StatusCorreo.enviado.GetHashCode().ToString()
                        };
                        _context.RelacionPlantilllaPacientes.Add(pacientePlantillaAgregar);
                        _context.SaveChanges();
                        correosEnviados.Add(new RelacionPlantilllaPaciente()
                        {
                            PlantillaId = plantilla.Id,
                            PacienteId = Convert.ToInt32(p.ID),
                            NombrePlantilla = plantilla.Nombre,
                            NombrePaciente = p.Nombre,
                            FechaCreacion = DateTime.Now,
                            FechaUltActualizacion = DateTime.Now,
                            Status = StatusCorreo.enviado.GetHashCode().ToString()

                        });
                    }
                });
                
            }
            catch (Exception ex )
            {
                throw new Exception(ex.ToString());
            }
            
            return Ok(correosEnviados);
        }


        private int EnviarCorreo(string asunto, string correoTexto, string emailDestino)
        {
            try
            {
#if DEBUG
                emailDestino = "developertestkat@gmail.com";
#endif
                string emailOrigen = "developertestkat@gmail.com";
                string pass = "susouzafsrjtkcnw";

                //string correoTexto = "este es un correo de prueba";

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailOrigen, "AMECAE");
                mailMessage.To.Add(new MailAddress(emailDestino, "Yo"));
                mailMessage.Body = correoTexto;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = asunto;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, pass);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
            return (int)StatusCorreo.enviado;
        }


        private List<PacientesCitas> ObtenerPacientesPorFecha(string fecha)
        {
            var pacientes = _context.PacientesInformacionGenerals.ToList();
            List<PacientesCitas> citas = new List<PacientesCitas>();

            try
            {
                citas.AddRange(pacientes.Where(c => c.FechaConsulta == Convert.ToDateTime(fecha)).Select(citas => new PacientesCitas()
                {
                    ID = citas.Id.ToString(),
                    Email = citas.Email,
                    FechaConsulta = citas.FechaConsulta,
                    Nombre = citas.Nombre
                }
                ));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return citas.ToList();
        }

        [HttpGet("NotificationEmailController/ObtenerStatusCorreos")]
        public IActionResult ObtenerStatusCorreos()
        {
            var listRelaciones = new List<RelacionPlantilllaPaciente>();
            try
            {              
                listRelaciones = _context.RelacionPlantilllaPacientes.ToList(); 
               
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(listRelaciones.OrderByDescending(x => x.FechaUltActualizacion));
        }


        #endregion
    }
}


public enum StatusCorreo
{
    sinEnviar = 0,
    enviado = 1,
    error = 2,
}

public class ParametrosModel
{
    public string fecha { get; set; }
    public Plantilla plantilla { get; set; }
}


public class PacientesCitas
{
    public string ID { get; set; }
    public string? Email { get; set; }
    public DateTime? FechaConsulta { get; set; }
    public string? Nombre { get; set; }
}

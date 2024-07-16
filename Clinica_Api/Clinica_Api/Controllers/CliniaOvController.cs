using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Clinica_Api.Modelss;
using System.Globalization;
using System.Collections;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing.Imaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Text.RegularExpressions;
using PdfSharp.Drawing.Layout;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;
using PdfSharp;

namespace Clinica_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CliniaOvController : ControllerBase
    {
        private readonly DbOliveraClinicaContext _context;

        public CliniaOvController(DbOliveraClinicaContext context)
        {
            _context = context;
        }

        [HttpGet("CliniaOvController/MostrarTexto")]
        public IActionResult MostrarTexto()
        {
            try
            {
                var contenido = _context.PacientesInformacionGenerals.Take(10).ToList();

                return Ok(contenido);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
        }

        [HttpGet("CliniaOvController/UsuarioMasactual")]
        public IActionResult MostrarTexto1()
        {
            try
            {
                var contenido = _context.PacientesInformacionGenerals.OrderBy(x => x.FechaConsulta)
                  .Select(x => new
                  {
                      x.FechaConsulta,
                      x.Nombre,
                      x.Sexo,
                      x.Clave
                  });
                return Ok(contenido);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
        }

        [HttpGet("CliniaOvController/GetPacientes")]
        public IActionResult GetPacientes(int pageIndex, int pageSize, string? filter = "")
        {
            try
            {
                var query = _context.PacientesInformacionGenerals.AsQueryable();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.ToLower();
                    query = query.Where(p => p.Nombre.ToLower().Contains(filter));
                }

                var totalPacientes = query.Count();

                var contenido = query.OrderByDescending(x => x.FechaConsulta).Skip(pageIndex * pageSize)
        .Take(pageSize).Select(x => new
        {
            x.FechaConsulta,
            x.Nombre,
            x.Sexo,
            x.Clave,
            x.FechaDeNacimiento,
            x.FechaUltimaConsulta,
            x.Email,
            x.Telefono
        });
                return Ok(new
                {
                    items = contenido,
                    totalCount = totalPacientes
                });
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo obtener la información del paciente. Error: " + ex.Message);
            }
        }

        [HttpGet("CliniaOvController/GetPacientesNotificacionesCitas")]
        public IActionResult GetPacientesConCitas()
        {
            try
            {

                var pacientes = _context.PacientesInformacionGenerals.ToList();
                var pacientesActivos = pacientes.Where(p => p.FechaUltimaConsulta != null && Convert.ToDateTime(p.FechaUltimaConsulta).Year > DateTime.Now.Year - 3).ToList();
                var pacientesInactivos = pacientes.Where(p => p.FechaUltimaConsulta != null && Convert.ToDateTime(p.FechaUltimaConsulta).Year < DateTime.Now.Year - 3).ToList();
                var infoNotificacionPacActivos = pacientesActivos.Select(p => new { p.Id, p.Email, p.FechaConsulta, p.Nombre }).ToList();
                var infoNotificacionPacInactivos = pacientesInactivos.Select(p => new { p.Id, p.Email, p.FechaConsulta, p.Nombre }).ToList();
                //var infoPacientesCitas = pacientes.Where(p => p.FechaUltimaConsulta != null && Convert.ToDateTime(p.FechaUltimaConsulta).Year >= DateTime.Now.Year - 3)
                //    .Select(p => new { p.Email, p.FechaConsulta, p.Nombre }).ToList();
                var infoPacientesCitas = infoNotificacionPacActivos;
                var result = new
                {
                    PacientesActivos = infoNotificacionPacActivos,
                    PacientesInactivos = infoNotificacionPacInactivos,
                    PacientesConCitas = infoPacientesCitas
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
        }



        [HttpGet("CliniaOvController/GetPacienteId/{id:int}")]
        public IActionResult ObtenerPacientePorClave(int id)
        {
            try
            {
                return Ok(getPacienteId(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No seconsultar la información del paciente. Error: " + ex.Message);
            }
        }
        private Object getPacienteId(int id)
        {
            var contenido = _context.PacientesInformacionGenerals
                    .Where(x => x.Clave == id)
                    .FirstOrDefault();

            if (contenido != null)
            {
                var fechaConsultaFormatted = FormatDateToDDMMYYYY(contenido.FechaConsulta);
                var fechaUltimaConsultaFormatted = FormatDateToDDMMYYYY(contenido.FechaUltimaConsulta);

                var result = new
                {
                    contenido.Clave,
                    contenido.FechaDeNacimiento,
                    contenido.Sexo,
                    contenido.EstadoCivil,
                    contenido.Ocupacion,
                    contenido.Domicilio,
                    contenido.Poblacion,
                    contenido.Telefono,
                    contenido.Email,
                    contenido.NombreDelEsposo,
                    contenido.EdadDelEsposo,
                    contenido.OcupacionEsposo,
                    contenido.Referencia,
                    contenido.Diabetes,
                    contenido.Hipertension,
                    contenido.Trombosis,
                    contenido.Cardiopatias,
                    contenido.Cancer,
                    contenido.EnfermedadesGeneticas,
                    contenido.OtraEnfermedad,
                    contenido.Inmunizaciones,
                    contenido.Alcoholismo,
                    contenido.Tabaquismo,
                    contenido.TabaquismoPasivo,
                    contenido.DrogasOmedicamentos,
                    contenido.GrupoSanguineo,
                    contenido.PropiasDeLaInfancia,
                    contenido.Rubeola,
                    contenido.Amigdalitis,
                    contenido.Bronquitis,
                    contenido.Bronconeumonia,
                    contenido.HepatitisViralTipo,
                    contenido.Parasitosis,
                    contenido.Toxoplasmosis,
                    contenido.Citomegalovirus,
                    contenido.Herpes,
                    contenido.Clamydiasis,
                    contenido.Hiv,
                    contenido.Sifilis,
                    contenido.Micosis,
                    contenido.Eip,
                    contenido.DiabetesMellitus,
                    contenido.OtrasEndocrinas,
                    contenido.Nefropatias,
                    contenido.Digestivas,
                    contenido.Neurologicas,
                    contenido.Hematologicas,
                    contenido.Tumores,
                    contenido.Condilomatosis,
                    contenido.Displasias,
                    contenido.Alergia,
                    contenido.Nombre,
                    FechaConsulta = fechaConsultaFormatted,
                    FechaUltimaConsulta = fechaUltimaConsultaFormatted
                };
                return result;

            }
            return null;
        }
        private string FormatDateToDDMMYYYY(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : null;
        }

        [HttpGet("CliniaOvController/GetFotoPaciente/{id:int}")]
        public IActionResult GetFotoPaciente(int id)
        {
            Foto blobData = new Foto();
            try
            {
                blobData = _context.Fotos.Where(x => x.Id == id).FirstOrDefault();
                return Ok(blobData);
            }
            catch (Exception ex)
            {
                return Ok(blobData);
            }

        }

        [HttpPost("CliniaOvController/PostImagenPerfil")]
        public async Task<IActionResult> PostImagenPerfil(IFormFile image, [FromForm] string id)
        {
            Foto blobData = new Foto();
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest("Archivo no enviado");

                var extension = Path.GetExtension(image.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);


                    var imageexist = _context.Fotos.Where(x => x.Id == int.Parse(id)).FirstOrDefault();
                    if (imageexist != null)
                    {
                        imageexist.BlobData = memoryStream.ToArray();
                        _context.Fotos.Update(imageexist);
                    }
                    else
                    {
                        var img = new Foto
                        {
                            BlobData = memoryStream.ToArray(),
                            Id = int.Parse(id)

                        };

                        _context.Fotos.Add(img);
                    }
                    await _context.SaveChangesAsync();
                    blobData = getImagePerfil(int.Parse(id));
                    return Ok(blobData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CliniaOvController/PostPaciente")]
        public IActionResult PostPaciente([FromBody] PacientesInformacionGeneral paciente)
        {
            PacientesInformacionGeneral informacionpaciente = new PacientesInformacionGeneral();
            int clave = 0;
            try
            {
                if (paciente.Clave > 0)
                {
                    informacionpaciente = _context.PacientesInformacionGenerals.FirstOrDefault(x => x.Clave == paciente.Clave);
                    if (informacionpaciente == null || !ModelState.IsValid)
                    {
                        return NotFound();
                    }

                    informacionpaciente.HepatitisViralTipo = paciente.HepatitisViralTipo;
                    informacionpaciente.Rubeola = paciente.Rubeola;
                    informacionpaciente.DiabetesMellitus = paciente.DiabetesMellitus;
                    informacionpaciente.Hiv = paciente.Hiv;
                    informacionpaciente.Amigdalitis = paciente.Amigdalitis;
                    informacionpaciente.Alcoholismo = paciente.Alcoholismo;
                    informacionpaciente.Alergia = paciente.Alergia;
                    informacionpaciente.Bronconeumonia = paciente.Bronconeumonia;
                    informacionpaciente.Bronquitis = paciente.Bronquitis;
                    informacionpaciente.Clamydiasis = paciente.Clamydiasis;
                    informacionpaciente.Displasias = paciente.Displasias;
                    informacionpaciente.Parasitosis = paciente.Parasitosis;
                    informacionpaciente.Sifilis = paciente.Sifilis;
                    informacionpaciente.Trombosis = paciente.Trombosis;
                    informacionpaciente.Cancer = paciente.Cancer;
                    informacionpaciente.Micosis = paciente.Micosis;
                    informacionpaciente.Displasias = paciente.Displasias;
                    informacionpaciente.Cardiopatias = paciente.Cardiopatias;
                    informacionpaciente.Citomegalovirus = paciente.Citomegalovirus;
                    informacionpaciente.Nefropatias = paciente.Nefropatias;
                    informacionpaciente.Neurologicas = paciente.Neurologicas;
                    informacionpaciente.Nombre = paciente.Nombre;
                    informacionpaciente.Tabaquismo = paciente.Tabaquismo;
                    informacionpaciente.TabaquismoPasivo = paciente.TabaquismoPasivo;
                    informacionpaciente.Digestivas = paciente.Digestivas;
                    informacionpaciente.Domicilio = paciente.Domicilio;
                    informacionpaciente.DrogasOmedicamentos = paciente.DrogasOmedicamentos;
                    informacionpaciente.Condilomatosis = paciente.Condilomatosis;
                    informacionpaciente.Diabetes = paciente.Diabetes;
                    informacionpaciente.EdadDelEsposo = paciente.EdadDelEsposo;
                    informacionpaciente.Eip = paciente.Eip;
                    informacionpaciente.Email = paciente.Email;
                    informacionpaciente.EnfermedadesGeneticas = paciente.EnfermedadesGeneticas;
                    informacionpaciente.EstadoCivil = paciente.EstadoCivil;
                    informacionpaciente.FechaConsulta = paciente.FechaConsulta;
                    informacionpaciente.FechaDeNacimiento = paciente.FechaDeNacimiento;
                    informacionpaciente.FechaUltimaConsulta = paciente.FechaUltimaConsulta;
                    informacionpaciente.GrupoSanguineo = paciente.GrupoSanguineo;
                    informacionpaciente.Hematologicas = paciente.Hematologicas;
                    informacionpaciente.Herpes = paciente.Herpes;
                    informacionpaciente.Hipertension = paciente.Hipertension;
                    informacionpaciente.Inmunizaciones = paciente.Inmunizaciones;
                    informacionpaciente.OtraEnfermedad = paciente.OtraEnfermedad;
                    informacionpaciente.OtrasEndocrinas = paciente.OtrasEndocrinas;
                    informacionpaciente.PropiasDeLaInfancia = paciente.PropiasDeLaInfancia;
                    informacionpaciente.Referencia = paciente.Referencia;
                    informacionpaciente.Sexo = paciente.Sexo;
                    informacionpaciente.Telefono = paciente.Telefono;
                    informacionpaciente.NombreDelEsposo = paciente.NombreDelEsposo;
                    informacionpaciente.Ocupacion = paciente.Ocupacion;
                    informacionpaciente.OcupacionEsposo = paciente.OcupacionEsposo;
                    informacionpaciente.Poblacion = paciente.Poblacion;
                    informacionpaciente.Toxoplasmosis = paciente.Toxoplasmosis;
                    informacionpaciente.Trombosis = paciente.Trombosis;
                    _context.PacientesInformacionGenerals.Update(informacionpaciente);
                    clave = informacionpaciente.Clave;
                }
                else
                {

                    informacionpaciente = _context.PacientesInformacionGenerals.OrderByDescending(p => p.Clave).FirstOrDefault();
                    paciente.Clave = informacionpaciente.Clave + 1;
                    _context.PacientesInformacionGenerals.Add(paciente);
                    clave = paciente.Clave;

                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(getPacienteId(clave));
        }

        [HttpPost("CliniaOvController/PostReceta")]
        public IActionResult PostReceta([FromBody] RecetasxPaciente receta)
        {
            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            if (receta == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (receta.Id > 0)
                    _context.Update(receta);
                else
                    _context.Add(receta);

                _context.SaveChanges();
                recetasxpaciente = getAllRecetasPaciente((int)receta.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(recetasxpaciente);
        }

        [HttpPost("CliniaOvController/PostDeleteReceta")]
        public IActionResult PostDeleteReceta([FromBody] RecetasxPaciente receta)
        {
            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            if (receta == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (receta.Id > 0)
                    _context.Remove(receta);

                _context.SaveChanges();
                recetasxpaciente = getAllRecetasPaciente((int)receta.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(recetasxpaciente);
        }

        [HttpGet("CliniaOvController/GetReceta/{id:int}")]
        public IActionResult GetReceta(int id)
        {

            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            try
            {
                recetasxpaciente = getAllRecetasPaciente(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la información del paciente. Error: " + ex.Message);
            }

            return Ok(recetasxpaciente);
        }

        [HttpPost("CliniaOvController/PostHistoria")]
        public IActionResult PostHistoria([FromBody] Historia historia)
        {
            List<Historia> historias = null;
            if (historia == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (historia.Id > 0)
                    _context.Update(historia);
                else
                    _context.Add(historia);

                _context.SaveChanges();
                historias = _context.Historias.ToList();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(historias);
        }

        [HttpPost("CliniaOvController/PostHistoriaDelete")]
        public IActionResult PostHistoriaDelete([FromBody] RecetasxPaciente historia)
        {
            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            if (historia == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (historia.Id > 0)
                    _context.Remove(historia);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(recetasxpaciente);
        }

        [HttpGet("CliniaOvController/GetAllHistorias")]
        public IActionResult GetAllHistoriaPaciente()
        {
            List<Historia> historias = new List<Historia>();
            try
            {
                historias = _context.Historias.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
            return Ok(historias);
        }

        [HttpPost("CliniaOvController/PostExpediente")]
        public IActionResult PostExpediente([FromBody] Expediente expediente)
        {
            Expediente expedienteresult = null;
            if (expediente == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (expediente.Id > 0)
                    _context.Update(expediente);
                else
                    _context.Add(expediente);

                _context.SaveChanges();
                expedienteresult = getAllExpediente(expediente.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(expedienteresult);
        }

        [HttpPost("CliniaOvController/PostExpedienteDelete")]
        public IActionResult PostExpedienteDelete([FromBody] RecetasxPaciente expediente)
        {
            Expediente expedienteresult = null;
            if (expediente == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (expediente.Id > 0)
                    _context.Remove(expediente);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(expedienteresult);
        }

        [HttpGet("CliniaOvController/GetExpediente/{id:int}")]
        public IActionResult GetExpediente(int id)
        {

            Expediente expediente = new Expediente();
            try
            {
                expediente = getAllExpediente(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la información del paciente. Error: " + ex.Message);
            }

            return Ok(expediente);
        }

        [HttpGet("CliniaOvController/GetImagenesPaciente/{id:int}")]
        public IActionResult GetImagenesPaciente(int id)
        {
            List<Imagene> blobData = getAllImage(id);

            return Ok(blobData);
        }

        [HttpPost("CliniaOvController/DeleteImagenPaciente")]
        public IActionResult DeleteImagenPaciente([FromBody] int id)
        {
            List<Imagene> blobData = new List<Imagene>();
            int clave = 0;
            try
            {
                Imagene imagetodelete = _context.Imagenes.Where(imagen => imagen.Id == id).First();
                if (imagetodelete != null)
                {
                    clave = (int)imagetodelete.Clave;
                    _context.Remove(imagetodelete);
                    _context.SaveChanges();
                }

                blobData = getAllImage(clave);
            }
            catch (Exception ex)
            {
                blobData = getAllImage(clave);
                return Ok(blobData);
            }
            return Ok(blobData);
        }

        [HttpPost("CliniaOvController/PostImagen")]
        public async Task<IActionResult> PostImagen(IFormFile image, [FromForm] string id)
        {
            List<Imagene> blobData = new List<Imagene>();
            try
            {
                if (image == null || image.Length == 0)
                    return BadRequest("Archivo no enviado");

                var extension = Path.GetExtension(image.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);

                    var img = new Imagene
                    {
                        BlobData = memoryStream.ToArray(),
                        Letra = "",
                        Ext = extension,
                        Clave = int.Parse(id),
                    };

                    _context.Imagenes.Add(img);
                    await _context.SaveChangesAsync();
                    blobData = getAllImage(int.Parse(id));
                    return Ok(blobData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CliniaOvController/PostNotas")]
        public IActionResult PostNotas([FromBody] Nota notas)
        {
            List<Nota> notasxpaciente = new List<Nota>();
            if (notas == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (notas.Id > 0)
                    _context.Update(notas);
                else
                    _context.Add(notas);

                _context.SaveChanges();
                notasxpaciente = getAllNotasPaciente((int)notas.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(notasxpaciente);
        }

        [HttpPost("CliniaOvController/PostDeleteNotas")]
        public IActionResult PostDeleteNotas([FromBody] Nota nota)
        {
            List<Nota> notasxpaciente = new List<Nota>();
            if (nota == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (nota.Id > 0)
                    _context.Remove(nota);

                _context.SaveChanges();
                notasxpaciente = getAllNotasPaciente((int)nota.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(notasxpaciente);
        }

        [HttpGet("CliniaOvController/GetNotas/{id:int}")]
        public IActionResult GetNotas(int id)
        {

            List<Nota> notas = new List<Nota>();
            try
            {
                notas = getAllNotasPaciente(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la información del paciente. Error: " + ex.Message);
            }

            return Ok(notas);
        }

        [HttpGet("CliniaOvController/GetAllInformeo")]
        public IActionResult GetAllInformeo()
        {
            List<InformeOperatorio> historias = new List<InformeOperatorio>();
            try
            {
                historias = _context.InformeOperatorios.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
            return Ok(historias);
        }

        [HttpPost("CliniaOvController/PostInformeo")]
        public IActionResult PostInformeo([FromBody] InformeOperatorio informe)
        {
            List<InformeOperatorio> informes = null;
            if (informe == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (informe.Id > 0)
                    _context.Update(informe);
                else
                    _context.Add(informe);

                _context.SaveChanges();
                informes = _context.InformeOperatorios.ToList();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(informes);
        }

        [HttpPost("CliniaOvController/PostInformeoDelete")]
        public IActionResult PostInformeoDelete([FromBody] InformeOperatorio informe)
        {
            List<InformeOperatorio> informes = new List<InformeOperatorio>();
            if (informe == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (informe.Id > 0)
                    _context.Remove(informe);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(informes);
        }

        [HttpGet("CliniaOvController/GetInforme/{id:int}")]
        public IActionResult GetInforme(int id)
        {

            InformeExpediente informeexpediente = new InformeExpediente();
            try
            {
                informeexpediente = getAllInformeExpediente(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la información del paciente. Error: " + ex.Message);
            }

            return Ok(informeexpediente);
        }

        [HttpPost("CliniaOvController/PostInformeExpediente")]
        public IActionResult PostInformeExpediente([FromBody] InformeExpediente expediente)
        {
            if (expediente == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (expediente.Id > 0)
                    _context.Update(expediente);
                else
                    _context.Add(expediente);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(expediente);
        }

        [HttpPost("CliniaOvController/PostInformeExpedienteDelete")]
        public IActionResult PostInformeExpedienteDelete([FromBody] InformeExpediente expediente)
        {
            InformeExpediente expedienteresult = null;
            if (expediente == null)
            {
                return NotFound();
            }
            // Guarda los cambios en la base de datos.
            try
            {
                if (expediente.Id > 0)
                    _context.Remove(expediente);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(expedienteresult);
        }
        private List<Nota> getAllNotasPaciente(int id)
        {
            List<Nota> notasxpaciente = new List<Nota>();
            try
            {
                notasxpaciente = _context.Notas.Where(x => x.Clave == id).AsEnumerable().OrderByDescending(x => DateTime.ParseExact(x.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();

            }
            catch (Exception ex)
            {
                return new List<Nota>();
            }
            return notasxpaciente;

        }
        private List<RecetasxPaciente> getAllRecetasPaciente(int id)
        {
            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            try
            {
                recetasxpaciente = _context.RecetasxPacientes.Where(x => x.Clave == id).AsEnumerable().OrderByDescending(x => DateTime.ParseExact(x.Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();

            }
            catch (Exception ex)
            {
                return new List<RecetasxPaciente>();
            }
            return recetasxpaciente;

        }
        private List<Imagene> getAllImage(int id)
        {
            List<Imagene> blobData = new List<Imagene>();
            try
            {
                blobData = _context.Imagenes.Where(x => x.Clave == id).ToList();
            }
            catch (Exception ex)
            {
                return blobData;
            }
            return blobData;

        }

        private List<Complementario> getAllDocumento(int id)
        {
            List<Complementario> blobData = new List<Complementario>();
            try
            {
                blobData = _context.Complementarios.Where(x => x.Clave == id).ToList();
            }
            catch (Exception ex)
            {
                return blobData;
            }
            return blobData;

        }
        private Foto getImagePerfil(int id)
        {
            Foto blobData = new Foto();
            try
            {
                blobData = _context.Fotos.Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return blobData;
            }
            return blobData;

        }
        private Expediente getAllExpediente(int id)
        {
            Expediente expediente = new Expediente();
            try
            {
                expediente = _context.Expedientes.Where(x => x.Clave == id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return expediente;
            }
            return expediente;

        }

        private InformeExpediente getAllInformeExpediente(int id)
        {
            InformeExpediente expediente = new InformeExpediente();
            try
            {
                expediente = _context.InformeExpedientes.Where(x => x.Clave == id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return expediente;
            }
            return expediente;
        }

        [HttpPost("CliniaOvController/Print")]
        public IActionResult PrintDocument([FromBody] PrintText print)
        {
            byte[] pdf = null;

            try
            {
                var textoModificado = AjustarTexto(print.text);
                ConfiguracionPrint lista = _context.ConfiguracionPrints
                    .FirstOrDefault(x => x.Usuario == print.user) ??
                    new ConfiguracionPrint
                    {
                        Largo = 279,
                        Ancho = 210,
                        MargenArriba = 40,
                        MargenAbajo = 170,
                        MargenIzquierdo = 20,
                        MargenDerecho = 20,
                        Espacio = 10,
                        Encabezado = " "
                    };

                  Document doc = new Document();
                Section section = doc.AddSection();

               
                  
                section.PageSetup.PageWidth = Unit.FromMillimeter((double)lista.Ancho);
                section.PageSetup.PageHeight = Unit.FromMillimeter((double)lista.Largo);
                section.PageSetup.Orientation = Orientation.Portrait;

                section.PageSetup.TopMargin = Unit.FromMillimeter((double)lista.MargenArriba);
                section.PageSetup.BottomMargin = Unit.FromMillimeter((double)lista.MargenAbajo);
                section.PageSetup.LeftMargin = Unit.FromMillimeter((double)lista.MargenIzquierdo);
                section.PageSetup.RightMargin = Unit.FromMillimeter((double)lista.MargenDerecho);

                // Agregar el encabezado
                Paragraph header = section.Headers.Primary.AddParagraph();
                header.AddText(lista.Encabezado);
                header.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
                header.Format.Alignment = ParagraphAlignment.Center;
                header.Format.SpaceAfter = Unit.FromMillimeter((double)lista.Espacio);

                // Agregar el texto del cuerpo
                Paragraph para = section.AddParagraph();
                para.Format.Alignment = ParagraphAlignment.Justify;
                para.Format.Font.Name = "Arial";
                para.Format.Font.Size = 8;
                para.AddText(textoModificado);

                // Renderizar el documento en un archivo PDF
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true)
                {
                    Document = doc
                };
                pdfRenderer.RenderDocument();

                // Guardar el PDF en un MemoryStream
                using (MemoryStream stream = new MemoryStream())
                {
                    pdfRenderer.Save(stream, false);
                    pdf = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Devuelve el archivo PDF como un archivo descargable
            return Ok(File(pdf, "application/pdf", "downloaded_file.pdf"));
        }


        [HttpPost("CliniaOvController/PostConfiguraImprimir")]
        public IActionResult ConfiguraImprimir([FromBody] ConfiguracionPrint print)
        {
            List<ConfiguracionPrint> lista = new List<ConfiguracionPrint>();
            if (print == null)
            {
                return NotFound();
            }
            try
            {
                if (print.Id > 0)
                    _context.Update(print);
                else
                    _context.Add(print);

                _context.SaveChanges();
                lista = _context.ConfiguracionPrints.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(lista);

        }

        [HttpGet("CliniaOvController/GetPdfPaciente/{id:int}")]
        public IActionResult GetPdfPaciente(int id)
        {
            List<Imagene> blobData = getAllImage(id);

            return Ok(blobData);
        }

        [HttpPost("CliniaOvController/PostPdf")]
        public async Task<IActionResult> PostPdf(IFormFile documento, [FromForm] string id)
        {
            List<Complementario> blobData = new List<Complementario>();
            try
            {
                if (documento == null || documento.Length == 0)
                    return BadRequest("Archivo no enviado");

                var extension = Path.GetExtension(documento.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await documento.CopyToAsync(memoryStream);

                    if (extension == ".pdf")
                    {
                        memoryStream.Position = 0;
                        Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
                        pdfDocument.LoadFromStream(memoryStream);
                        int totalPages = pdfDocument.Pages.Count;
                        int pageCount = 0;

                        for (int i = 0; i < totalPages && pageCount < 2; i++)
                        {
                            var pageImageStream = new MemoryStream();
                            var imagen = pdfDocument.SaveAsImage(i);
                            using (var image = Image.FromStream(imagen))
                            {
                                image.Save(pageImageStream, ImageFormat.Jpeg);
                            }

                            pageImageStream.Position = 0;

                            var documentosubir = new Complementario
                            {
                                BlobData = pageImageStream.ToArray(),
                                Nombre = documento.FileName,
                                Ext = ".jpg",
                                Clave = int.Parse(id),
                            };
                            _context.Complementarios.Add(documentosubir);

                            pageCount++;
                        }
                    }
                    else
                    {
                        var documentosubir = new Complementario
                        {
                            BlobData = memoryStream.ToArray(),
                            Nombre = documento.FileName,
                            Ext = extension,
                            Clave = int.Parse(id),
                        };
                        _context.Complementarios.Add(documentosubir);
                    }
                    await _context.SaveChangesAsync();
                    blobData = getAllDocumento(int.Parse(id));
                    return Ok(blobData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("CliniaOvController/ListaImpresionUsuario")]
        public IActionResult ListaImpresionUsuario()
        {
            List<ConfiguracionPrint> lista = new List<ConfiguracionPrint>();
            try
            {
                lista = _context.ConfiguracionPrints.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la informacion. Error: " + ex.Message);
            }

            return Ok(lista);

        }

        [HttpGet("CliniaOvController/GetComplementariosPaciente/{id:int}")]
        public IActionResult GetComplementariosPaciente(int id)
        {
            List<Complementario> blobData = getAllDocumento(id);

            return Ok(blobData);
        }

        [HttpPost("CliniaOvController/DeleteComplementario")]
        public IActionResult DeleteComplementario([FromBody] int id)
        {
            List<Complementario> blobData = new List<Complementario>();
            int clave = 0;
            try
            {
                Complementario complementario = _context.Complementarios.Where(compl => compl.Id == id).First();
                if (complementario != null)
                {
                    clave = (int)complementario.Clave;
                    _context.Remove(complementario);
                    _context.SaveChanges();
                }

                blobData = getAllDocumento(clave);
            }
            catch (Exception ex)
            {
                blobData = getAllDocumento(clave);
                return Ok(blobData);
            }
            return Ok(blobData);
        }
        private static byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        [HttpGet("CliniaOvController/GetCitasPorFecha/{fecha}")]
        public IActionResult GetCitasPorFecha([FromRoute] string fecha)
        {

            var pacientes = _context.PacientesInformacionGenerals.ToList();

            try
            {
                var citas = pacientes.Where(c => c.FechaConsulta == Convert.ToDateTime(fecha)).Select(citas => new
                { citas.Id, citas.Email, citas.FechaConsulta, citas.Nombre }
                ).ToList();
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "No se pudo obtener la información de la cita del paciente. Error: " + ex.Message);
            }
            return Ok(pacientes);
        }


        private static string AjustarTexto(string textoConHTML)
        {
            //string textoAjustado = textoConHTML.Replace("<p>", string.Empty); 
            string plainText = Regex.Replace(textoConHTML, "<.*?>", string.Empty);

            // Reemplazar los caracteres \n con saltos de línea
            plainText = plainText.Replace("\n", Environment.NewLine);
            return plainText.ToString();
        }
    }
    public class LayoutHelper
    {
        private readonly PdfSharp.Pdf.PdfDocument _document;
        private readonly XUnit _topPosition;
        private readonly XUnit _bottomMargin;
        private XUnit _currentPosition;

        public LayoutHelper(PdfSharp.Pdf.PdfDocument document, XUnit topPosition, XUnit bottomMargin)
        {
            _document = document;
            _topPosition = topPosition;
            _bottomMargin = bottomMargin;
            // Set a value outside the page - a new page will be created on the first request.
            _currentPosition = bottomMargin + 10000;
        }

        public XUnit GetLinePosition(XUnit requestedHeight)
        {
            return GetLinePosition(requestedHeight, -1f);
        }

        public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
        {
            XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
            if (_currentPosition + required > _bottomMargin)
                CreatePage();
            XUnit result = _currentPosition;
            _currentPosition += requestedHeight;
            return result;
        }

        public XGraphics Gfx { get; private set; }
        public PdfPage Page { get; private set; }

        void CreatePage()
        {
            Page = _document.AddPage();
            Page.Size = PageSize.A4;
            Gfx = XGraphics.FromPdfPage(Page);
            _currentPosition = _topPosition;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Clinica_Api.Modelss;
using System.Collections.Generic;
using Microsoft.Win32;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Globalization;
using System.Collections;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf.IO;
using ImageMagick;
using Microsoft.EntityFrameworkCore;

namespace Clinica_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CliniaOvController : ControllerBase
    {
        private readonly DbOliveraClinicaContext _context;
        private readonly IConverter _converter;

        public CliniaOvController(DbOliveraClinicaContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
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
        public IActionResult GetPacientes()
        {
            try
            {
                var contenido = _context.PacientesInformacionGenerals.OrderByDescending(x => x.FechaConsulta)
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
                byte[] pdf = new byte[1];

                try
                {
                    ConfiguracionPrint lista = new ConfiguracionPrint();
                    lista = _context.ConfiguracionPrints.Where(x => x.Usuario == print.user).FirstOrDefault();
                    if (lista == null)
                    {
                        lista = new ConfiguracionPrint() { Largo = 135, Ancho = 210, MargenArriba = 30, MargenAbajo = 20, MargenIzquierdo = 20, MargenDerecho = 20, Espacio = 10, Encabezado = " " };
                    }

                    PechkinPaperSize custompage = new PechkinPaperSize(lista.Ancho.ToString() + "mm", lista.Largo.ToString() + "mm");


                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                     PaperSize = custompage,
                     Margins = new MarginSettings { Top = (double?)lista.MargenArriba, Bottom = (double?)lista.MargenAbajo, Left = (double?)lista.MargenIzquierdo, Right = (double?)lista.MargenDerecho },
                     DocumentTitle = "Receta",
                },
                        Objects = {
                 new ObjectSettings
                {
                 HtmlContent = print.text,
            WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = new HeaderSettings
            {
                FontSize = 12,
                Center = lista.Encabezado ,
                Spacing = (double?)lista.Espacio,
            }
                }
               }
                    };

                    pdf = _converter.Convert(doc);
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
                            using (var images = new MagickImageCollection())
                            {
                                memoryStream.Position = 0;
                                images.Read(memoryStream, new MagickReadSettings { Density = new Density(300, 300) });

                                int pageCount = 0;
                                foreach (var pageImage in images)
                                {
                                    if (pageCount >= 2) break;

                                    var pageMemoryStream = new MemoryStream();
                                    pageImage.Format = MagickFormat.Jpg;
                                    pageImage.Write(pageMemoryStream);
                                    pageMemoryStream.Position = 0;

                                    var documentosubir = new Complementario
                                    {
                                        BlobData = pageMemoryStream.ToArray(),
                                        Nombre = documento.FileName,
                                        Ext = ".jpg",
                                        Clave = int.Parse(id),
                                    };
                                    _context.Complementarios.Add(documentosubir);
                                    pageCount++;
                                }
                            }
                        }
                        else
                        {
                            var documentosubir = new Complementario
                            {
                                BlobData = memoryStream.ToArray(),
                                Nombre = documento.FileName,
                                Ext = ".jpg",
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
                    //var citas = pacientes.Where(c => c.FechaConsulta == fecha).Select(citas => new
                    //{ citas.Id, citas.Email, citas.FechaConsulta, citas.Nombre }
                    //).ToList();
                    //return Ok(citas);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "No se pudo obtener la información de la cita del paciente. Error: " + ex.Message);
                }
                return Ok(pacientes);
            }

        }
    }
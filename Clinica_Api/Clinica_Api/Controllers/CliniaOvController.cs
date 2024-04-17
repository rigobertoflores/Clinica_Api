using Clinica_Api.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Clinica_Api.Modelss;
using System.Collections.Generic;
using Microsoft.Win32;

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
            var contenido = _context.PacientesInformacionGenerals.OrderByDescending(x => x.FechaConsulta).FirstOrDefault();

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
                var contenido = _context.PacientesInformacionGenerals.OrderByDescending(x => x.FechaConsulta);         
            return Ok(contenido);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }
        }

        [HttpGet("CliniaOvController/GetPacienteId/{id:int}")]
        public IActionResult GetPacienteId(int id)
        {
            var contenido = _context.PacientesInformacionGenerals.Where(x => x.Clave==id).FirstOrDefault();

            return Ok(contenido);
        }         

        [HttpGet("CliniaOvController/GetFotoPaciente/{id:int}")]
        public IActionResult GetFotoPaciente(int id)
        {
            try
            {
                Foto blobData = _context.Fotos.Where(x=>x.Id==id).FirstOrDefault();
                return Ok(blobData);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
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

                    
                    var imageexist=_context.Fotos.Where(x=>x.Id== int.Parse(id)).FirstOrDefault();
                    if (imageexist != null) {
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
            PacientesInformacionGeneral informacionpaciente =new PacientesInformacionGeneral();
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
                }
                else
                {

                    var info = _context.PacientesInformacionGenerals.OrderByDescending(p => p.Clave).FirstOrDefault();
                    paciente.Clave = info.Clave+1;
                    _context.PacientesInformacionGenerals.Add(paciente);
                   
                }
                _context.SaveChanges();
                informacionpaciente = _context.PacientesInformacionGenerals.FirstOrDefault(x => x.Clave == paciente.Clave);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(informacionpaciente);
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
                recetasxpaciente=getAllRecetasPaciente(id);
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
                 historias =_context.Historias.ToList();
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
                expedienteresult = _context.Expedientes.FirstOrDefault();
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

        [HttpPost("CliniaOvController/PostImagen")]
        public async Task<IActionResult> PostImagen(IFormFile image, [FromForm] string id)
        {
            List<Imagene> blobData = new List<Imagene>();
            try { 
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
                    Clave=int.Parse(id),
                };

                _context.Imagenes.Add(img);
                await _context.SaveChangesAsync();
                blobData= getAllImage(int.Parse(id));
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
            InformeExpediente expedienteresult = null;
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
                notasxpaciente = _context.Notas.Where(x => x.Clave == id).OrderByDescending(x => x.Fecha).ToList();

            }
            catch (Exception ex)
            {
                return new List<Nota>();
            }
            return notasxpaciente;

        }
        private List<RecetasxPaciente> getAllRecetasPaciente(int id) {
            List<RecetasxPaciente> recetasxpaciente = new List<RecetasxPaciente>();
            try 
            {
                recetasxpaciente = _context.RecetasxPacientes.Where(x => x.Clave == id).OrderByDescending(x=>x.Fecha).ToList();

            }
            catch (Exception ex)
            {
                return  new List<RecetasxPaciente>();
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
        private static byte[] HexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
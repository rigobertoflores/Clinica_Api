using Clinica_Api.Data;
using Clinica_Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using Clinica_Api.Modelss;

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
            var contenido = _context.PacientesInformacionGenerals.Take(10).ToList();

            return Ok(contenido);
        }

        [HttpGet("CliniaOvController/UsuarioMasactual")]
        public IActionResult MostrarTexto1()
        {
            var contenido = _context.PacientesInformacionGenerals.OrderByDescending(x => x.FechaConsulta).FirstOrDefault();

            return Ok(contenido);
        }


        [HttpGet("CliniaOvController/GetPacientes")]
        public IActionResult GetPacientes()
        {
            var contenido = _context.PacientesInformacionGenerals.Where(x => x.Clave == 429).OrderByDescending(x => x.FechaConsulta).Take(20);

            return Ok(contenido);
        }

        [HttpGet("CliniaOvController/GetPacienteId/{id:int}")]
        public IActionResult GetPacienteId(int id)
        {
            var contenido = _context.PacientesInformacionGenerals.Where(x => x.Clave == id).FirstOrDefault();

            return Ok(contenido);
        }

        [HttpGet("CliniaOvController/GetImagenesPaciente/{id:int}")]
        public IActionResult GetImagenesPaciente(int id)
        {
            List<Imagene> blobData = _context.Imagenes.Where(x => x.Id == id).ToList();
            //int i = 0;
            //if (blobData != null)
            //{
            //    //var id = blobData.Id.ToString();
            //    //byte[] blob = blobData.BlobData as byte[];

            //    //if (blob != null)
            //    //{
            //        //    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Obtiene el directorio base de la aplicación
            //        //    string imagesDirectory = Path.Combine(baseDirectory, "Imagenes"); // Construye el path hacia la carpeta Imagenes

            //        //    Asegurándose de que el directorio existe
            //        //    if (!Directory.Exists(imagesDirectory))
            //        //    {
            //        //        Directory.CreateDirectory(imagesDirectory); // Crea el directorio si no existe
            //        //    }

            //        //    Formando el path final donde se guardará la imagen, incluyendo el nombre del archivo
            //        //    string filePath = Path.Combine(imagesDirectory, $"imagen_{id}.jpg");

            //        //    Especifica el tipo MIME del archivo que estás devolviendo
            //        //    string contentType = "application/octet-stream";

            //        //    Nombre del archivo para el usuario
            //        //    string fileName = "nombre_del_archivo.ext";

            //        //    Usa el método File para crear un FileContentResult con los bytes del archivo, el tipo MIME y el nombre del archivo
            //        //    File(blob, contentType, fileName);
            //        //    Console.WriteLine($"Archivo guardado: {filePath}");
            //        //}



            //        foreach (byte[] blob in blobData)
            //    {
            //        try { 
            //            using (MemoryStream ms = new MemoryStream(blob))
            //            {
            //                using (Image image = Image.FromStream(ms))
            //                {
            //                    // Guardar la imagen en disco
            //                    image.Save("output_image" + i + ".jpg", ImageFormat.Jpeg);
            //                    Console.WriteLine("La imagen ha sido guardada como 'output_image.jpg'.");
            //                i++;
            //                }
            //            }
            //            }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("Exception in image"+i + e);
            //        }
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("No se encontró ningún dato de imagen para cargar.");
            //    }

            return Ok(blobData);
        }

        [HttpGet("CliniaOvController/GetFotoPaciente/{id:int}")]
        public IActionResult GetFotoPaciente(int id)
        {
            List<Foto> blobData = _context.Fotos.Where(x => x.Id == id).ToList();
            return Ok(blobData);
        }

        [HttpGet("CliniaOvController/GetHistoriaPaciente/{id:int}")]
        public IActionResult GetHistoriaPaciente(int id)
        {
            Expediente historia = _context.Expedientes.Where(x => x.Clave == id).First();
            return Ok(historia);
        }

        [HttpPost("CliniaOvController/PostPaciente")]
        public IActionResult PostPaciente([FromBody] PacientesInformacionGeneral paciente)
        {
            PacientesInformacionGeneral informacionpaciente = _context.PacientesInformacionGenerals.FirstOrDefault(x => x.Clave == paciente.Clave);
            if (informacionpaciente == null || !ModelState.IsValid)
            {
                return NotFound();
            }
            try
            {
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
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(informacionpaciente);
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
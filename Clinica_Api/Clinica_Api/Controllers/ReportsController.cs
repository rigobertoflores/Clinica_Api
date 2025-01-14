using Clinica_Api.Modelss;
using Microsoft.AspNetCore.Mvc;

namespace Clinica_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase 
    {
        private readonly DbOliveraClinicaContext _context;

        public ReportsController(DbOliveraClinicaContext context)
        {
            _context = context;
        }

        [HttpPost("ReportsController/UltimasConsultas")]
        public IActionResult UltimasConsultas([FromForm] DateTime fechaInicio, [FromForm] DateTime fechaFin)
     {
            try
            {
                // Filtrar pacientes cuya FechaUltimaConsulta esté entre fechaInicio y fechaFin
                var pacientes = _context.PacientesInformacionGenerals
                    .Where(p => p.FechaUltimaConsulta.HasValue &&
                                p.FechaUltimaConsulta.Value >= fechaInicio &&
                                p.FechaUltimaConsulta.Value <= fechaFin)
                    .Select(p => new
                    {
                        Nombre = p.Nombre,
                        FechaUltimaConsulta = p.FechaUltimaConsulta
                    })
                    .ToList();

                // Preparar la respuesta
                var resultado = new
                {
                    TotalPacientes = pacientes.Count,
                    DetallePacientes = pacientes.OrderBy(x=> x.FechaUltimaConsulta)
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante la consulta.
                return StatusCode(500, "Error al consultar la información: " + ex.Message);
            }
        }

        //[HttpPost("ReportsController/RangosDeEdad")]
        //public IActionResult RangosDeEdad()
        //{
        //    try
        //    {
        //        // Obtener todos los pacientes
        //        var pacientes = _context.PacientesInformacionGenerals
        //            .Where(p => p.FechaDeNacimiento != null)
        //            .Select(p => new
        //            {
        //                Nombre = p.Nombre,
        //                FechaDeNacimiento = DateTime.Parse(p.FechaDeNacimiento),
        //                Edad = DateTime.Now.Year - DateTime.Parse(p.FechaDeNacimiento).Year -
        //                        (DateTime.Now.DayOfYear < DateTime.Parse(p.FechaDeNacimiento).DayOfYear ? 1 : 0)
        //            })
        //            .ToList();

        //        // Definir los rangos de edad
        //        var rangosDeEdad = new Dictionary<string, Func<int, bool>>
        //{
        //    { "Niños (0-12 años)", edad => edad >= 0 && edad <= 12 },
        //    { "Adolescentes (13-17 años)", edad => edad >= 13 && edad <= 17 },
        //    { "Adultos Jóvenes (18-35 años)", edad => edad >= 18 && edad <= 35 },
        //    { "Adultos (36-59 años)", edad => edad >= 36 && edad <= 59 },
        //    { "Adultos Mayores (60+ años)", edad => edad >= 60 }
        //};

        //        // Clasificar pacientes por rango de edad
        //        var pacientesPorRango = rangosDeEdad.ToDictionary(
        //            rango => rango.Key,
        //            rango => pacientes
        //                .Where(p => rango.Value(p.Edad))
        //                .Select(p => new { p.Nombre, p.Edad })
        //                .ToList()
        //        );

        //        // Crear el resultado con el conteo por cada rango
        //        var resultado = pacientesPorRango.Select(rango => new
        //        {
        //            Rango = rango.Key,
        //            TotalPacientes = rango.Value.Count,
        //            DetallePacientes = rango.Value
        //        });

        //        return Ok(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Maneja cualquier error que pueda ocurrir durante la consulta.
        //        return StatusCode(500, "Error al calcular los rangos de edad: " + ex.Message);
        //    }
        //}

        [HttpPost("ReportsController/RangosDeEdad")]
        public IActionResult RangosDeEdad()
        {
            try
            {
                // Obtener todos los pacientes con fecha de nacimiento válida
                var pacientes = _context.PacientesInformacionGenerals
                    .Where(p => p.FechaDeNacimiento != null)
                    .Select(p => new
                    {
                        Edad = DateTime.Now.Year - DateTime.Parse(p.FechaDeNacimiento).Year -
                               (DateTime.Now.DayOfYear < DateTime.Parse(p.FechaDeNacimiento).DayOfYear ? 1 : 0)
                    })
                    .ToList();

                // Contar los pacientes en cada rango de edad
                var resultado = new
                {
                    Rango_0_18 = pacientes.Count(p => p.Edad >= 0 && p.Edad <= 18),
                    Rango_19_35 = pacientes.Count(p => p.Edad >= 19 && p.Edad <= 35),
                    Rango_36_50 = pacientes.Count(p => p.Edad >= 36 && p.Edad <= 50),
                    Rango_51_Plus = pacientes.Count(p => p.Edad >= 51)
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, "Error al calcular los rangos de edad: " + ex.Message);
            }
        }


    }
}

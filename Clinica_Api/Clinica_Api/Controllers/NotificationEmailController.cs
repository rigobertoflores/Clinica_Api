using Clinica_Api.Modelss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

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


        //[HttpPost("NotificationEmailController/EditPlantilla")]

        //public IActionResult Edit([FromBody] Plantillas plantilla)
        //{
        //    Plantillas plantillaModificada = null;
        //    try
        //    {
        //        var context = _context.Plantillas.ToList();

        //        if (context == null || !ModelState.IsValid)
        //        {
        //            return NotFound();
        //        }
        //        plantillaModificada = new Plantillas()
        //        {
        //            Nombre = plantilla.Nombre,
        //            Asunto = plantilla.Asunto,
        //            CuerpoEmail = plantilla.CuerpoEmail,
        //            FechaEnvio = plantilla.FechaEnvio,
        //            Adjunto = ""
        //        };
        //        _context.Plantillas.Update(plantillaModificada);
        //        _context.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        // Maneja cualquier error que pueda ocurrir durante el guardado.
        //        return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
        //    }

        //    return Ok(plantillaModificada);
        //}


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
    }
}

using Clinica_Api.Modelss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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




        [HttpPost("NotificationEmailController/PostInsertPlantillas")]
        public IActionResult PostPlantillas([FromBody] Plantillas plantilla)
        {
            Plantillas plan = null;
            try
            {
                var context = _context.Plantillas;

                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }
                plan = new Plantillas()
                {
                    Id = 0,
                    Nombre = plantilla.Nombre,
                    Asunto = plantilla.Asunto,
                    CuerpoEmail = plantilla.CuerpoEmail,
                    FechaEnvio = plantilla.FechaEnvio,
                    Adjunto = plantilla.Adjunto

                };
                _context.Plantillas.Add(plan);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(plan);
        }


        //[HttpPost("NotificationEmailController/EditTratamiento/{id:int}")]

        //public IActionResult Edit(int id, [FromBody] TratamientosEnfermedade tratamientosEnfermedades)
        //{
        //    TratamientosEnfermedade tratamiento = null;
        //    try
        //    {
        //        var context = _context.TratamientosEnfermedades;

        //        if (context == null || !ModelState.IsValid)
        //        {
        //            return NotFound();
        //        }
        //        tratamiento = new TratamientosEnfermedade()
        //        {
        //            Id = id,
        //            Nombre = tratamientosEnfermedades.Nombre,
        //            DescripcionEnfermedad = tratamientosEnfermedades.DescripcionEnfermedad,
        //            Tratamiento = tratamientosEnfermedades.Tratamiento,
        //            PalabrasClaves = tratamientosEnfermedades.PalabrasClaves,
        //        };
        //        _context.TratamientosEnfermedades.Update(tratamiento);
        //        _context.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        // Maneja cualquier error que pueda ocurrir durante el guardado.
        //        return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
        //    }

        //    return Ok(tratamiento);

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

using Clinica_Api.Modelss;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Clinica_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TratamientosController : ControllerBase
    {
        private readonly DbOliveraClinicaContext _context;
        public TratamientosController(DbOliveraClinicaContext context)
        {
            _context = context;
        }


        
        [HttpGet("TratamientosController/GetTratamientos")]
        public IActionResult Get()
        {
            var trata = _context.TratamientosEnfermedades.ToList(); 
            return Ok(trata);
        }

        [HttpGet("TratamientosController/GetTratamientoId/{id:int}")]
        public IActionResult GetById(int id)
        {
            var trata = _context.TratamientosEnfermedades.ToList().FirstOrDefault(x=>x.Id == id);
            return Ok(trata);
        }




        [HttpPost("TratamientosController/PostInsertTratamientos")]
        public IActionResult PostTratamientos([FromBody] TratamientosEnfermedade tratamientosEnfermedades)
        {
            TratamientosEnfermedade tratamiento = null;
            try
            {
                var context = _context.TratamientosEnfermedades;

                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }
                tratamiento = new TratamientosEnfermedade()
                {
                    Id = 0,
                    Nombre = tratamientosEnfermedades.Nombre,
                    DescripcionEnfermedad = tratamientosEnfermedades.DescripcionEnfermedad,
                    Tratamiento = tratamientosEnfermedades.Tratamiento,
                    PalabrasClaves = tratamientosEnfermedades.PalabrasClaves,
                };
                _context.TratamientosEnfermedades.Add(tratamiento);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(tratamiento);
        }


        [HttpPost("TratamientosController/EditTratamiento/{id:int}")]
        
        public IActionResult Edit(int id, [FromBody] TratamientosEnfermedade tratamientosEnfermedades)
        {
            TratamientosEnfermedade tratamiento = null;
            try
            {
                var context = _context.TratamientosEnfermedades;

                if (context == null || !ModelState.IsValid)
                {
                    return NotFound();
                }
                tratamiento = new TratamientosEnfermedade()
                {
                    Id= id,
                    Nombre = tratamientosEnfermedades.Nombre,
                    DescripcionEnfermedad = tratamientosEnfermedades.DescripcionEnfermedad,
                    Tratamiento = tratamientosEnfermedades.Tratamiento,
                    PalabrasClaves = tratamientosEnfermedades.PalabrasClaves,
                };
                _context.TratamientosEnfermedades.Update(tratamiento);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                // Maneja cualquier error que pueda ocurrir durante el guardado.
                return StatusCode(500, "No se pudo guardar la información del paciente. Error: " + ex.Message);
            }

            return Ok(tratamiento);
        }

        [HttpDelete("TratamientosController/DeleteTratamiento/{id:int}")]
            public IActionResult Delete(int id)
        {
            
            var trataAEliminar = _context.TratamientosEnfermedades.Where(t=>t.Id == id).First();
            if(trataAEliminar == null) return NotFound();
            else

            _context.TratamientosEnfermedades.Remove(trataAEliminar);
            _context.SaveChanges();
            return Ok(trataAEliminar) ;
        }
    }
}

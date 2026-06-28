using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly BDdata _context;

        public WeatherForecastController(BDdata context)
        {
            _context = context;
        }

        [HttpGet("GetPuesto")]
        public async Task<IActionResult> GetPuesto()
        {
            try
            {
                var empleados = await _context.Set<Puesto>()
                    .FromSqlRaw("EXEC sp_Puesto")
                    .ToListAsync();

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en la API: {ex.Message}");
            }
        }

        [HttpGet("GetSucursal")]
        public async Task<IActionResult> GetSucursal()
        {
            try
            {
                var empleados = await _context.Set<Sucursal>()
                    .FromSqlRaw("EXEC sp_Sucursal")
                    .ToListAsync();

                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en la API: {ex.Message}");
            }
        }


        [HttpGet("GetEmpleados")]
        public async Task<ActionResult<IEnumerable<EmpleadosTDO>>> BuscarPorPuesto([FromQuery] int? puesto)
        {
            try
            {
                var resultados = await _context.GetEmpleadosbyID
                    .FromSqlInterpolated($"EXEC sp_GetEmpleados @ID_Puesto = {puesto}")
                    .ToListAsync();

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

       
        [HttpPost("CrearEmpleado")]
        public async Task<IActionResult> CrearEmpleado([FromBody] Empleados nuevoEmpleado)
        {


            ModelState.Remove(nameof(nuevoEmpleado.IDSucursal));
            ModelState.Remove(nameof(nuevoEmpleado.IDPuesto));           

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (nuevoEmpleado == null)
            {
                return BadRequest("Los datos Invalida.");
            }

            try
            {              
                _context.Empleados.Add(nuevoEmpleado);
                await _context.SaveChangesAsync();

            
                return Ok(new { mensaje = "Empleado guardado con éxito en la base de datos." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al guardar: {ex.Message}");
            }
        }



    }
}

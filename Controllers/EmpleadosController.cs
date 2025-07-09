using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiGestionEmpleados.Modelo.Dtos;
using ApiGestionEmpleados.Repositorio.IRepositorio;
using ApiGestionEmpleados.GestionEmpleadosMapper;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionEmpleados.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoRepositorio _empleadoRepo;
        private readonly IRolRepositorio _rolRepo;

        public EmpleadosController(IEmpleadoRepositorio empleadoRepo, IRolRepositorio rolRepo)
        {
            _empleadoRepo = empleadoRepo;
            _rolRepo = rolRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetEmpleados()
        {
            var empleados = await _empleadoRepo.GetAllAsync();
            return Ok(empleados.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetEmpleado(int id)
        {
            var empleado = await _empleadoRepo.GetByIdAsync(id);

            if (empleado == null)
            {
                return NotFound($"No se encontró el empleado con ID {id}");
            }

            return Ok(empleado.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDto>> CreateEmpleado(EmpleadoCreateDto createDto)
        {
            if (!await _rolRepo.ExistsAsync(createDto.RolId))
            {
                return BadRequest($"No existe el rol con ID {createDto.RolId}");
            }

            if (await _empleadoRepo.EmailExistsAsync(createDto.Email))
            {
                return BadRequest($"Ya existe un empleado con el email {createDto.Email}");
            }

            try
            {
                var empleado = createDto.ToEntity();
                var nuevoEmpleado = await _empleadoRepo.CreateAsync(empleado);
                return CreatedAtAction(
                    nameof(GetEmpleado),
                    new { id = nuevoEmpleado.Id },
                    nuevoEmpleado.ToDto()
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpleadoDto>> UpdateEmpleado(int id, EmpleadoUpdateDto updateDto)
        {
            var empleadoExistente = await _empleadoRepo.GetByIdAsync(id);
            if (empleadoExistente == null)
            {
                return NotFound($"No se encontró el empleado con ID {id}");
            }

            if (!await _rolRepo.ExistsAsync(updateDto.RolId))
            {
                return BadRequest($"No existe el rol con ID {updateDto.RolId}");
            }

            if (await _empleadoRepo.EmailExistsAsync(updateDto.Email, id))
            {
                return BadRequest($"Ya existe otro empleado con el email {updateDto.Email}");
            }

            try
            {
                updateDto.UpdateEntity(empleadoExistente);
                var empleadoActualizado = await _empleadoRepo.UpdateAsync(empleadoExistente);
                return Ok(empleadoActualizado.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var resultado = await _empleadoRepo.DeleteAsync(id);

            if (!resultado)
            {
                return NotFound($"No se encontró el empleado con ID {id}");
            }

            return NoContent();
        }

        [HttpGet("rol/{rolId}")]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetEmpleadosByRol(int rolId)
        {
            if (!await _rolRepo.ExistsAsync(rolId))
            {
                return NotFound($"No existe el rol con ID {rolId}");
            }

            var empleados = await _empleadoRepo.GetByRolAsync(rolId);
            return Ok(empleados.ToDto());
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> SearchEmpleados([Required] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("El término de búsqueda es requerido");
            }

            var empleados = await _empleadoRepo.SearchAsync(term);
            return Ok(empleados.ToDto());
        }
    }
}
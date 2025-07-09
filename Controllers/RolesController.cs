using ApiGestionEmpleados.GestionEmpleadosMapper;
using ApiGestionEmpleados.Modelo.Dtos;
using ApiGestionEmpleados.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionEmpleados.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolRepositorio _rolRepo;

        public RolesController(IRolRepositorio rolRepo)
        {
            _rolRepo = rolRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()
        {
            var roles = await _rolRepo.GetAllAsync();
            return Ok(roles.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolDto>> GetRol(int id)
        {
            var rol = await _rolRepo.GetByIdAsync(id);

            if (rol == null)
            {
                return NotFound($"No se encontró el rol con ID {id}");
            }

            return Ok(rol.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<RolDto>> CreateRol(RolCreateDto createDto)
        {
            // Validar que el nombre no existe
            if (await _rolRepo.NameExistsAsync(createDto.Nombre))
            {
                return BadRequest($"Ya existe un rol con el nombre {createDto.Nombre}");
            }

            try
            {
                var rol = createDto.ToEntity();
                var nuevoRol = await _rolRepo.CreateAsync(rol);
                return CreatedAtAction(
                    nameof(GetRol),
                    new { id = nuevoRol.Id },
                    nuevoRol.ToDto()
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RolDto>> UpdateRol(int id, RolUpdateDto updateDto)
        {
            var rolExistente = await _rolRepo.GetByIdAsync(id);
            if (rolExistente == null)
            {
                return NotFound($"No se encontró el rol con ID {id}");
            }

            if (await _rolRepo.NameExistsAsync(updateDto.Nombre, id))
            {
                return BadRequest($"Ya existe otro rol con el nombre {updateDto.Nombre}");
            }

            try
            {
                updateDto.UpdateEntity(rolExistente);
                var rolActualizado = await _rolRepo.UpdateAsync(rolExistente);
                return Ok(rolActualizado.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                var resultado = await _rolRepo.DeleteAsync(id);

                if (!resultado)
                {
                    return NotFound($"No se encontró el rol con ID {id}");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
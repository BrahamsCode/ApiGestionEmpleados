using Microsoft.EntityFrameworkCore;
using ApiGestionEmpleados.Data;
using ApiGestionEmpleados.Modelo;
using ApiGestionEmpleados.Repositorio.IRepositorio;

namespace ApiGestionEmpleados.Repositorio
{
    public class EmpleadoRepositorio : IEmpleadoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados
                .Include(e => e.Rol)
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        }

        public async Task<Empleado> CreateAsync(Empleado empleado)
        {
            empleado.FechaCreacion = DateTime.Now;
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            // Cargar el rol para devolver el objeto completo
            await _context.Entry(empleado)
                .Reference(e => e.Rol)
                .LoadAsync();

            return empleado;
        }

        public async Task<Empleado> UpdateAsync(Empleado empleado)
        {
            empleado.FechaActualizacion = DateTime.Now;
            _context.Entry(empleado).State = EntityState.Modified;

            // No actualizar estos campos
            _context.Entry(empleado).Property(e => e.FechaCreacion).IsModified = false;

            await _context.SaveChangesAsync();

            // Cargar el rol para devolver el objeto completo
            await _context.Entry(empleado)
                .Reference(e => e.Rol)
                .LoadAsync();

            return empleado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return false;

            // Soft delete
            empleado.Activo = false;
            empleado.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Empleados
                .AnyAsync(e => e.Id == id && e.Activo);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Empleados.Where(e => e.Email == email && e.Activo);

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Empleado>> GetByRolAsync(int rolId)
        {
            return await _context.Empleados
                .Include(e => e.Rol)
                .Where(e => e.RolId == rolId && e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Empleado>> SearchAsync(string searchTerm)
        {
            return await _context.Empleados
                .Include(e => e.Rol)
                .Where(e => e.Activo &&
                           (e.Nombre.Contains(searchTerm) ||
                            e.Apellido.Contains(searchTerm) ||
                            e.Email.Contains(searchTerm)))
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }
    }
}
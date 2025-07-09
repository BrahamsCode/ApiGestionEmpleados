using Microsoft.EntityFrameworkCore;
using ApiGestionEmpleados.Data;
using ApiGestionEmpleados.Modelo;
using ApiGestionEmpleados.Repositorio.IRepositorio;

namespace ApiGestionEmpleados.Repositorio
{
    public class RolRepositorio : IRolRepositorio
    {
        private readonly ApplicationDbContext _context;

        public RolRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && r.Activo);
        }

        public async Task<Rol> CreateAsync(Rol rol)
        {
            rol.FechaCreacion = DateTime.Now;
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> UpdateAsync(Rol rol)
        {
            _context.Entry(rol).State = EntityState.Modified;
            _context.Entry(rol).Property(r => r.FechaCreacion).IsModified = false;

            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            // Verificar si hay empleados con este rol
            var hasEmpleados = await _context.Empleados
                .AnyAsync(e => e.RolId == id && e.Activo);

            if (hasEmpleados)
            {
                throw new InvalidOperationException("No se puede eliminar el rol porque tiene empleados asignados");
            }

            rol.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Roles
                .AnyAsync(r => r.Id == id && r.Activo);
        }

        public async Task<bool> NameExistsAsync(string nombre, int? excludeId = null)
        {
            var query = _context.Roles.Where(r => r.Nombre == nombre && r.Activo);

            if (excludeId.HasValue)
            {
                query = query.Where(r => r.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
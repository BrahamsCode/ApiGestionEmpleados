using ApiGestionEmpleados.Modelo;

namespace ApiGestionEmpleados.Repositorio.IRepositorio
{
    public interface IEmpleadoRepositorio
    {
        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado?> GetByIdAsync(int id);
        Task<Empleado> CreateAsync(Empleado empleado);
        Task<Empleado> UpdateAsync(Empleado empleado);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<IEnumerable<Empleado>> GetByRolAsync(int rolId);
        Task<IEnumerable<Empleado>> SearchAsync(string searchTerm);
    }
}
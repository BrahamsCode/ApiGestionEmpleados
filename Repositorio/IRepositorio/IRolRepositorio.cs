using ApiGestionEmpleados.Modelo;

namespace ApiGestionEmpleados.Repositorio.IRepositorio
{
    public interface IRolRepositorio
    {
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
        Task<Rol> CreateAsync(Rol rol);
        Task<Rol> UpdateAsync(Rol rol);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string nombre, int? excludeId = null);
    }
}
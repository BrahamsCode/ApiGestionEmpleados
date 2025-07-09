using Microsoft.AspNetCore.Identity;

namespace ApiGestionEmpleados.Modelo
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ApiGestionEmpleados.Modelo
{
    public class Rol
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del rol no puede exceder 50 caracteres")]
        public string Nombre { get; set; } = string.Empty; // ✅ Inicializar con string vacío

        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Navegación
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}

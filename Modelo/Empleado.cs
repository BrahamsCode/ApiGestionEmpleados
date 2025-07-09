using System.ComponentModel.DataAnnotations;

namespace ApiGestionEmpleados.Modelo
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty; // ✅ Inicializar con string vacío

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; } = string.Empty; // ✅ Inicializar con string vacío

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty; // ✅ Inicializar con string vacío

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal Salario { get; set; }

        public int RolId { get; set; }
        public virtual Rol? Rol { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
    }
}
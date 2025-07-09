using System.ComponentModel.DataAnnotations;

namespace ApiGestionEmpleados.Modelo.Dtos
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaContratacion { get; set; }
        public decimal Salario { get; set; }
        public int RolId { get; set; }
        public required string RolNombre { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class EmpleadoCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }
    }

    public class EmpleadoUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ApiGestionEmpleados.Modelo.Dtos
{
    public class RolDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class RolCreateDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del rol no puede exceder 50 caracteres")]
        public required string Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string? Descripcion { get; set; }
    }

    public class RolUpdateDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del rol no puede exceder 50 caracteres")]
        public required string Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string? Descripcion { get; set; }
    }
}
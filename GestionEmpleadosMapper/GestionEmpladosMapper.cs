using ApiGestionEmpleados.Modelo;
using ApiGestionEmpleados.Modelo.Dtos;

namespace ApiGestionEmpleados.GestionEmpleadosMapper
{
    public static class GestionEmpleadosMapper
    {
        public static EmpleadoDto ToDto(this Empleado empleado)
        {
            return new EmpleadoDto
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Email = empleado.Email,
                Telefono = empleado.Telefono,
                FechaContratacion = empleado.FechaContratacion,
                Salario = empleado.Salario,
                RolId = empleado.RolId,
                RolNombre = empleado.Rol?.Nombre ?? "",
                Activo = empleado.Activo,
                FechaCreacion = empleado.FechaCreacion,
                FechaActualizacion = empleado.FechaActualizacion
            };
        }

        public static Empleado ToEntity(this EmpleadoCreateDto createDto)
        {
            return new Empleado
            {
                Nombre = createDto.Nombre,
                Apellido = createDto.Apellido,
                Email = createDto.Email,
                Telefono = createDto.Telefono,
                FechaContratacion = createDto.FechaContratacion,
                Salario = createDto.Salario,
                RolId = createDto.RolId,
                Activo = true
            };
        }

        public static void UpdateEntity(this EmpleadoUpdateDto updateDto, Empleado empleado)
        {
            empleado.Nombre = updateDto.Nombre;
            empleado.Apellido = updateDto.Apellido;
            empleado.Email = updateDto.Email;
            empleado.Telefono = updateDto.Telefono;
            empleado.FechaContratacion = updateDto.FechaContratacion;
            empleado.Salario = updateDto.Salario;
            empleado.RolId = updateDto.RolId;
        }

        // Mappers para Rol
        public static RolDto ToDto(this Rol rol)
        {
            return new RolDto
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Activo = rol.Activo,
                FechaCreacion = rol.FechaCreacion
            };
        }

        public static Rol ToEntity(this RolCreateDto createDto)
        {
            return new Rol
            {
                Nombre = createDto.Nombre,
                Descripcion = createDto.Descripcion,
                Activo = true
            };
        }

        public static void UpdateEntity(this RolUpdateDto updateDto, Rol rol)
        {
            rol.Nombre = updateDto.Nombre;
            rol.Descripcion = updateDto.Descripcion;
        }

        // Mappers para colecciones
        public static IEnumerable<EmpleadoDto> ToDto(this IEnumerable<Empleado> empleados)
        {
            return empleados.Select(e => e.ToDto());
        }

        public static IEnumerable<RolDto> ToDto(this IEnumerable<Rol> roles)
        {
            return roles.Select(r => r.ToDto());
        }
    }
}
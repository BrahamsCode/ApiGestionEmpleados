using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApiGestionEmpleados.Modelo;

namespace ApiGestionEmpleados.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.Salario).HasColumnType("decimal(18,2)");

                // Relación con Rol
                entity.HasOne(e => e.Rol)
                      .WithMany(r => r.Empleados)
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Índices
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de la entidad Rol
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Descripcion).HasMaxLength(200);

                // Índice único para el nombre del rol
                entity.HasIndex(r => r.Nombre).IsUnique();
            });

            // Datos semilla para Roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Descripcion = "Acceso completo al sistema",
                    FechaCreacion = new DateTime(2024, 1, 1),
                    Activo = true
                },
                new Rol
                {
                    Id = 2,
                    Nombre = "Gerente",
                    Descripcion = "Gestión de equipos y proyectos",
                    FechaCreacion = new DateTime(2024, 1, 1),
                    Activo = true
                },
                new Rol
                {
                    Id = 3,
                    Nombre = "Desarrollador",
                    Descripcion = "Desarrollo de software",
                    FechaCreacion = new DateTime(2024, 1, 1),
                    Activo = true
                },
                new Rol
                {
                    Id = 4,
                    Nombre = "Analista",
                    Descripcion = "Análisis de sistemas y datos",
                    FechaCreacion = new DateTime(2024, 1, 1),
                    Activo = true
                }
            );
        }
    }
}
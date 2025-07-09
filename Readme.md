# API Gestión de Empleados

## Descripción General

La API de Gestión de Empleados es una aplicación web desarrollada en ASP.NET Core que permite administrar empleados y sus roles dentro de una organización. El sistema incluye autenticación JWT, autorización basada en roles y operaciones CRUD completas.

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core 9.0+
- **Base de Datos**: SQL Server con Entity Framework Core
- **Autenticación**: JWT (JSON Web Tokens)
- **Autorización**: ASP.NET Core Identity
- **Mapeo de Objetos**: AutoMapper
- **Documentación**: Swagger/OpenAPI
- **Patrones**: Repository Pattern

## Arquitectura del Sistema

### Estructura de Carpetas

```
ApiGestionEmpleados/
├── Controllers/          # Controladores de la API
│   ├── AuthController.cs
│   ├── EmpleadosController.cs
│   └── RolesController.cs
├── Data/                 # Contexto de base de datos
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── Modelo/              # Modelos de datos
│   ├── Dtos/            # Data Transfer Objects
│   ├── Empleado.cs
│   ├── Rol.cs
│   └── Usuario.cs
├── Repositorio/         # Patrón Repository
│   ├── IRepositorio/    # Interfaces
│   ├── EmpleadoRepositorio.cs
│   └── RolRepositorio.cs
└── GestionEmpleadosMapper/ # Configuración AutoMapper
```

## Componentes Principales

### 1. Modelos de Datos

#### Empleado
- Información personal (nombre, apellido, email, teléfono)
- Datos laborales (salario, fecha de contratación)
- Relación con Rol (muchos a uno)

#### Rol
- Definición de roles organizacionales
- Descripción y permisos asociados
- Relación con Empleados (uno a muchos)

#### Usuario
- Gestión de autenticación
- Basado en ASP.NET Core Identity

### 2. Patrón Repository

Implementa el patrón Repository para abstraer el acceso a datos:
- IEmpleadoRepositorio / EmpleadoRepositorio
- IRolRepositorio / RolRepositorio

### 3. Autenticación y Autorización

- **JWT Authentication**: Tokens seguros para autenticación
- **ASP.NET Core Identity**: Gestión de usuarios y roles
- **Políticas de Autorización**: Control de acceso granular

## Funcionalidades Principales

### Funcionalidades Avanzadas

#### Gestión de Empleados
- ✅ CRUD completo: Crear, leer, actualizar, eliminar empleados
- ✅ Validación de email único: Previene duplicados
- ✅ Relación con roles: Asignación obligatoria de rol
- ✅ Filtrado por rol: Consulta empleados por rol específico
- ✅ Búsqueda avanzada: Búsqueda por nombre, apellido o email
- ✅ Validaciones de integridad: Verificación de rol existente

#### Gestión de Roles
- ✅ CRUD completo: Crear, leer, actualizar, eliminar roles
- ✅ Validación de nombre único: Previene duplicados
- ✅ Protección referencial: No permite eliminar roles con empleados asignados
- ✅ Datos semilla: Roles predefinidos al inicializar BD
- ✅ Control de estado: Roles activos/inactivos

#### Autenticación
- ✅ Registro de usuarios
- ✅ Inicio de sesión
- ✅ Generación de tokens JWT
- ✅ Validación de tokens

## Configuración de Base de Datos

### Entity Framework Core

El contexto ApplicationDbContext configura:
- **Empleados**: Tabla principal con validaciones y restricciones
- **Roles**: Catálogo de roles con datos semilla
- **Usuarios**: Integración con Identity Framework

### Datos Semilla

El sistema incluye roles predefinidos:
- Administrador
- Gerente
- Desarrollador
- Analista

## Configuración CORS

Configurado para permitir comunicación con frontend React:
- **Origen permitido**: http://localhost:5173
- **Métodos**: Todos los métodos HTTP
- **Headers**: Todos los headers
- **Credenciales**: Habilitadas

## Endpoints Principales

### Autenticación (`/api/auth`)

> **Nota**: Todos los endpoints de autenticación son públicos (no requieren token).

#### POST `/api/auth/register`
Registra un nuevo usuario en el sistema.

**Body**:
```json
{
  "email": "usuario@ejemplo.com",
  "password": "Password123",
  "nombre": "Juan",
  "apellido": "Pérez"
}
```

**Respuesta exitosa**:
```json
{
  "success": true,
  "message": "Usuario registrado exitosamente"
}
```

#### POST `/api/auth/login`
Autentica un usuario y devuelve un token JWT.

**Body**:
```json
{
  "email": "usuario@ejemplo.com",
  "password": "Password123"
}
```

**Respuesta exitosa**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-12-31T23:59:59Z",
  "email": "usuario@ejemplo.com",
  "nombre": "Juan",
  "success": true,
  "message": "Login exitoso"
}
```

### Empleados (`/api/empleados`)

> **Nota**: Todos los endpoints requieren autenticación (token JWT).

#### GET `/api/empleados`
Obtiene la lista completa de empleados.

**Respuesta**:
```json
[
  {
    "id": 1,
    "nombre": "Juan",
    "apellido": "Pérez",
    "email": "juan.perez@empresa.com",
    "telefono": "123-456-7890",
    "salario": 50000.00,
    "fechaContratacion": "2024-01-15T00:00:00Z",
    "rolId": 1,
    "rolNombre": "Desarrollador"
  }
]
```

#### GET `/api/empleados/{id}`
Obtiene un empleado específico por su ID.

#### POST `/api/empleados`
Crea un nuevo empleado.

**Body**:
```json
{
  "nombre": "María",
  "apellido": "González",
  "email": "maria.gonzalez@empresa.com",
  "telefono": "098-765-4321",
  "salario": 60000.00,
  "fechaContratacion": "2024-02-01T00:00:00Z",
  "rolId": 2
}
```

#### PUT `/api/empleados/{id}`
Actualiza un empleado existente.

#### DELETE `/api/empleados/{id}`
Elimina un empleado del sistema.

#### GET `/api/empleados/rol/{rolId}`
Obtiene todos los empleados que pertenecen a un rol específico.

#### GET `/api/empleados/search?term={término}`
Busca empleados por nombre, apellido o email.

**Parámetros**:
- `term` (requerido): Término de búsqueda

### Roles (`/api/roles`)

> **Nota**: Todos los endpoints requieren autenticación (token JWT).

#### GET `/api/roles`
Obtiene la lista completa de roles.

**Respuesta**:
```json
[
  {
    "id": 1,
    "nombre": "Desarrollador",
    "descripcion": "Desarrollo de software",
    "fechaCreacion": "2024-01-01T00:00:00Z",
    "activo": true
  }
]
```

#### GET `/api/roles/{id}`
Obtiene un rol específico por su ID.

#### POST `/api/roles`
Crea un nuevo rol.

**Body**:
```json
{
  "nombre": "Tester",
  "descripcion": "Pruebas de software",
  "activo": true
}
```

#### PUT `/api/roles/{id}`
Actualiza un rol existente.

#### DELETE `/api/roles/{id}`
Elimina un rol del sistema.

> **Nota**: No se puede eliminar un rol que tenga empleados asignados.

## Seguridad

### Autenticación JWT

La API utiliza JSON Web Tokens (JWT) para autenticación:
- **Duración del token**: Configurable en `Jwt:ExpirationInMinutes`
- **Algoritmo**: HMAC-SHA256
- **Claims incluidos**: Email, Nombre, ID de usuario, JTI (JWT ID)

### Autorización

- **Endpoints públicos**: Solo registro y login
- **Endpoints protegidos**: Todos los endpoints de empleados y roles requieren token JWT válido
- **Header requerido**: `Authorization: Bearer <token>`

### Validaciones Implementadas

- **Registro**: Verificación de email único, validación de contraseña
- **Empleados**: Email único, validación de rol existente
- **Roles**: Nombre único, no eliminación si tiene empleados asignados
- **Modelos**: Validación con Data Annotations
- **Tokens**: Validación de firma, expiración y audiencia

### Manejo de Errores

La API maneja errores de forma consistente:
- **400 Bad Request**: Validaciones fallidas, datos duplicados
- **401 Unauthorized**: Token inválido o faltante
- **404 Not Found**: Recurso no encontrado
- **500 Internal Server Error**: Errores del servidor

### Políticas de Contraseña

- **Longitud mínima**: 6 caracteres
- **Requiere**: mayúsculas, minúsculas, números
- **No requiere**: caracteres especiales

## Configuración de Desarrollo

### Requisitos Previos

- .NET 6.0 o superior
- SQL Server / SQL Server Express
- Visual Studio 2022 o VS Code

### Variables de Configuración

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;Trusted_Connection=true;"
  },
  "Jwt": {
    "Key": "clave-secreta-jwt",
    "Issuer": "api-gestion-empleados",
    "Audience": "usuarios-api"
  }
}
```

### Swagger/OpenAPI

La API incluye documentación automática accesible en:
- **Desarrollo**: https://localhost:7163/swagger

### Logging

Sistema de logging configurado para:
- Información de inicio de aplicación
- Configuración de CORS
- Errores y excepciones

## Buenas Prácticas Implementadas

- **Separación de Responsabilidades**: Arquitectura en capas
- **Patrón Repository**: Abstracción de acceso a datos
- **DTOs**: Transferencia de datos controlada
- **Validación**: Validación en múltiples niveles
- **Seguridad**: Autenticación y autorización robusta
- **Documentación**: Swagger para documentación automática
- **CORS**: Configuración específica para frontend
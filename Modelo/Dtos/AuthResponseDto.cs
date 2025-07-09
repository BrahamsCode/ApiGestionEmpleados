namespace ApiGestionEmpleados.Modelo.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
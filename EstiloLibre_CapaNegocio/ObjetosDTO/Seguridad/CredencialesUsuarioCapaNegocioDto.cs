namespace EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad;

public class CredencialesUsuarioCapaNegocioDTO
{
    public required string NombreDeUsuario { get; set; }
    public required string Contraseña { get; set; }
    public bool EstaAutenticado { get; set; }
    public string? NumeroDocumentoIdentificacion { get; set; }
    public string? DireccionCorreoElectronico { get; set; }
}

namespace EstiloLibre_CapaNegocio.ObjetosDTO.Seguridad
{
    public class UsuarioAutenticadoDTO
    {
        public int Id { get; set; }
        public string Login { get; set;}
        public string Nombre { get; set;}
        public string Apellidos { get; set;}
        public List<string> Permisos { get; set; }
        public string? CorreoE { get; set; }
        public string CodigoIdiomaActual { get; set; }
        public int IdiomaActualId { get; set; }
        public int Telefono { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}

namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdUsuariosAdminSaveData
    {
        public class DTOs
        {
            public class UsuarioAdminSaveDataDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string? Contraseña { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string? Apellido2 { get; set; }
                public string CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public DateTime FechaNacimiento { get; set; }
                public int Telefono { get; set; }
                public List<int> PermisosIds { get; set; }
                public string? FotoBase64 { get; set; }

                public UsuarioAdminSaveDataDTO()
                {
                    this.PermisosIds = new List<int>();
                }
            }
        }
    }
}

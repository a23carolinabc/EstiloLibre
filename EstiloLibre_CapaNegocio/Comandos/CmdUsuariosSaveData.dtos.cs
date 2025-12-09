namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdUsuariosSaveData
    {
        public class DTOs
        {
            public class UsuarioSaveDataDTO
            {
                public UsuarioDTO Usuario { get; set; }
                public IEnumerable<int> LstPermisosAsignadosIds { get; set; }
                public IEnumerable<int> LstGruposAsignadosIds { get; set; }
            }
            public class UsuarioDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string? Contraseña { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string? Apellido2 { get; set; }
                public string? CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public bool Activo { get; set; }
            }
        }
    }
}

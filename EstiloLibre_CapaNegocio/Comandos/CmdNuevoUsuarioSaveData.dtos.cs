namespace EstiloLibre_CapaNegocio.Comandos
{
    public partial class CmdNuevoUsuarioSaveData
    {
        public class DTOs
        {
            public class NuevoUsuarioSaveDataDTO
            {
                public string Login { get; set; }
                public string Contraseña { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string? Apellido2 { get; set; }
                public string CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public DateTime FechaNacimiento { get; set; }
                public int Telefono { get; set; }
            }
        }
    }
}

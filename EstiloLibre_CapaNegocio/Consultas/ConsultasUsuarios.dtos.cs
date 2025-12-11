using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuarios
    {
        public class DTOs
        {
            public class UsuarioAddNewDTO
            {
                public IEnumerable<ControlItem> Idiomas { get; set; }
            }

            public class UsuarioShowDataDTO
            {
                public UsuarioDTO Usuario { get; set; }
                public IEnumerable<ControlItem> Idiomas { get; set; }
            }

            public class UsuarioDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string? Apellido2 { get; set; }
                public string CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public bool Publico { get; set; }
                public DateTime FechaNacimiento { get; set; }
                public int Telefono { get; set; }
                public string? FotoBase64 { get; set; }


                public UsuarioDTO() { }
                public UsuarioDTO(Usuario usuario)
                {
                    this.Id = usuario.Id;
                    this.Login = usuario.Login;
                    this.Nombre = usuario.Nombre;
                    this.Apellido1 = usuario.Apellido1;
                    this.Apellido2 = usuario.Apellido2;
                    this.CorreoE = usuario.CorreoE;
                    this.IdiomaId = usuario.IdiomaId;
                    this.Publico = usuario.Publico;
                    this.FechaNacimiento = usuario.FechaNacimiento;
                    this.Telefono = usuario.Telefono;
                }
            }

            public class PermisoAccesoDTO
            {
                public int Id { get; set; }
                public string Codigo { get; set; }
                public string Nombre { get; set; }
                public string Descripcion { get; set; }
                public bool Asignado { get; set; }
            }
        }
    }
}

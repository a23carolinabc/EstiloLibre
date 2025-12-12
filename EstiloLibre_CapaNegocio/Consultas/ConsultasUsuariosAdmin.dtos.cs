using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuariosAdmin
    {
        public class DTOs
        {
            public class UsuarioAdminResumenDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string NombreCompleto { get; set; }
                public string CorreoE { get; set; }
                public List<string> PermisosAsignados { get; set; }
                public string? FotoBase64 { get; set; }

                public UsuarioAdminResumenDTO()
                {
                    this.PermisosAsignados = new List<string>();
                }
            }

            public class UsuarioAdminAddNewDTO
            {
                public IEnumerable<ControlItem> Idiomas { get; set; }
                public IEnumerable<PermisoDTO> PermisosDisponibles { get; set; }
            }

            public class UsuarioAdminShowDataDTO
            {
                public UsuarioAdminDTO Usuario { get; set; }
                public IEnumerable<ControlItem> Idiomas { get; set; }
                public IEnumerable<PermisoDTO> PermisosDisponibles { get; set; }
            }

            public class UsuarioAdminDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string? Apellido2 { get; set; }
                public string CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public DateTime FechaNacimiento { get; set; }
                public int Telefono { get; set; }
                public List<int> PermisosIds { get; set; }
                public string? FotoBase64 { get; set; }

                public UsuarioAdminDTO()
                {
                    this.PermisosIds = new List<int>();
                }

                public UsuarioAdminDTO(Usuario usuario)
                {
                    this.Id = usuario.Id;
                    this.Login = usuario.Login;
                    this.Nombre = usuario.Nombre;
                    this.Apellido1 = usuario.Apellido1;
                    this.Apellido2 = usuario.Apellido2;
                    this.CorreoE = usuario.CorreoE;
                    this.IdiomaId = usuario.IdiomaId;
                    this.FechaNacimiento = usuario.FechaNacimiento;
                    this.Telefono = usuario.Telefono;
                    this.PermisosIds = new List<int>();
                }
            }

            public class PermisoDTO
            {
                public int Id { get; set; }
                public string Codigo { get; set; }
                public string Descripcion { get; set; }
                public bool Asignado { get; set; }

                public PermisoDTO() { }

                public PermisoDTO(Permiso permiso, bool asignado = false)
                {
                    this.Id = permiso.Id;
                    this.Codigo = permiso.Codigo;
                    this.Descripcion = permiso.Descripcion;
                    this.Asignado = asignado;
                }
            }
        }
    }
}

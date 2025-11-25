using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasUsuarios
    {
        public class Dtos
        {
            public class UsuarioAddNewDTO
            {
                public IEnumerable<ControlItem> Idiomas { get; set; }
                public IEnumerable<PermisoAccesoDTO> PermisosAcceso { get; set; }
                public IEnumerable<GrupoAccesoDTO> GruposAcceso { get; set; }
            }

            public class UsuarioShowDataDTO
            {
                public UsuarioDTO Usuario { get; set; }
                public int iUsuarioAnteriorId { get; set; }
                public int iUsuarioSiguienteId { get; set; }
                public IEnumerable<ControlItem> Idiomas { get; set; }
                public IEnumerable<PermisoAccesoDTO> PermisosAcceso { get; set; }
                public IEnumerable<GrupoAccesoDTO> GruposAcceso { get; set; }
            }

            public class UsuarioDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string Nombre { get; set; }
                public string Apellido1 { get; set; }
                public string Apellido2 { get; set; }
                public string CorreoE { get; set; }
                public int IdiomaId { get; set; }
                public bool Activo { get; set; }


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
                    this.Activo = usuario.FechaBaja is null? true:false;
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

            public class GrupoAccesoDTO
            {
                public int Id { get; set; }
                public string Codigo { get; set; }
                public string Descripcion { get; set; }
                public IEnumerable<int> lstPermisosAccesoIds { get; set; }
                public bool Asignado { get; set; }

                public GrupoAccesoDTO() { }
            }
        }
    }
}

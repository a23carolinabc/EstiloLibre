using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;

namespace EstiloLibre_CapaNegocio.Consultas
{
    public partial class ConsultasAdministracion
    {
        public class DTOs
        {
            public class UsuarioNormalResumenDTO
            {
                public int Id { get; set; }
                public string Login { get; set; }
                public string NombreCompleto { get; set; }
                public string CorreoE { get; set; }
                public DateTime FechaNacimiento { get; set; }
                public int CantidadPrendas { get; set; }
                public int CantidadConjuntos { get; set; }
                public bool Publico { get; set; }
                public string? FotoBase64 { get; set; }
            }

            public class UsuarioNormalShowDataDTO
            {
                public UsuarioNormalDTO Usuario { get; set; }
                public IEnumerable<ControlItem> Idiomas { get; set; }
                public IEnumerable<PrendaAdminDTO> Prendas { get; set; }
                public IEnumerable<ConjuntoAdminDTO> Conjuntos { get; set; }
            }

            public class UsuarioNormalDTO
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

                public UsuarioNormalDTO() { }

                public UsuarioNormalDTO(Usuario usuario)
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

            public class PrendaAdminDTO
            {
                public int Id { get; set; }
                public string? CategoriaNombre { get; set; }
                public string? ColorNombre { get; set; }
                public string? MarcaNombre { get; set; }
                public string? ImagenBase64 { get; set; }
            }

            public class ConjuntoAdminDTO
            {
                public int Id { get; set; }
                public string? Descripcion { get; set; }
                public string? EstilonNombre { get; set; }
                public bool EsFavorito { get; set; }
                public int CantidadPrendas { get; set; }
                public string? ImagenBase64 { get; set; }
            }

            public class BusquedaUsuariosDTO
            {
                public string? TextoBusqueda { get; set; }
                public TipoBusquedaUsuario TipoBusqueda { get; set; }
            }

            public enum TipoBusquedaUsuario
            {
                Todos = 0,
                Nombre = 1,
                Login = 2,
                CorreoE = 3
            }
        }
    }
}

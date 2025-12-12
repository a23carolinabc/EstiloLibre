using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosListado : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuarios Usuarios { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosListado(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT u.*
                FROM {TablasBD.Usuarios} u
                WHERE u.Id NOT IN (
                    SELECT DISTINCT pu.UsuarioId 
                    FROM {TablasBD.PermisosUsuarios} pu
                    INNER JOIN {TablasBD.Permisos} p ON pu.PermisoId = p.Id
                    WHERE p.Codigo = '{Codigos.Permisos.ADMIN}'
                )
                AND (@strTextoBusqueda IS NULL OR @strTextoBusqueda = '' OR
                     (@strTipoBusqueda = 'Nombre' AND CONCAT(u.Nombre, ' ', u.Apellido1, ' ', IFNULL(u.Apellido2, '')) LIKE CONCAT('%', @strTextoBusqueda, '%')) OR
                     (@strTipoBusqueda = 'Login' AND u.Login LIKE CONCAT('%', @strTextoBusqueda, '%')) OR
                     (@strTipoBusqueda = 'CorreoE' AND u.CorreoE LIKE CONCAT('%', @strTextoBusqueda, '%')))
                ORDER BY u.Nombre, u.Apellido1;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(string? strTextoBusqueda, string? strTipoBusqueda)
        {
            // Agregar parámetros de búsqueda
            this.AgregarParametro("strTextoBusqueda", strTextoBusqueda, MySqlDbType.VarChar);
            this.AgregarParametro("strTipoBusqueda", strTipoBusqueda ?? "Todos", MySqlDbType.VarChar);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colección de usuarios
            this.Usuarios = new Usuarios(this.MapearLista<Usuario>(TablasBD.Usuarios));
        }

        #endregion
    }
}

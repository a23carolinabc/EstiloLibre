using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosAdminShowData : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }
        public Idiomas Idiomas { get; set; }
        public Permisos PermisosDisponibles { get; set; }
        public Permisos PermisosAsignados { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosAdminShowData(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Usuarios}
                WHERE Id = @iUsuarioId;

                SELECT *
                FROM {TablasBD.Idiomas};

                SELECT *
                FROM {TablasBD.Permisos}
                WHERE Codigo IN ('{Codigos.Permisos.ADMIN}', '{Codigos.Permisos.ADMINPLUS}');

                SELECT p.* 
                FROM {TablasBD.Permisos} p 
                INNER JOIN {TablasBD.PermisosUsuarios} pu ON p.Id = pu.PermisoId 
                WHERE pu.UsuarioId = @iUsuarioId;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios, TablasBD.Idiomas, TablasBD.Permisos, "PermisosAsignados" };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear objetos y colecciones
            this.Usuario = this.MapearObjeto<Usuario>(TablasBD.Usuarios) ?? new();
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));
            this.PermisosDisponibles = new Permisos(this.MapearLista<Permiso>(TablasBD.Permisos));
            this.PermisosAsignados = new Permisos(this.MapearLista<Permiso>("PermisosAsignados"));
        }

        #endregion
    }
}

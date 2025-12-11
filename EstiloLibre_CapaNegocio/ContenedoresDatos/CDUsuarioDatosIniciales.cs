using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuarioDatosIniciales : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuarioDatosIniciales(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Usuarios} p
                WHERE p.Id = @iUsuarioId;

                SELECT p.* 
                FROM {TablasBD.Permisos} p 
                INNER JOIN {TablasBD.PermisosUsuarios} pu ON p.Id = pu.PermisoId AND pu.UsuarioId = @iUsuarioId;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios, TablasBD.Permisos };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            DataTable tabla;
            List<string> permisos;

            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            this.Usuario = new Usuario();
            this.Usuario.IniciarListaPermisos();

            // Asignar datos del usuario
            this.Usuario = this.MapearObjeto<Usuario>(TablasBD.Usuarios)??new();

            // Asignar permisos
            if (this.TablaTieneDatos(TablasBD.Permisos))
            {
                tabla = this.GetTabla(TablasBD.Permisos)!;

                permisos = new List<string>();

                foreach (DataRow fila in tabla.Rows)
                {
                    permisos.Add(UtilsConversion.GetString(fila["Codigo"])!);
                }

                this.Usuario.Permisos = permisos;
            }            
        }

        #endregion
    }
}

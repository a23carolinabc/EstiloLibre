using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosAdminAddNew : Vista
    {
        #region ***** PROPIEDADES *****

        public Idiomas Idiomas { get; set; }
        public Permisos Permisos { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosAdminAddNew(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Idiomas};

                SELECT *
                FROM {TablasBD.Permisos}
                WHERE Codigo IN ('{Codigos.Permisos.ADMIN}', '{Codigos.Permisos.ADMINPLUS}');
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Idiomas, TablasBD.Permisos };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar()
        {
            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));
            this.Permisos = new Permisos(this.MapearLista<Permiso>(TablasBD.Permisos));
        }

        #endregion
    }
}

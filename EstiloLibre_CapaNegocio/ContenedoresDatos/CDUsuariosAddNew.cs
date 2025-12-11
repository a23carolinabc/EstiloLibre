using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosAddNew : Vista
    {
        #region ***** PROPIEDADES *****

        public Idiomas Idiomas { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosAddNew(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"

                SELECT *
                FROM {TablasBD.Idiomas};
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Idiomas};
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar()
        {
            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));
        }

        #endregion
    }
}

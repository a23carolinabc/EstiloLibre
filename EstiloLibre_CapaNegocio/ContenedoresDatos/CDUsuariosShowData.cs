using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuariosShowData : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }
        public Idiomas Idiomas { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuariosShowData(Conexion conexion) : base(conexion) { }

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
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios, TablasBD.Idiomas};
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Usuario = this.MapearObjeto<Usuario>(TablasBD.Usuarios)??new();
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));
        }

        #endregion
    }
}

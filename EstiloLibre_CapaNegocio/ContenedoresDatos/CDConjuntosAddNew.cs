using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDConjuntosAddNew : Vista
    {
        #region ***** PROPIEDADES *****

        public Estaciones Estaciones { get; set; }
        public Estilos Estilos { get; set; }
        public Colores Colores { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDConjuntosAddNew(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Estaciones};

                SELECT *
                FROM {TablasBD.Estilos};

                SELECT *
                FROM {TablasBD.Colores};
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] {
                TablasBD.Estaciones,
                TablasBD.Estilos,
                TablasBD.Colores
            };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar()
        {
            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Estaciones = new Estaciones(this.MapearLista<Estacion>(TablasBD.Estaciones));
            this.Estilos = new Estilos(this.MapearLista<Estilo>(TablasBD.Estilos));
            this.Colores = new Colores(this.MapearLista<Color>(TablasBD.Colores));
        }

        #endregion
    }
}
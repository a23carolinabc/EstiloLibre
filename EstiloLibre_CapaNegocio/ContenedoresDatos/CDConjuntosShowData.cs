using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDConjuntosShowData : Vista
    {
        #region ***** PROPIEDADES *****

        public Conjunto Conjunto { get; set; }
        public Estaciones Estaciones { get; set; }
        public Estilos Estilos { get; set; }
        public Colores Colores { get; set; }
        public PrendasConjuntos PrendasConjuntos { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDConjuntosShowData(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Conjuntos}
                WHERE Id = @iConjuntoId;

                SELECT *
                FROM {TablasBD.Estaciones};

                SELECT *
                FROM {TablasBD.Estilos};

                SELECT *
                FROM {TablasBD.Colores};

                SELECT *
                FROM {TablasBD.PrendasConjuntos}
                WHERE ConjuntoId = @iConjuntoId;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] {
                TablasBD.Conjuntos,
                TablasBD.Estaciones,
                TablasBD.Estilos,
                TablasBD.Colores,
                TablasBD.PrendasConjuntos
            };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iConjuntoId)
        {
            this.AgregarParametro("iConjuntoId", iConjuntoId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear objetos
            this.Conjunto = this.MapearObjeto<Conjunto>(TablasBD.Conjuntos) ?? new();
            this.Estaciones = new Estaciones(this.MapearLista<Estacion>(TablasBD.Estaciones));
            this.Estilos = new Estilos(this.MapearLista<Estilo>(TablasBD.Estilos));
            this.Colores = new Colores(this.MapearLista<Color>(TablasBD.Colores));
            this.PrendasConjuntos = new PrendasConjuntos(this.MapearLista<PrendaConjunto>(TablasBD.PrendasConjuntos));
        }

        #endregion
    }
}
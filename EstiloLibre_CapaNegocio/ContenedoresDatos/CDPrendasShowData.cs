using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using MySqlConnector;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDPrendasShowData : Vista
    {
        #region ***** PROPIEDADES *****

        public Prenda Prenda { get; set; }
        public Colores Colores { get; set; }
        public Categorias Categorias { get; set; }
        public Estados Estados { get; set; }
        public Tallas Tallas { get; set; }
        public Materiales Materiales { get; set; }
        public Marcas Marcas { get; set; }
        public Estaciones Estaciones { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDPrendasShowData(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"
                SELECT *
                FROM {TablasBD.Prendas}
                WHERE Id = @iPrendaId;

                SELECT *
                FROM {TablasBD.Estaciones};

                SELECT *
                FROM {TablasBD.Marcas};

                SELECT *
                FROM {TablasBD.Materiales};

                SELECT *
                FROM {TablasBD.Tallas};

                SELECT *
                FROM {TablasBD.Estados};

                SELECT *
                FROM {TablasBD.Categorias};
    
                SELECT *
                FROM {TablasBD.Colores};
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Prendas, TablasBD.Estaciones, TablasBD.Marcas,
                                     TablasBD.Materiales, TablasBD.Tallas,
                                     TablasBD.Estados, TablasBD.Categorias,
                                     TablasBD.Colores, };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iPrendaId)
        {
            this.AgregarParametro("iPrendaId", iPrendaId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Prenda = this.MapearObjeto<Prenda>(TablasBD.Prendas)??new();
            this.Estaciones = new Estaciones(this.MapearLista<Estacion>(TablasBD.Estaciones));
            this.Marcas = new Marcas(this.MapearLista<Marca>(TablasBD.Marcas));
            this.Materiales = new Materiales(this.MapearLista<Material>(TablasBD.Materiales));
            this.Tallas = new Tallas(this.MapearLista<Talla>(TablasBD.Tallas));
            this.Categorias = new Categorias(this.MapearLista<Categoria>(TablasBD.Categorias));
            this.Colores = new Colores(this.MapearLista<Color>(TablasBD.Colores));
            this.Estados = new Estados(this.MapearLista<Estado>(TablasBD.Estados));
        }

        #endregion
    }
}

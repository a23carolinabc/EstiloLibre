using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDPrendasAddNew : Vista
    {
        #region ***** PROPIEDADES *****

        public Colores Colores { get; set; }
        public Categorias Categorias { get; set; }
        public Estados Estados { get; set; }
        public Tallas Tallas { get; set; }
        public Materiales Materiales { get; set; }
        public Marcas Marcas { get; set; }
        public Estaciones Estaciones { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDPrendasAddNew(Conexion conexion) : base(conexion) { }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****

        protected override string DefinirConsultaSql()
        {
            return @$"

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
            return new string[] { TablasBD.Estaciones, TablasBD.Marcas,
                                     TablasBD.Materiales, TablasBD.Tallas,
                                     TablasBD.Estados, TablasBD.Categorias,
                                     TablasBD.Colores, };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public override void Cargar()
        {
            DataSet datos;

            // Ejecutar consulta
            datos = this.EjecutarConsulta();

            // Mapear colecciones automáticamente
            this.Estaciones = (Estaciones)this.MapearLista<Estacion>(TablasBD.Estaciones);
            this.Marcas = (Marcas)this.MapearLista<Marca>(TablasBD.Marcas);
            this.Materiales = (Materiales)this.MapearLista<Material>(TablasBD.Materiales);
            this.Tallas = (Tallas)this.MapearLista<Talla>(TablasBD.Tallas);
            this.Categorias = (Categorias)this.MapearLista<Categoria>(TablasBD.Categorias);
            this.Colores = (Colores)this.MapearLista<Color>(TablasBD.Colores);
            this.Estados = (Estados)this.MapearLista<Estado>(TablasBD.Estados);
        }

        #endregion
    }
}

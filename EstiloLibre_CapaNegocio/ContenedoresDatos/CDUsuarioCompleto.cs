using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;
using MySqlConnector;

namespace EstiloLibre_CapaNegocio.ContenedoresDatos
{
    public class CDUsuarioCompleto : Vista
    {
        #region ***** PROPIEDADES *****

        public Usuario Usuario { get; set; }
        public Idiomas Idiomas { get; set; }
        public Prendas Prendas { get; set; }
        public Conjuntos Conjuntos { get; set; }
        public DataTable TablaPrendas { get; set; }
        public DataTable TablaConjuntos { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public CDUsuarioCompleto(Conexion conexion) : base(conexion) { }

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

                SELECT p.*, 
                       c.Nombre as CategoriaNombre, 
                       col.Nombre as ColorNombre, 
                       m.Nombre as MarcaNombre
                FROM {TablasBD.Prendas} p
                LEFT JOIN {TablasBD.Categorias} c ON p.CategoriaId = c.Id
                LEFT JOIN {TablasBD.Colores} col ON p.ColorId = col.Id
                LEFT JOIN {TablasBD.Marcas} m ON p.MarcaId = m.Id
                WHERE p.UsuarioId = @iUsuarioId
                ORDER BY p.Id DESC;

                SELECT co.*, 
                       e.Nombre as EstiloNombre,
                       (SELECT COUNT(*) FROM {TablasBD.PrendasConjuntos} pc WHERE pc.ConjuntoId = co.Id) as CantidadPrendas
                FROM {TablasBD.Conjuntos} co
                LEFT JOIN {TablasBD.Estilos} e ON co.EstiloId = e.Id
                WHERE co.UsuarioId = @iUsuarioId
                ORDER BY co.Id DESC;
            ";
        }

        protected override string[] DefinirNombresTablas()
        {
            return new string[] { TablasBD.Usuarios, TablasBD.Idiomas, "PrendasConInfo", "ConjuntosConInfo" };
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public void Cargar(int iUsuarioId)
        {
            this.AgregarParametro("iUsuarioId", iUsuarioId, MySqlDbType.Int32);

            // Ejecutar consulta
            this.EjecutarConsulta();

            // Mapear objetos y colecciones básicas
            this.Usuario = this.MapearObjeto<Usuario>(TablasBD.Usuarios) ?? new();
            this.Idiomas = new Idiomas(this.MapearLista<Idioma>(TablasBD.Idiomas));

            // Mapear prendas básicas para la colección
            this.Prendas = new Prendas(this.MapearLista<Prenda>("PrendasConInfo"));

            // Mapear conjuntos básicos para la colección
            this.Conjuntos = new Conjuntos(this.MapearLista<Conjunto>("ConjuntosConInfo"));

            // Guardar las tablas con información adicional para procesamiento en consultas
            if (this.TablaTieneDatos("PrendasConInfo"))
            {
                this.TablaPrendas = this.GetTabla("PrendasConInfo")!;
            }
            else
            {
                this.TablaPrendas = new DataTable();
            }

            if (this.TablaTieneDatos("ConjuntosConInfo"))
            {
                this.TablaConjuntos = this.GetTabla("ConjuntosConInfo")!;
            }
            else
            {
                this.TablaConjuntos = new DataTable();
            }
        }

        #endregion
    }
}

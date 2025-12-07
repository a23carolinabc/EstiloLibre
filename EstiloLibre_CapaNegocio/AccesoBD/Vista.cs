using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Utils;
using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public abstract class Vista
    {
        #region ***** PROPIEDADES INTERNAS *****

        protected Conexion Conexion { get; set; }
        private string _consultaSql;
        private string[] _nombresTablas;
        private List<ParametroBD> _parametros;
        protected DataSet _datos;

        #endregion

        #region ***** CONSTRUCTORES *****

        public Vista(Conexion conexion)
        {
            this.Conexion = conexion;
            this._parametros = new List<ParametroBD>();
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public virtual void Cargar() { }

        protected abstract string DefinirConsultaSql();

        protected abstract string[] DefinirNombresTablas();

        protected void AgregarParametro(string nombreParametro, object valor)
        {
            ParametroBD? parametro;

            // Asegurar que el nombre del parámetro tenga el prefijo @
            if (!nombreParametro.StartsWith("@"))
            {
                nombreParametro = "@" + nombreParametro;
            }

            // Buscar si ya existe el parámetro y actualizarlo
            parametro = this._parametros.FirstOrDefault(p => p.ParameterName == nombreParametro);
            if (parametro != null)
            {
                parametro.Value = valor ?? DBNull.Value;
            }
            else
            {
                parametro = new ParametroBD(nombreParametro, valor);
                this._parametros.Add(parametro);
            }
        }

        protected void AgregarParametro(string nombreParametro, object valor, MySqlDbType tipo)
        {
            ParametroBD? parametro;

            // Asegurar que el nombre del parámetro tenga el prefijo @
            if (!nombreParametro.StartsWith("@"))
            {
                nombreParametro = "@" + nombreParametro;
            }

            // Buscar si ya existe el parámetro y actualizarlo
            parametro = this._parametros.FirstOrDefault(p => p.ParameterName == nombreParametro);
            if (parametro != null)
            {
                parametro.Value = valor ?? DBNull.Value;
                parametro.DbType = (DbType)tipo;
            }
            else
            {
                // Crear nuevo parámetro usando tu clase ParametroBD con tipo específico
                parametro = new ParametroBD(nombreParametro, valor, tipo);
                this._parametros.Add(parametro);
            }
        }

        protected void LimpiarParametros()
        {
            this._parametros.Clear();
        }

        protected DataSet EjecutarConsulta()
        {
            ParametroBD[] arrayParametros;
            DbCommand comando;
            DataSet dataSet;

            // Obtener la consulta SQL y los nombres de las tablas
            this._consultaSql = this.DefinirConsultaSql();
            this._nombresTablas = this.DefinirNombresTablas();

            if (string.IsNullOrWhiteSpace(this._consultaSql))
            {
                throw new CapaNegocioException("La consulta SQL no puede estar vacía.");
            }

            if (this._nombresTablas == null || this._nombresTablas.Length == 0)
            {
                throw new CapaNegocioException("Debe definir al menos un nombre de tabla.");
            }

            // Crear el comando SQL con los parámetros
            arrayParametros = this._parametros.Count > 0 ? this._parametros.ToArray() : null;
            comando = this.Conexion.ConexionBD.CrearComando(this._consultaSql, arrayParametros, CommandType.Text);

            // Ejecutar la consulta y obtener el DataSet
            dataSet = this.Conexion.ConexionBD.GetConjuntoDatos(comando, this._nombresTablas);

            // Guardar el dataset para las comprobaciones
            this._datos = dataSet;

            return dataSet;
        }


        protected bool DataSetTieneDatos()
        {
            if (this._datos == null)
            {
                return false;
            }

            if (this._datos.Tables.Count == 0)
            {
                return false;
            }
            
            return true;
        }

        protected bool TablaTieneDatos(string nombreTabla)
        {
            DataTable tabla;

            if (!this.DataSetTieneDatos())
            {
                return false;
            }
            
            if (!this._datos.Tables.Contains(nombreTabla))
            {
                return false;
            }

            tabla = this._datos.Tables[nombreTabla]!;

            return tabla.Rows.Count > 0;
        }

        protected DataTable? GetTabla(string nombreTabla)
        {
            if (!this.TablaTieneDatos(nombreTabla))
            {
                return null;
            }

            return this._datos.Tables[nombreTabla];
        }

        protected T? MapearObjeto<T>(string nombreTabla) where T : class
        {
            DataTable? tabla;

            tabla = this.GetTabla(nombreTabla);
            if(tabla == null)
            {
                return null;
            }

            return tabla!.MapearAObjeto<T>();
        }

        protected ListaObjetosBD<T> MapearLista<T>(string nombreTabla) where T : ObjetoBD, new()
        {
            DataTable? tabla;
            IEnumerable<T> lista;

            if (!this.TablaTieneDatos(nombreTabla))
            {
                return new ListaObjetosBD<T>();
            }

            tabla = this.GetTabla(nombreTabla);
            lista = tabla!.MapearALista<T>();

            return new ListaObjetosBD<T>(lista);
        }
        #endregion
    }
}

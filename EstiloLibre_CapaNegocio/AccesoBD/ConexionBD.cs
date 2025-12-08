using EstiloLibre_CapaNegocio.Excepciones;
using MySqlConnector;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public class ConexionBD : IDisposable
    {
        #region ***** PROPIEDADES *****
        private Configuracion Configuracion { get; set; }

        private DbConnection DbConexion { get; set; }

        private DbTransaction? Transaccion { get; set; }

        private bool _disposed { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****
        public ConexionBD(Configuracion cfg)
        {
            this._disposed = false;
            this.Configuracion = cfg;
        }

        public ConexionBD(ConexionBD oConexionBD)
        {
            this._disposed = false;
            this.Configuracion = oConexionBD.Configuracion;
        }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        internal IDbConnection GetConexion()
        {
            this.Conectar();
            return this.DbConexion;
        }

        /// <summary>
        /// Obtiene la transacción activa (si existe).
        /// </summary>
        /// <returns>La transacción activa o null si no hay ninguna.</returns>
        internal DbTransaction? GetTransaccion()
        {
            return this.Transaccion;
        }

        internal void PrepararComando(DbCommand comandoSql)
        {
            this.Conectar();

            comandoSql.Connection = this.DbConexion;
            if (this.Transaccion != null)
            {
                comandoSql.Transaction = this.Transaccion;
            }
            if (comandoSql.CommandTimeout <= 0 && this.Configuracion.iSqlQueryTimeOut > 0)
            {
                comandoSql.CommandTimeout = this.Configuracion.iSqlQueryTimeOut;
            }
        }

        internal DbCommand CrearComando(string strSql, ParametroBD[] parametros, CommandType tipoComando = CommandType.Text)
        {
            DbCommand comando;
            MySqlParameter[] parametrosMySql;

            comando = this.CrearComando(strSql, tipoComando);

            if (parametros != null && parametros.Length > 0)
            {
                parametrosMySql = parametros.Select(p => p.ParametroInterno).ToArray();
                comando.Parameters.AddRange(parametrosMySql);
            }

            return comando;
        }

        internal DbCommand CrearComando(string strSql, CommandType tipoComando = CommandType.Text)
        {
            DbCommand comando;

            this.Conectar();
            comando = this.DbConexion.CreateCommand();
            comando.CommandText = strSql;
            comando.CommandType = tipoComando;
            comando.CommandTimeout = this.Configuracion.iSqlQueryTimeOut;
            if (this.Transaccion != null)
            {
                comando.Transaction = this.Transaccion;
            }
            return comando;
        }

        internal DbDataReader CrearSqlDataReader(DbCommand comandoSql)
        {
            this.PrepararComando(comandoSql);

            return comandoSql.ExecuteReader();
        }

        internal object? EjecutarComandoSql(DbCommand comandoSql)
        {
            object? valorObjeto;

            this.PrepararComando(comandoSql);

            valorObjeto = RuntimeHelpers.GetObjectValue(comandoSql.ExecuteScalar());

            return valorObjeto == DBNull.Value ? null : valorObjeto;
        }

        public DataTable GetTablaDatos(DbCommand comandoSql)
        {
            return this.GetTablaDatos(comandoSql, string.Empty);
        }

        public DataTable GetTablaDatos(DbCommand comandoSql, string nombreTabla)
        {
            DataTable dataTable;
            DbDataReader dbDataReader;

            dataTable = new DataTable(nombreTabla);
            dbDataReader = this.CrearSqlDataReader(comandoSql);
            try
            {
                dataTable.Load(dbDataReader);
                return dataTable;
            }
            finally
            {
                dbDataReader.Close();
            }
        }

        public DataSet GetConjuntoDatos(DbCommand comandoSql, string[] nombresTablas)
        {
            DbDataReader dbDataReader;
            DataSet dataSet;

            dbDataReader = this.CrearSqlDataReader(comandoSql);
            dataSet = new DataSet();
            try
            {
                dataSet.Load(dbDataReader, LoadOption.Upsert, nombresTablas);
                return dataSet;
            }
            finally
            {
                dbDataReader.Close();
            }
        }

        public void Conectar()
        {
            if (this.DbConexion == null)
            {
                this.DbConexion = new MySqlConnection(this.Configuracion.strCadenaConexion);
            }
            if (this.DbConexion.State == ConnectionState.Closed)
            {
                this.DbConexion.Open();
            }
        }

        public void Desconectar()
        {
            this.DbConexion.Close();
        }

        public void BeginTrans(bool bContinuar = false)
        {
            this.Conectar();

            if (this.Transaccion != null)
            {
                if (!bContinuar)
                {
                    throw new CapaNegocioException("Ya hay una transacción abierta");
                }
                return;
            }
            this.Transaccion = this.DbConexion.BeginTransaction();
        }

        public void CommitTrans()
        {
            if (this.Transaccion == null)
            {
                return;
            }
            this.Transaccion.Commit();
            this.Transaccion.Dispose();
            this.Transaccion = null;
        }

        public void RollBackTrans()
        {
            if (this.Transaccion == null)
            {
                return;
            }
            this.Transaccion.Rollback();
            this.Transaccion.Dispose();
            this.Transaccion = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                this.DbConexion.Close();
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ***** MÉTODOS PRIVADOS *****
        void IDisposable.Dispose()
        {
            this.Dispose();
        }
        #endregion
    }
}
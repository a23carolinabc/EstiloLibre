using Dapper;
using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.Base
{
    public abstract class DAO<T> : IDAO where T : ObjetoBD, new()
    {
        #region ***** PROPIEDADES *****

        public Conexion Conexion { get; set; }
        protected string NombreTabla { get; set; }
        protected string NombreTablaTraducciones { get; set; }
        protected string NombreColumnaEnlaceTraducciones { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public DAO(Conexion conexion, string strNombreTabla)
        {
            this.Conexion = conexion;
            this.NombreTabla = strNombreTabla;
        }

        public DAO(Conexion conexion, string strNombreTabla, string strNombreTablaTraducciones, string strNombreColumnaEnlaceTraducciones)
        {
            this.Conexion = conexion;
            this.NombreTabla = strNombreTabla;
            this.NombreTablaTraducciones = strNombreTablaTraducciones;
            this.NombreColumnaEnlaceTraducciones = strNombreColumnaEnlaceTraducciones;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public abstract ObjetoBD CrearObjetoBD();

        public ObjetoBD? CargarObjetoBD(int iId)
        {
            T? objetoBD;
            IDbConnection conexion;
            DbTransaction? transaccion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            if (iId <= 0) return null;

            objetoBD = conexion.Get<T>(iId);
            if (objetoBD != null)
            {
                objetoBD.DAO = this;
            }

            return objetoBD;
        }

        public ObjetoBD? CargarObjetoBD(string clausulaWhere, string? orderBy = null)
        {
            T? objetoBD;
            string strSql;
            IDbConnection conexion;
            DbTransaction? transaccion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            strSql = $"SELECT * FROM {this.NombreTabla} WHERE {clausulaWhere}";
            if (!string.IsNullOrEmpty(orderBy))
            {
                strSql += $" ORDER BY {orderBy}";
            }

            objetoBD = conexion.QueryFirstOrDefault<T>(strSql);
            if (objetoBD != null)
            {
                objetoBD.DAO = this;
            }

            return objetoBD;
        }

        public void GuardarObjetoBD(ObjetoBD objeto)
        {
            IDbConnection conexion;
            DbTransaction? transaccion;
            T obj;
            long nuevoId;
            bool actualizado;

            obj = (T)objeto;
            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            if (obj.Id <= 0)
            {
                // Insertar nuevo registro pasando la transacción activa
                nuevoId = conexion.Insert<T>(obj, transaccion);

                if (nuevoId <= 0)
                {
                    throw new CapaNegocioException($"No se pudo crear el objeto");
                }

                // Actualizar id del objeto usando reflexión
                var propertyInfo = typeof(T).BaseType?.GetProperty("Id",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                propertyInfo?.SetValue(obj, Convert.ToInt32(nuevoId));
            }
            else
            {
                // Actualizar registro existente pasando la transacción activa
                actualizado = conexion.Update<T>(obj, transaccion);
                if (!actualizado)
                {
                    throw new CapaNegocioException($"No se pudo actualizar el objeto con Id {obj.Id}");
                }
            }
        }

        public void EliminarObjetoBD(ObjetoBD objeto)
        {
            IDbConnection conexion;
            DbTransaction? transaccion;
            bool eliminado;
            T obj;

            obj = (T)objeto;

            if (obj.Id <= 0)
            {
                throw new CapaNegocioException("No se puede eliminar un objeto sin Id asociado");
            }

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            // Eliminar registro pasando la transacción activa
            eliminado = conexion.Delete<T>(obj, transaccion);
            if (!eliminado)
            {
                throw new CapaNegocioException($"No se pudo eliminar el objeto con Id {obj.Id}");
            }
        }

        public ListaObjetosBD<T> CargarTodos()
        {
            ListaObjetosBD<T> lista;
            List<T> resultado;
            IDbConnection conexion;
            DbTransaction? transaccion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            resultado = conexion.GetAll<T>().ToList();
            resultado.ForEach(o => o.DAO = this);

            lista = new(resultado);
            return lista;
        }

        public ListaObjetosBD<T> CargarObjetosBD(List<int> ids)
        {
            ListaObjetosBD<T> lista;            
            List<T> resultado;
            string strSql;
            string idsString; 
            IDbConnection conexion;
            DbTransaction? transaccion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            if (ids == null || ids.Count == 0)
            {
                return new ListaObjetosBD<T>();
            }

            // Filtrar IDs válidos
            ids = ids.Where(id => id > 0).ToList();
            if (ids.Count == 0)
            {
                return new ListaObjetosBD<T>();
            }

            idsString = string.Join(",", ids);
            strSql = $"SELECT * FROM {this.NombreTabla} WHERE Id IN ({idsString})";

            resultado = conexion.Query<T>(strSql).ToList();

            foreach (var obj in resultado)
            {
                obj.DAO = this;
            }

            lista = new ListaObjetosBD<T>(resultado);
            return lista;
        }

        public ListaObjetosBD<T> CargarObjetosBD(string clausulaWhere, string? orderBy = null)
        {
            List<T> resultado;
            string strSql;
            ListaObjetosBD<T> lista; 
            IDbConnection conexion;
            DbTransaction? transaccion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            if (string.IsNullOrEmpty(clausulaWhere))
            {
                return new ListaObjetosBD<T>();
            }

            strSql = $"SELECT * FROM {this.NombreTabla} WHERE {clausulaWhere}";
            if (!string.IsNullOrEmpty(orderBy))
            {
                strSql += $" ORDER BY {orderBy}";
            }

            resultado = conexion.Query<T>(strSql).ToList();
            resultado.ForEach(o => o.DAO = this);

            lista = new ListaObjetosBD<T>(resultado);
            return lista;
        }

        public void GuardarObjetosBD(List<ObjetoBD> objetos)
        {
            IDbConnection conexion;
            DbTransaction? transaccion;
            T obj;
            long nuevoId;
            bool actualizado;

            if (objetos == null || objetos.Count == 0)
            {
                return;
            }

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            foreach (ObjetoBD objeto in objetos)
            {
                obj = (T)objeto;

                if (obj.Id <= 0)
                {
                    // Insertar nuevo objeto pasando la transacción activa
                    nuevoId = conexion.Insert<T>(obj, transaccion);

                    if (nuevoId <= 0)
                    {
                        throw new CapaNegocioException($"No se pudo crear el objeto");
                    }

                    // Actualizar id del objeto usando reflexión
                    var propertyInfo = typeof(T).BaseType?.GetProperty("Id",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    propertyInfo?.SetValue(obj, Convert.ToInt32(nuevoId));
                }
                else
                {
                    // Actualizar objeto existente pasando la transacción activa
                    actualizado = conexion.Update<T>(obj, transaccion);
                    if (!actualizado)
                    {
                        throw new CapaNegocioException($"No se pudo actualizar el objeto con Id {obj.Id}");
                    }
                }
            }
        }

        public void EliminarObjetosBD(List<ObjetoBD> objetos)
        {
            IDbConnection conexion;
            DbTransaction? transaccion;
            T obj;
            bool eliminado;

            if (objetos == null || objetos.Count == 0)
            {
                return;
            }

            conexion = this.Conexion.ConexionBD.GetConexion();
            transaccion = this.Conexion.ConexionBD.GetTransaccion();

            foreach (ObjetoBD objeto in objetos)
            {
                obj = (T)objeto;

                if (obj.Id <= 0)
                {
                    throw new CapaNegocioException("No se puede eliminar un objeto sin Id asociado");
                }

                // Eliminar objeto pasando la transacción activa
                eliminado = conexion.Delete<T>(obj, transaccion);
                if (!eliminado)
                {
                    throw new CapaNegocioException($"No se pudo eliminar el objeto con Id {obj.Id}");
                }
            }
        }

        #endregion
    }
}
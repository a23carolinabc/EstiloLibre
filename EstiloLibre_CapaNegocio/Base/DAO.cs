using Dapper;
using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.Base
{
    public abstract class DAO<T>: IDAO where T : ObjetoBD, new()
    {
        #region ***** PROPIEDADES INTERNAS *****
        #endregion

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
            this.NombreTabla = strNombreTabla;
            this.NombreTablaTraducciones = strNombreTablaTraducciones;
            this.NombreColumnaEnlaceTraducciones = strNombreColumnaEnlaceTraducciones;
        }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public abstract ObjetoBD CrearObjetoBD();


        //public virtual ObjetoBD CrearObjetoBDTraduccion()
        //{
        //    return null;
        //}

        //protected ObjetoTraduccionDAO GetDAOObjetoTraduccion()
        //{
        //    return new ObjetoTraduccionDAO(this.Conexion, this.NombreTablaTraducciones, this.NombreColumnaEnlaceTraducciones);
        //}

        //protected virtual int GetIdiomaId()
        //{
        //    return this.Conexion.IdiomaActualId;
        //}

        public ObjetoBD? CargarObjetoBD(int iId)
        {
            T? objetoBD;
            IDbConnection conexion;

            if (iId <= 0) return null;

            conexion = this.Conexion.ConexionBD.GetConexion();
            objetoBD = conexion.Get<T>(iId);
            if (objetoBD != null)
            {
                objetoBD.DAO = this;
            }

            return objetoBD;
        }

        public ObjetoBD? CargarObjetoBD(string clausulaWhere, string? orderBy = null)
        {
            IDbConnection conexion;
            T? objetoBD;
            string strSql;

            strSql = $"SELECT * FROM {NombreTabla} WHERE {clausulaWhere}";            
            if (!string.IsNullOrEmpty(orderBy))
            {
                strSql = $" ORDER BY {orderBy}";
            }

            conexion = this.Conexion.ConexionBD.GetConexion();
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
            T obj = (T)objeto;

            conexion = this.Conexion.ConexionBD.GetConexion();
            if (obj.Id <= 0)
            {
                // Recoger id generado
                long nuevoId = conexion.Insert<T>(obj);

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
                bool actualizado = conexion.Update<T>(obj);
                if (!actualizado)
                {
                    throw new CapaNegocioException($"No se pudo actualizar el objeto con Id {obj.Id}");
                }
            }
        }

        public void EliminarObjetoBD(ObjetoBD objeto)
        {
            IDbConnection conexion;
            bool eliminado;

            T obj = (T)objeto;

            if (obj.Id <= 0)
            {
                throw new CapaNegocioException("No se puede eliminar un objeto sin Id asociado");
            }

            conexion = this.Conexion.ConexionBD.GetConexion();
            eliminado = conexion.Delete<T>(obj);
            if (!eliminado)
            {
                throw new CapaNegocioException($"No se pudo eliminar el objeto con Id {obj.Id}");
            }
        }

        public ListaObjetosBD<T> CargarTodos()
        {
            ListaObjetosBD<T> lista;
            IDbConnection conexion;

            conexion = this.Conexion.ConexionBD.GetConexion();
            var resultado = conexion.GetAll<T>().ToList();
            foreach (var obj in resultado)
            {
                obj.DAO = this;
            }
            lista = new(resultado);
            return lista;
        }
        #endregion

        #region ***** MÉTODOS PRIVADOS *****        
        #endregion
    }
}

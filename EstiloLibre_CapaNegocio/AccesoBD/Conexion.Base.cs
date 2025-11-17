using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using System.Runtime.Caching;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public partial class Conexion
    {
        #region  Idiomas 

        internal IdiomasDAO GetDAOIdiomas()
        {
            return new IdiomasDAO(this);
        }

        internal Idioma CrearIdioma()
        {
            return (Idioma)this.GetDAOIdiomas().CrearObjetoBD();
        }

        internal Idioma CargarIdioma(string strCodigo)
        {
            Predicate<Idioma> condicion;

            condicion = new Predicate<Idioma>(objeto => objeto.Codigo == strCodigo);

            return new();
        }

        //internal Idiomas CargarIdiomas()
        //{
        //    lock (_objetoDeBloqueo)
        //    {
        //        if (!MemoryCache.Default.Contains("Idiomas"))
        //        {
        //            MemoryCache.Default.Add("Idiomas", this.GetDAOIdiomas().CargarIdiomas(), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(1) });
        //        }
        //    }
        //    return (Idiomas)MemoryCache.Default["Idiomas"];
        //}

        #endregion

        #region Usuarios

        public UsuariosDAO GetDAOUsuarios()
        {
            return new UsuariosDAO(this);
        }

        public Usuario CrearUsuario()
        {
            return (Usuario)this.GetDAOUsuarios().CrearObjetoBD();
        }

        public Usuario? CargarUsuario(int iUsuarioId)
        {
            return this.GetDAOUsuarios().CargarUsuario(iUsuarioId);
        }

        public Usuario? CargarUsuario(string strLogin)
        {
            return this.GetDAOUsuarios().CargarUsuarioPorLogin(strLogin);
        }

        public Usuario? CargarUsuarioActual()
        {
            return this.CargarUsuario(this.UsuarioAutenticado.Id);
        }

        #endregion
    }
}
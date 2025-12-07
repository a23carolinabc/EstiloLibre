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

        internal Idioma? CargarIdioma(int iIdiomaId)
        {
            return this.GetDAOIdiomas().CargarIdioma(iIdiomaId);
        }

        internal Idioma? CargarIdioma(string strCodigo)
        {
            return this.GetDAOIdiomas().CargarIdiomaPorLogin(strCodigo);
        }

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

        #region Adjuntos

        public AdjuntosDAO GetDAOAdjuntos()
        {
            return new AdjuntosDAO(this);
        }

        public Adjunto CrearAdjunto()
        {
            return (Adjunto)this.GetDAOAdjuntos().CrearObjetoBD();
        }

        public Adjunto? CargarAdjunto(int iAdjuntoId)
        {
            return this.GetDAOAdjuntos().CargarAdjunto(iAdjuntoId);
        }

        public Adjuntos CargarAdjuntos(int iClaseObjetoId, int iObjetoId)
        {
            return this.GetDAOAdjuntos().CargarAdjuntos(iClaseObjetoId, iObjetoId);
        }
        #endregion
    }
}
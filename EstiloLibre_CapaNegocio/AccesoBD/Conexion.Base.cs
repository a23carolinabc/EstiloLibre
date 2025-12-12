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

        public Usuario? CargarUsuarioPorCorreo(string strCorreoE)
        {
            return this.GetDAOUsuarios().CargarUsuarioPorCorreo(strCorreoE);
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

        #region Permisos

        public PermisosDAO GetDAOPermisos()
        {
            return new PermisosDAO(this);
        }

        public Permiso CrearPermiso()
        {
            return (Permiso)this.GetDAOPermisos().CrearObjetoBD();
        }

        public Permiso? CargarPermiso(int iPermisoId)
        {
            return this.GetDAOPermisos().CargarPermiso(iPermisoId);
        }

        public Permiso? CargarPermisoPorCodigo(string strCodigo)
        {
            return this.GetDAOPermisos().CargarPermisoPorCodigo(strCodigo);
        }

        public Permisos CargarPermisos(List<int> lstPermisosIds)
        {
            return this.GetDAOPermisos().CargarPermisos(lstPermisosIds);
        }

        public Permisos CargarTodosLosPermisos()
        {
            return this.GetDAOPermisos().CargarTodosLosPermisos();
        }

        #endregion

        #region PermisosUsuarios

        public PermisosUsuariosDAO GetDAOPermisosUsuarios()
        {
            return new PermisosUsuariosDAO(this);
        }

        public PermisoUsuario CrearPermisoUsuario()
        {
            return (PermisoUsuario)this.GetDAOPermisosUsuarios().CrearObjetoBD();
        }

        public PermisoUsuario? CargarPermisoUsuario(int iPermisoUsuarioId)
        {
            return this.GetDAOPermisosUsuarios().CargarPermisoUsuario(iPermisoUsuarioId);
        }

        /// <summary>
        /// Carga los permisos asignados a un usuario
        /// </summary>
        public Permisos CargarPermisosUsuario(int iUsuarioId)
        {
            PermisosUsuariosDAO daoPermisosUsuarios;
            PermisosDAO daoPermisos;
            PermisosUsuarios permisosUsuarios;
            List<int> lstPermisosIds;
            Permisos permisos;

            daoPermisosUsuarios = this.GetDAOPermisosUsuarios();
            daoPermisos = this.GetDAOPermisos();

            // Cargar relaciones PermisoUsuario
            permisosUsuarios = daoPermisosUsuarios.CargarPermisosPorUsuario(iUsuarioId);

            // Obtener IDs de permisos
            lstPermisosIds = permisosUsuarios.Select(pu => pu.PermisoId).ToList();

            // Cargar objetos Permiso
            if (lstPermisosIds.Any())
            {
                permisos = daoPermisos.CargarPermisos(lstPermisosIds);
            }
            else
            {
                permisos = new Permisos();
            }

            return permisos;
        }

        /// <summary>
        /// Asigna una lista de permisos a un usuario
        /// </summary>
        public void AsignarPermisosAUsuario(int iUsuarioId, List<int> lstPermisosIds)
        {
            PermisoUsuario permisoUsuario;

            if (lstPermisosIds == null || !lstPermisosIds.Any())
            {
                return;
            }

            foreach (int iPermisoId in lstPermisosIds)
            {
                permisoUsuario = this.CrearPermisoUsuario();
                permisoUsuario.UsuarioId = iUsuarioId;
                permisoUsuario.PermisoId = iPermisoId;
                permisoUsuario.Guardar();
            }
        }

        /// <summary>
        /// Elimina todos los permisos de un usuario
        /// </summary>
        public void EliminarPermisosDeUsuario(int iUsuarioId)
        {
            PermisosUsuariosDAO dao;

            dao = this.GetDAOPermisosUsuarios();
            dao.EliminarPermisosPorUsuario(iUsuarioId);
        }

        #endregion
    }
}
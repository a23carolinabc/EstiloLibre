using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class PermisosUsuariosDAO : DAO<PermisoUsuario>
    {
        #region ***** CONSTRUCTORES *****

        public PermisosUsuariosDAO(Conexion conexion) : base(conexion, TablasBD.PermisosUsuarios) { }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public override ObjetoBD CrearObjetoBD()
        {
            return new PermisoUsuario(this);
        }

        public PermisoUsuario? CargarPermisoUsuario(int iPermisoUsuarioId)
        {
            return (PermisoUsuario?)this.CargarObjetoBD(iPermisoUsuarioId);
        }

        public PermisosUsuarios CargarPermisosPorUsuario(int iUsuarioId)
        {
            PermisosUsuarios permisosUsuarios;

            permisosUsuarios = new PermisosUsuarios(this.CargarObjetosBD($"UsuarioId = {iUsuarioId}"));

            return permisosUsuarios;
        }

        public void EliminarPermisosPorUsuario(int iUsuarioId)
        {
            PermisosUsuarios permisosUsuarios;

            permisosUsuarios = this.CargarPermisosPorUsuario(iUsuarioId);

            foreach (PermisoUsuario permisoUsuario in permisosUsuarios)
            {
                permisoUsuario.Eliminar();
            }
        }

        #endregion
    }
}

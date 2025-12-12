using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class PermisosDAO : DAO<Permiso>
    {
        #region ***** CONSTRUCTORES *****

        public PermisosDAO(Conexion conexion) : base(conexion, TablasBD.Permisos) { }

        #endregion

        #region ***** MÉTODOS PÚBLICOS *****

        public override ObjetoBD CrearObjetoBD()
        {
            return new Permiso(this);
        }

        public Permiso? CargarPermiso(int iPermisoId)
        {
            return (Permiso?)this.CargarObjetoBD(iPermisoId);
        }

        public Permiso? CargarPermisoPorCodigo(string strCodigo)
        {
            return (Permiso?)this.CargarObjetoBD($"Codigo = '{strCodigo}'");
        }

        public Permisos CargarPermisos(List<int> lstPermisosIds)
        {
            Permisos permisos;

            if (lstPermisosIds == null || !lstPermisosIds.Any())
            {
                return new Permisos();
            }

            permisos = new Permisos(this.CargarObjetosBD(lstPermisosIds));

            return permisos;
        }

        public Permisos CargarTodosLosPermisos()
        {
            Permisos permisos;

            permisos = new Permisos(this.CargarObjetosBD());

            return permisos;
        }

        #endregion
    }
}

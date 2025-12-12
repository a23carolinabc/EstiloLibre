using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.PermisosUsuarios)]
    public class PermisoUsuario : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public int UsuarioId { get; set; }
        public int PermisoId { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public PermisoUsuario() : base() { }

        public PermisoUsuario(DAO<PermisoUsuario> dao) : base(dao) { }

        #endregion
    }
}

using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Permisos)]
    public class Permiso : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Permiso() : base() { }

        public Permiso(DAO<Permiso> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

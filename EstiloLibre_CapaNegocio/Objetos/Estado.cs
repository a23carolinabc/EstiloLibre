using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Estados)]
    public class Estado : ObjetoBD
    {
        #region ***** PROPIEDADES *****
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Estado() : base() { }

        public Estado(DAO<Estado> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

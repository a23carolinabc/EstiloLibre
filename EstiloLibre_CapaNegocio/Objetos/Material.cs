using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Materiales)]
    public class Material : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Material() : base() { }

        public Material(DAO<Material> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

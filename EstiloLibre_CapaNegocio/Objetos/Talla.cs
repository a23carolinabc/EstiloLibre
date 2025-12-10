using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Tallas)]
    public class Talla : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }
        public int CodigoNumerico { get; set; }
        public string CodigoAlfabetico { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Talla() : base() { }

        public Talla(DAO<Talla> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

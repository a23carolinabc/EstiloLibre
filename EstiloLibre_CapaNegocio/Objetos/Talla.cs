using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
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

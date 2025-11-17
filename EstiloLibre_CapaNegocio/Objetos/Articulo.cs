using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Articulo : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public int MarcaId { get; set; }
        public decimal Precio { get; set; }
        public string EnlaceCompra { get; set; }
        public string RutaFoto { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Articulo() : base() { }

        public Articulo(DAO<Articulo> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

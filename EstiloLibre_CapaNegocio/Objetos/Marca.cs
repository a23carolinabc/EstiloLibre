using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Marca : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Marca() : base() { }

        public Marca(DAO<Marca> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

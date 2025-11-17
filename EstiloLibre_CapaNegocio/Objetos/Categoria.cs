using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Categoria : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Categoria() : base() { }

        public Categoria(DAO<Categoria> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

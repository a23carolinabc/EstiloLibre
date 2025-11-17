using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
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

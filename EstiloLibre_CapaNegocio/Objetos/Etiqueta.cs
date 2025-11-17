using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Etiqueta : ObjetoBD
    {
        #region ***** PROPIEDADES *****
        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Etiqueta() : base() { }

        public Etiqueta(DAO<Etiqueta> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

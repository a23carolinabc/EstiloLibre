using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Color : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Color() : base() { }

        public Color(DAO<Color> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

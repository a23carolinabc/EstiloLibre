using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Estacion : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Estacion() : base() { }

        public Estacion(DAO<Estacion> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

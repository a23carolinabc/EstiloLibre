using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Permiso : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Permiso() : base() { }

        public Permiso(DAO<Permiso> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

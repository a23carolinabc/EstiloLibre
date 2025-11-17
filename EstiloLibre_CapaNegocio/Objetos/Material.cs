using EstiloLibre_CapaNegocio.Base;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Material : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Material() : base() { }

        public Material(DAO<Material> objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

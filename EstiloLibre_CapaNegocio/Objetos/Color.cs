using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Colores)]
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

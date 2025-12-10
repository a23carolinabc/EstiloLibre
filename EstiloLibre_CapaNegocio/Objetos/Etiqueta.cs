using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Etiquetas)]
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

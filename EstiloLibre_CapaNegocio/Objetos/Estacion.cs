using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Estaciones)]
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

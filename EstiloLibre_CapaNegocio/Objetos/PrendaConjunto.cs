using Dapper.Contrib.Extensions;
using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.PrendasConjuntos)]
    public class PrendaConjunto : ObjetoBD
    {
        #region ****** PROPIEDADES *****

        public int ConjuntoId { get; set; }
        public int PrendaId { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public PrendaConjunto() : base() { }

        public PrendaConjunto(PrendasConjuntosDAO dao) : base(dao) { }

        #endregion
    }
}
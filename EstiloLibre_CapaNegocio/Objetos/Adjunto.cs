using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;

namespace EstiloLibre_CapaNegocio.Objetos
{
    public class Adjunto : ObjetoBD
    {
        #region ***** PROPIEDADES *****

        public string Guid { get; set;}
        public int ClaseObjetoId { get; set; }
        public int ObjetoId { get; set; }
        public int TipoAdjuntoId { get; set; }

        #endregion

        #region ***** CONSTRUCTORES *****

        public Adjunto() : base() { }

        public Adjunto(AdjuntosDAO objetoDAO) : base(objetoDAO) { }

        #endregion
    }
}

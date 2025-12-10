using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.DAOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstiloLibre_CapaNegocio.Objetos
{
    [Table(TablasBD.Adjuntos)]
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

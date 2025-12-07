using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class AdjuntosDAO : DAO<Adjunto>
    {
        #region ***** CONSTRUCTORES *****
        public AdjuntosDAO(Conexion conexion) : base(conexion, TablasBD.Adjuntos) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new Adjunto(this);
        }
        public Adjunto? CargarAdjunto(int iAdjuntoId)
        {
            return (Adjunto?)this.CargarObjetoBD(iAdjuntoId);
        }
        public Adjunto? CargarAdjunto(string strGuid)
        {
            return (Adjunto?)this.CargarObjetoBD($"Guid = '{strGuid}'");
        }
        #endregion
    }    
}

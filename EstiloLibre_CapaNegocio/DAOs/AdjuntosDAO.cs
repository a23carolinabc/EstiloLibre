using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;

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
        public Adjuntos CargarAdjuntos(int iClaseObjetoId, int iObjetoId)
        {
            Adjuntos adjuntos;
                       
            adjuntos = new(this.CargarObjetosBD($"ClaseObjetoId = {iClaseObjetoId} AND ObjetoId = {iObjetoId}"));

            return adjuntos;
        }
        #endregion
    }    
}

using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
using EstiloLibre_CapaNegocio.Excepciones;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class PrendasConjuntosDAO : DAO<PrendaConjunto>
    {
        #region ***** CONSTRUCTORES *****
        public PrendasConjuntosDAO(Conexion conexion) : base(conexion, TablasBD.PrendasConjuntos) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new PrendaConjunto(this);
        }
        public PrendaConjunto? CargarPrendaConjunto(int iPrendaConjuntoId)
        {
            return (PrendaConjunto?)this.CargarObjetoBD(iPrendaConjuntoId);
        }

        public PrendasConjuntos CargarPrendasConjuntos(int iConjuntoId)
        {
            PrendasConjuntos prendasConjuntos;

            prendasConjuntos = new(this.CargarObjetosBD($"ConjuntoId = {iConjuntoId}"));

            return prendasConjuntos;
        }

        public PrendasConjuntos CargarPrendasConjuntosPorPrenda(int iPrendaId)
        {
            PrendasConjuntos prendasConjuntos;

            prendasConjuntos = new(this.CargarObjetosBD($"PrendaId = {iPrendaId}"));

            return prendasConjuntos;
        }

        public PrendasConjuntos CargarPrendasConjuntosPorConjunto(int iConjuntoId)
        {
            // Alias para mayor claridad - hace lo mismo que CargarPrendasConjuntos
            return this.CargarPrendasConjuntos(iConjuntoId);
        }
        #endregion
    }    
}

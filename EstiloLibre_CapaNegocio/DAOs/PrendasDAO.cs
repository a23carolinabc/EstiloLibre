using EstiloLibre_CapaNegocio.AccesoBD;
using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;
using System.Data.Common;

namespace EstiloLibre_CapaNegocio.DAOs
{
    public class PrendasDAO : DAO<Prenda>
    {
        #region ***** CONSTRUCTORES *****
        public PrendasDAO(Conexion conexion) : base(conexion, TablasBD.Prendas) { }
        #endregion

        #region ***** MÉTODOS PÚBLICOS *****        

        public override ObjetoBD CrearObjetoBD()
        {
            return new Prenda(this);
        }
        public Prenda? CargarPrenda(int iPrendaId)
        {
            return (Prenda?)this.CargarObjetoBD(iPrendaId);
        }
        #endregion
    }    
}

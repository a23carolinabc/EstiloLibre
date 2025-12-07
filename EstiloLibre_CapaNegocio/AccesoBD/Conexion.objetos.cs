using EstiloLibre_CapaNegocio.DAOs;
using EstiloLibre_CapaNegocio.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstiloLibre_CapaNegocio.AccesoBD
{
    public partial class Conexion
    {
        #region Prendas

        public PrendasDAO GetDAOPrendas()
        {
            return new PrendasDAO(this);
        }

        public Prenda CrearPrenda()
        {
            return (Prenda)this.GetDAOPrendas().CrearObjetoBD();
        }

        public Prenda? CargarPrenda(int iPrendaId)
        {
            return this.GetDAOPrendas().CargarPrenda(iPrendaId);
        }

        #endregion
    }
}

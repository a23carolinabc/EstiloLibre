using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Colecciones;
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

        public Prendas CargarPrendas(int iUsuarioId)
        {
            return this.GetDAOPrendas().CargarPrendas(iUsuarioId);
        }

        #endregion

        #region Conjuntos

        public ConjuntosDAO GetDAOConjuntos()
        {
            return new ConjuntosDAO(this);
        }

        public Conjunto CrearConjunto()
        {
            return (Conjunto)this.GetDAOConjuntos().CrearObjetoBD();
        }

        public Conjunto? CargarConjunto(int iConjuntoId)
        {
            return this.GetDAOConjuntos().CargarConjunto(iConjuntoId);
        }

        public Conjuntos CargarConjuntos(int iUsuarioId)
        {
            return this.GetDAOConjuntos().CargarConjuntos(iUsuarioId);
        }
        #endregion

        #region PrendasConjuntos

        public PrendasConjuntosDAO GetDAOPrendasConjuntos()
        {
            return new PrendasConjuntosDAO(this);
        }

        public PrendaConjunto CrearPrendaConjunto()
        {
            return (PrendaConjunto)this.GetDAOPrendasConjuntos().CrearObjetoBD();
        }

        public PrendaConjunto? CargarPrendaConjunto(int iPrendaConjuntoId)
        {
            return this.GetDAOPrendasConjuntos().CargarPrendaConjunto(iPrendaConjuntoId);
        }

        public PrendasConjuntos CargarPrendasConjuntos(int iConjuntoId)
        {
            return this.GetDAOPrendasConjuntos().CargarPrendasConjuntos(iConjuntoId);
        }

        public PrendasConjuntos CargarPrendasConjuntosPorPrendas(int iPrendaId)
        {
            return this.GetDAOPrendasConjuntos().CargarPrendasConjuntosPorPrenda(iPrendaId);
        }

        #endregion
    }
}

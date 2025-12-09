using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class PrendasConjuntos : ListaObjetosBD<PrendaConjunto>
    {
        public PrendasConjuntos() : base() { }
        public PrendasConjuntos(int iCapacidadInicial) : base(iCapacidadInicial) { }
        public PrendasConjuntos(IEnumerable<PrendaConjunto> lista) : base(lista) { }
    }
}
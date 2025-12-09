using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Conjuntos : ListaObjetosBD<Conjunto>
    {
        public Conjuntos() : base() { }
        public Conjuntos(int iCapacidadInicial) : base(iCapacidadInicial) { }
        public Conjuntos(IEnumerable<Conjunto> lista) : base(lista) { }
    }
}
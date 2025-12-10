using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Prendas : ListaObjetosBD<Prenda>
    {
        public Prendas() : base() { }
        public Prendas(int iCapacidadInicial) : base(iCapacidadInicial) { }
        public Prendas(IEnumerable<Prenda> lista) : base(lista) { }
    }
}
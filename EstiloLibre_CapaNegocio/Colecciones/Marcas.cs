using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Marcas : ListaObjetosBD<Marca>
    {
        public Marcas() : base() { }
        public Marcas(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Marcas(IEnumerable<Marca> lista) : base(lista) { }
    }
}

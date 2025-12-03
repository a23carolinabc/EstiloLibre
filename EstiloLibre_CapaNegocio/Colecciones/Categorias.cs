using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Categorias : ListaObjetosBD<Categoria>
    {
        public Categorias() : base() { }
        public Categorias(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Categorias(IEnumerable<Categoria> lista) : base(lista) { }
    }
}

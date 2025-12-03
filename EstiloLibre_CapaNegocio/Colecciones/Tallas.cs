using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Tallas : ListaObjetosBD<Talla>
    {
        public Tallas() : base() { }
        public Tallas(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Tallas(IEnumerable<Talla> lista) : base(lista) { }
    }
}

using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Materiales : ListaObjetosBD<Material>
    {
        public Materiales() : base() { }
        public Materiales(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Materiales(IEnumerable<Material> lista) : base(lista) { }
    }
}

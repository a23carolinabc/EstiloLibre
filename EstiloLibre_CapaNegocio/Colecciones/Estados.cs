using EstiloLibre_CapaNegocio.Base;
using EstiloLibre_CapaNegocio.Objetos;

namespace EstiloLibre_CapaNegocio.Colecciones
{
    public class Estados : ListaObjetosBD<Estado>
    {
        public Estados() : base() { }
        public Estados(int iCapacidadeInicial) : base(iCapacidadeInicial) { }
        public Estados(IEnumerable<Estado> lista) : base(lista) { }
    }
}
